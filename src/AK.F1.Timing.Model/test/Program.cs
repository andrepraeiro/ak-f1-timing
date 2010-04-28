﻿// Copyright 2010 Andy Kernahan
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
using System.IO;

using AK.F1.Timing.Messages.Driver;
using AK.F1.Timing.Model.Session;

namespace AK.F1.Timing.Model
{
    public class Program
    {
        public static void Main(string[] args) {

            Message message;
            SessionModel session = new SessionModel();
            string path = @"..\..\..\..\..\tms\2010\04-china\race.tms";

            using(var reader = F1Timing.Playback.Read(path)) {
                reader.PlaybackSpeed = 5000000d;
                while((message = reader.Read()) != null) {
                    if(message is DriverMessageBase && !(message is SetGridColumnValueMessage)) {
                        //System.Diagnostics.Debugger.Break();
                    }
                    session.Process(message);
                }                
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}