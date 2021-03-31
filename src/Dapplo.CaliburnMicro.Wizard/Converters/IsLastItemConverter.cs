// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Dapplo.CaliburnMicro.Wizard.Converters
{
    /// <summary>
    ///     This converter is specially written for the WizardProgress
    /// </summary>
    public sealed class IsLastItemConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var contentPresenter = value as ContentPresenter;
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(contentPresenter);
            if (itemsControl == null)
            {
                return false;
            }
            var index = 0;
            if (contentPresenter != null)
            {
                index = itemsControl.ItemContainerGenerator?.IndexFromContainer(contentPresenter) ?? -1;
            }
            return index == itemsControl.Items.Count - 1;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}