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

namespace AK.F1.Timing.Messages.Weather
{
    public class SetWindSpeedMessageTest : MessageTestBase<SetWindSpeedMessage>
    {
        [Fact]
        public override void can_visit()
        {
            var message = CreateTestMessage();
            var visitor = CreateMockMessageVisitor();

            visitor.Setup(x => x.Visit(message));
            message.Accept(visitor.Object);
            visitor.VerifyAll();
        }

        [Fact]
        public void ctor_throws_if_speed_is_negative()
        {
            Assert.DoesNotThrow(() => { new SetWindSpeedMessage(0D); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { new SetWindSpeedMessage(-1D); });
        }

        protected override void AssertEqualsTestMessage(SetWindSpeedMessage message)
        {
            Assert.Equal(1D, message.Speed);
        }

        protected override SetWindSpeedMessage CreateTestMessage()
        {
            return new SetWindSpeedMessage(1D);
        }
    }
}