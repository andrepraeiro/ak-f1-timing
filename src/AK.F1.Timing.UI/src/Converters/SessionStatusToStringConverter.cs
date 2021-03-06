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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using AK.F1.Timing.Messages.Session;

namespace AK.F1.Timing.UI.Converters
{
    /// <summary>
    /// This class cannot be inherited.
    /// </summary>
    [ValueConversion(typeof(SessionStatus), typeof(string))]
    public sealed class SessionStatusToStringConverter : IValueConverter
    {
        #region Public Interface.

        /// <ineritdoc/>        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return Brushes.White;
            }

            switch((SessionStatus)value)
            {
                case SessionStatus.Finished:
                    return "Finished";
                case SessionStatus.Green:
                    return "Green";
                case SessionStatus.Yellow:
                    return "Yellow";
                case SessionStatus.SafetyCarOnStandBy:
                    return "SCS";
                case SessionStatus.SafetyCarDeployed:
                    return "SCD";
                case SessionStatus.Red:
                    return "Red";
                default:
                    throw Guard.ArgumentOutOfRange("value");
            }
        }

        /// <summary>
        /// This method always throws a <see cref="System.NotImplementedException"/>.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}