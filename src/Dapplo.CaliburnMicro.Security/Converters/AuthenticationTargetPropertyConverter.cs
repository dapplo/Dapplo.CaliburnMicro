// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Data;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security.Converters
{
    /// <summary>
    ///     Returns AuthenticationTargetProperties from INeedAuthentication, or AuthenticationTargetProperties.None as a default
    /// </summary>
    public class AuthenticationTargetPropertyConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var needAuthentication = value as INeedAuthentication;
            return needAuthentication?.AuthenticationTargetProperty ?? AuthenticationTargetProperties.None;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}