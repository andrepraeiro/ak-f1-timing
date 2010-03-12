﻿// Copyright 2009 Andy Kernahan
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

using AK.F1.Timing.Messages.Driver;
using AK.F1.Timing.Messages.Feed;
using AK.F1.Timing.Messages.Session;
using AK.F1.Timing.Messages.Weather;

namespace AK.F1.Timing.Fixup
{
    internal class MessageClassifier : IMessageVisitor
    {
        #region Public Interface.

        public bool IsTranslated(Message message) {

            this.Result = false;

            message.Accept(this);

            return this.Result;
        }

        public void Visit(SetSessionStatusMessage message) {

            // The live message reader does not receive a Finished status, the ping interval message
            // is translated when appropiate.
            this.Result = message.SessionStatus == SessionStatus.Finished;
        }

        public void Visit(ReplaceDriverLapTimeMessage message) {

            this.Result = true;
        }

        public void Visit(ReplaceDriverSectorTimeMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverCarNumberMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverCompletedLapsMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverGapMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverIntervalMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverLapNumberMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverLapTimeMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverNameMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverPitCountMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverQuallyTimeMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverSectorTimeMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverStatusMessage message) {

            this.Result = true;
        }

        public void Visit(SetRaceLapNumberMessage message) {

            this.Result = true;
        }

        public void Visit(EndOfSessionMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverPitTimeMessage message) {

            this.Result = true;
        }

        public void Visit(SetDriverPositionMessage message) { }

        public void Visit(StartSessionTimeCountdownMessage message) { }

        public void Visit(StopSessionTimeCountdownMessage message) { }

        public void Visit(ClearGridRowMessage message) { }

        public void Visit(SetGridColumnColourMessage message) { }

        public void Visit(SetGridColumnValueMessage message) { }

        public void Visit(SetElapsedSessionTimeMessage message) { }

        public void Visit(SetAirTemperatureMessage message) { }

        public void Visit(SetAtmosphericPressureMessage message) { }

        public void Visit(AddCommentaryMessage message) { }

        public void Visit(SetCopyrightMessage message) { }

        public void Visit(SetKeyframeMessage message) { }

        public void Visit(SetHumidityMessage message) { }

        public void Visit(SetRemainingSessionTimeMessage message) { }

        public void Visit(SetPingIntervalMessage message) { }

        public void Visit(SetSystemMessageMessage message) { }

        public void Visit(SetSessionTypeMessage message) { }

        public void Visit(SetStreamValidityMessage message) { }

        public void Visit(SetTrackTemperatureMessage message) { }

        public void Visit(SetIsWetMessage message) { }

        public void Visit(SetWindAngleMessage message) { }

        public void Visit(SetWindSpeedMessage message) { }

        public void Visit(SetNextMessageDelayMessage message) { }

        #endregion

        #region Private Impl.

        private bool Result { get; set; }

        #endregion
    }
}
