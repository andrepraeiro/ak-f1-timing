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

namespace AK.F1.Timing.Serialization
{
    public class TypeIdAttributeTest
    {
        [Fact]
        public void ctor_should_set_the_id_property()
        {
            byte id = 10;
            var attribute = new TypeIdAttribute(id);

            Assert.Equal(id, attribute.Id);
        }

        [Fact]
        public void ctor_should_allow_entire_id_range()
        {
            Assert.DoesNotThrow(() => { new TypeIdAttribute(Byte.MinValue); });
            Assert.DoesNotThrow(() => { new TypeIdAttribute(Byte.MaxValue); });
        }
    }
}