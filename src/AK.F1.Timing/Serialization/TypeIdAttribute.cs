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

namespace AK.F1.Timing.Serialization
{
    /// <summary>
    /// Specifies the identifier of the decorated type. This class is <see langword="sealed"/>.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TypeIdAttribute : Attribute
    {
        #region Public Interface.

        /// <summary>
        /// Initialises a new instance of the <see cref="TypeIdAttribute"/> class and specifies
        /// the identifier of the decorated type.
        /// </summary>
        /// <param name="id">The identifier of the decorated type.</param>
        public TypeIdAttribute(int id) {

            this.Id = id;      
        }

        /// <summary>
        /// Returns a value indicating if the <see cref="TypeIdAttribute"/> is applied to the
        /// specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true"/> if the attribute is applied to the specified type,
        /// otherwise; <see langword="false"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        public static bool IsDefined(Type type) {

            Guard.NotNull(type, "type");

            return Attribute.IsDefined(type, typeof(TypeIdAttribute), false);
        }

        /// <summary>
        /// Gets identifier of the decorated type.
        /// </summary>
        public int Id { get; private set; }

        #endregion
    }
}