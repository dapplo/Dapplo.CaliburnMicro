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

using System.ComponentModel.Composition;
using System.Windows;
using Dapplo.Windows.Dpi.Wpf;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that every window is DPI aware
    /// </summary>
    [Export(typeof(IConfigureWindowViews))]
    [Export(typeof(IConfigureDialogViews))]
    public class DpiAwareViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        /// <inheritdoc />
        public void ConfigureWindowView(Window view, object viewModel = null)
        {
            Configure(view);
        }

        /// <inheritdoc />
        public void ConfigureDialogView(Window view, object viewModel = null)
        {
            Configure(view);
        }

        private void Configure(Window view)
        {
            // Make sure DPI handling is done correctly
            view.AttachWindowDpiHandler();
        }
    }
}
