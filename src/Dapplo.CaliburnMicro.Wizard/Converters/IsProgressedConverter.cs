// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Dapplo.CaliburnMicro.Wizard.ViewModels;

namespace Dapplo.CaliburnMicro.Wizard.Converters
{
    /// <summary>
    ///     This converter is specially written for the WizardProgress
    /// </summary>
    public sealed class IsProgressedConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] is ContentPresenter && values[1] is int) == false)
            {
                return Visibility.Collapsed;
            }
            var checkNextItem = Convert.ToBoolean(parameter.ToString());
            var contentPresenter = (ContentPresenter) values[0];
            var progress = (int) values[1];
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(contentPresenter);

            var index = itemsControl.ItemContainerGenerator.IndexFromContainer(contentPresenter);
            if (checkNextItem)
            {
                index++;
            }

            if (!(itemsControl.DataContext is WizardProgressViewModel wizardProgressViewModel))
            {
                return Visibility.Collapsed;
            }

            var percent = (int) ((double) index / wizardProgressViewModel.Wizard.WizardScreens.Count(x => x.IsVisible) * 100);
            return percent < progress ? Visibility.Visible : Visibility.Collapsed;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}