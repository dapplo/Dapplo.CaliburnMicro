//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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