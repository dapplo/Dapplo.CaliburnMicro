// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Data;

namespace Dapplo.CaliburnMicro.Converters
{
    /// <summary>
    /// This can be used to test if a value is a type which implements a certain interface
    /// </summary>
    public class HasInterfaceConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            var typeToTest = value.GetType();
            var interfaceType = parameter as Type;
            return interfaceType?.IsAssignableFrom(typeToTest) == true;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
