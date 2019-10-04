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

using Dapplo.CaliburnMicro.Menu;
using Application.Demo.OverlayAddon.ViewModels;
using Caliburn.Micro;
using System.Windows;
using Dapplo.CaliburnMicro.Security;

namespace Application.Demo.OverlayAddon
{
    /// <summary>
    ///     This will add an extry for the exit to the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class OverlayMenuItem : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private readonly IWindowManager _windowManager;
        private readonly DemoOverlayContainerViewModel _demoOverlayContainerViewModel;

        public OverlayMenuItem(
            IWindowManager windowManager,
            DemoOverlayContainerViewModel demoOverlayContainerViewModel)
        {
            _windowManager = windowManager;
            _demoOverlayContainerViewModel = demoOverlayContainerViewModel;

            this.VisibleOnPermissions("Admin");
        }

        public override void Click(IMenuItem clickedItem)
        {
            _windowManager.ShowWindow(_demoOverlayContainerViewModel);
        }

        public override void Initialize()
        {
            Id = "Y_Overlay";
            // automatically update the DisplayName
            DisplayName = "Show Overlay";
        }
    }
}