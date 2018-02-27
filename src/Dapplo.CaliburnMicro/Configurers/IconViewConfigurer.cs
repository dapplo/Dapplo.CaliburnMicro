//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

using Dapplo.CaliburnMicro.Behaviors;
using System.ComponentModel.Composition;
using System.Windows;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that windows become their icons
    /// </summary>
    [Export(typeof(IConfigureWindowViews))]
    [Export(typeof(IConfigureDialogViews))]
    public class IconViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        /// <inheritdoc />
        public void ConfigureWindowView(Window view, object viewModel = null)
        {
            Configure(view, viewModel);
        }

        /// <inheritdoc />
        public void ConfigureDialogView(Window window, object model = null)
        {
            Configure(window, model);
        }

        private void Configure(Window window, object model = null)
        {
            if (model is IHaveIcon haveIcon && window.Icon == null)
            {
                // Now use the attached behavior to set the icon
                window.SetCurrentValue(FrameworkElementIcon.ValueProperty, haveIcon.Icon);
            }
        }
    }
}
