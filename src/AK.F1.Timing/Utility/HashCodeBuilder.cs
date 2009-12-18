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

namespace AK.F1.Timing.Utility
{
    /// <summary>
    /// Allows hash code to be easily built.
    /// </summary>
    public struct HashCodeBuilder : IEquatable<HashCodeBuilder>
    {
        #region Fields.

        private int _hashCode;

        #endregion

        #region Public Interface.

        /// <summary>
        /// Creates and initialises a new <see cref="HashCodeBuilder"/>.
        /// </summary>
        /// <returns>A a new <see cref="HashCodeBuilder"/></returns>
        public static HashCodeBuilder New() {

            return new HashCodeBuilder {
                _hashCode = 7
            };
        }

        /// <summary>
        /// Adds the hash code of the specified object to the builder.
        /// </summary>
        /// <param name="obj">The object whose hash code will be added to the builer.</param>
        /// <returns>This builder.</returns>
        public HashCodeBuilder Add<T>(T obj) {

            if(obj != null) {
                _hashCode = 31 * _hashCode + obj.GetHashCode();
            }

            return this;
        }

        /// <inhetitdoc/>
        public override bool Equals(object obj) {

            if(obj == null || obj.GetType() != GetType()) {
                return false;
            }
            return Equals((HashCodeBuilder)obj);
        }

        /// <inhetitdoc/>
        public override bool Equals(HashCodeBuilder other) {
            
            return other._hashCode == _hashCode;
        }

        /// <inhetitdoc/>        
        public override int GetHashCode() {

            return _hashCode;
        }

        /// <inhetitdoc/>
        public override string ToString() {

            return _hashCode.ToString();
        }

        /// <summary>
        /// Converts the specified <paramref name="builder"/> to an <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="builder">The builder to convert.</param>
        /// <returns>The specified <paramref name="builder"/> to an <see cref="System.Int32"/>.</returns>
        public static implicit operator int(HashCodeBuilder builder) {

            return builder._hashCode;
        }

        #endregion
    }
}
