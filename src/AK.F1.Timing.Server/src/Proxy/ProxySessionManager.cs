﻿// Copyright 2011 Andy Kernahan
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AK.F1.Timing.Serialization;
using AK.F1.Timing.Server.Extensions;
using AK.F1.Timing.Server.IO;
using AK.F1.Timing.Utility;
using log4net;

namespace AK.F1.Timing.Server.Proxy
{
    /// <summary>
    /// A <see cref="AK.F1.Timing.Server.ISocketHandler"/> which creates and manages a
    /// <see cref="AK.F1.Timing.Server.Proxy.ProxySession"/> for each connected client.
    /// This class cannot be inherited.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ProxySessionManager : DisposableBase, ISocketHandler
    {
        #region Fields.

        private readonly string _username;
        private readonly string _password;

        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Task _dispatchMessagesTask;
        private readonly ManualResetEventSlim _dispatchTaskDrainedQueueEvent = new ManualResetEventSlim();
        // One MiB should be sufficient for most sessions.
        private readonly ByteBuffer _messageHistory = new ByteBuffer(1024 * 1024);
        private readonly ReaderWriterLockSlim _messageHistoryLock = new ReaderWriterLockSlim();

        private Task _readMessagesTask;
        // TODO consider specifying a bounded capacity.
        private readonly BlockingCollection<byte[]> _readMessageQueue = new BlockingCollection<byte[]>();

        private int _nextSessionId;
        private readonly IDictionary<int, ProxySession> _sessions = new Dictionary<int, ProxySession>();
        private readonly ManualResetEventSlim _sessionsEmptiedEvent = new ManualResetEventSlim(true);
        private readonly ReaderWriterLockSlim _sessionsLock = new ReaderWriterLockSlim();

        private static readonly ILog Log = LogManager.GetLogger(typeof(ProxySessionManager));

        #endregion

        #region Public Interface.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">The user's F1 live-timing username.</param>
        /// <param name="password">The user's F1 live-timing password.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="username"/> or <paramref name="password"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="username"/> or <paramref name="password"/> is empty.
        /// </exception>
        public ProxySessionManager(string username, string password)
        {
            Guard.NotNullOrEmpty(username, "username");
            Guard.NotNullOrEmpty(password, "password");

            _username = username;
            _password = password;
            _cancellationToken = _cancellationTokenSource.Token;
            _dispatchMessagesTask = new Task(DispatchMessages, _cancellationToken);
            _dispatchMessagesTask.Start();
            _readMessagesTask = new Task(ReadMessages, _cancellationToken);
            _readMessagesTask.Start();
        }

        /// <inheritdoc/>
        public void Handle(Socket client)
        {
            CheckDisposed();
            Guard.NotNull(client, "client");

            StartSession(client);
        }

        #endregion

        #region Protected Interface.

        /// <inheritdoc/>
        protected override void DisposeOfManagedResources()
        {
            Log.Info("stopping");
            _cancellationTokenSource.Cancel();
            Task.WaitAll(_dispatchMessagesTask, _readMessagesTask);
            DisposeOf(_cancellationTokenSource);
            DisposeOf(_dispatchMessagesTask);
            DisposeOf(_readMessagesTask);
            Log.Info("stopping sessions");
            ForEachSession(DisposeOf, throwIfCancellationRequested: false);
            _sessionsEmptiedEvent.Wait();
            DisposeOf(_sessionsEmptiedEvent);
            DisposeOf(_sessionsLock);
            DisposeOf(_readMessageQueue);
            DisposeOf(_messageHistoryLock);
            Log.Info("stopped");
        }

        #endregion

        #region Private Impl.

        private void ReadMessages()
        {
            Log.Info("read task started");
            try
            {
                using(var reader = F1Timing.Live.Read(F1Timing.Live.Login(_username, _password)))
                //using(var reader = F1Timing.Playback.Read(@"D:\dev\.net\src\ak-f1-timing\tms\2011\11-hungary\race.tms"))
                using(var buffer = new MemoryStream(1024))
                using(var writer = new DecoratedObjectWriter(buffer))
                {
                    Message message;
                    do
                    {
                        ThrowIfCancellationRequested();
                        message = reader.Read();
                        buffer.SetLength(0L);
                        writer.WriteMessage(message);
                        _readMessageQueue.Add(buffer.ToArray());
                    } while(message != null);
                }
            }
            catch(OperationCanceledException) { }
            catch(Exception exc)
            {
                Log.Fatal(exc);
            }
            finally
            {
                _readMessageQueue.CompleteAdding();
                Log.Info("read task stopped");
            }
        }

        private void DispatchMessages()
        {
            Log.Info("dispatch task started");
            try
            {
                byte[] buffer;
                while(_readMessageQueue.TryTake(out buffer, Timeout.Infinite, _cancellationToken))
                {
                    _messageHistoryLock.InWriteLock(() =>
                    {
                        _messageHistory.Append(buffer);
                        ForEachSession(session => session.SendAsync(buffer));
                    });
                }
                _dispatchTaskDrainedQueueEvent.Set();
                ForEachSession(session => session.CompleteAsync());
            }
            catch(OperationCanceledException) { }
            catch(Exception exc)
            {
                Log.Fatal(exc);
            }
            finally
            {
                Log.Info("dispatch task stopped");
            }
        }

        private void StartSession(Socket client)
        {
            var session = CreateSession(client);
            // Lock order is critical to prevent duplicate message buffers from being sent.
            _messageHistoryLock.InReadLock(() =>
            {
                _sessionsLock.InWriteLock(() =>
                {
                    _sessions.Add(session.Id, session);
                    session.SendAsync(_messageHistory.CreateSnapshot());
                    if(_dispatchTaskDrainedQueueEvent.IsSet)
                    {
                        session.CompleteAsync();
                    }
                    Log.InfoFormat("started, id={0}, endpoint={1}, open={2}",
                        session.Id, client.RemoteEndPoint, _sessions.Count);
                    _sessionsEmptiedEvent.Reset();
                });
            });
        }

        private ProxySession CreateSession(Socket client)
        {
            var session = new ProxySession(NextSessionId(), client);
            session.Disposed += OnSessionDisposed;
            return session;
        }

        private void OnSessionDisposed(object sender, EventArgs e)
        {
            var session = (ProxySession)sender;
            session.Disposed -= OnSessionDisposed;
            _sessionsLock.InWriteLock(() =>
            {
                _sessions.Remove(session.Id);
                Log.InfoFormat("removed, id={0}, open={1}", session.Id, _sessions.Count);
                if(_sessions.Count == 0)
                {
                    _sessionsEmptiedEvent.Set();
                }
            });
        }

        private void ForEachSession(Action<ProxySession> action, bool throwIfCancellationRequested = true)
        {
            _sessionsLock.InReadLock(() =>
            {
                foreach(var session in _sessions)
                {
                    if(throwIfCancellationRequested)
                    {
                        ThrowIfCancellationRequested();
                    }
                    try
                    {
                        action(session.Value);
                    }
                    catch(ObjectDisposedException)
                    {
                        // The session may have been disposed of whilst we hold the read lock and
                        // therefore couldn't remove itself from the collection.
                    }
                }
            });
        }

        private void ThrowIfCancellationRequested()
        {
            _cancellationToken.ThrowIfCancellationRequested();
        }

        private int NextSessionId()
        {
            // TODO not sure what to do on overflow, seems unlikely...
            return Interlocked.Increment(ref _nextSessionId);
        }

        #endregion
    }
}
