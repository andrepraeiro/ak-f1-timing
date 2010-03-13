// Copyright 2009 Andy Kernahan
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
using Xunit;

using AK.F1.Timing.Messages.Driver;

namespace AK.F1.Timing.Messages.Driver
{
    public class SetDriverCarNumberMessageTest  : MessageTestBase<SetDriverCarNumberMessage>
    {
        [Fact]
        public override void can_create() {

            var message = CreateMessage();

            Assert.Equal(1, message.CarNumber);
            Assert.Equal(1, message.DriverId);            
        }

        [Fact]
        public override void can_visit() {

            var message = CreateMessage();
            var visitor = CreateMockMessageVisitor();

            visitor.Setup(x => x.Visit(message));
            message.Accept(visitor.Object);
            visitor.VerifyAll();
        }

        [Fact]
        public void ctor_throws_if_car_number_is_not_positive() {

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new SetDriverCarNumberMessage(1, 0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                new SetDriverCarNumberMessage(1, -1);
            });
        }

        protected override SetDriverCarNumberMessage CreateMessage() {

            return new SetDriverCarNumberMessage(1, 1);
        }
    }
}