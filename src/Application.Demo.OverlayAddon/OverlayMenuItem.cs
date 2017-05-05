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

#region using

using Dapplo.CaliburnMicro.Menu;
using System.ComponentModel.Composition;
using Caliburn.Micro;

#endregion

namespace Application.Demo.OverlayAddon
{
    /// <summary>
    ///     This will add an extry for the exit to the context menu
    /// </summary>
    [Export("contextmenu", typeof(IMenuItem))]
    public sealed class OverlayMenuItem : MenuItem
    {
        [Import]
        private DemoOverlayContainerViewModel OverlayContainerViewModel { get; set; }

        [Import]
        private IWindowManager WindowManager { get; set; }

        public override void Click(IMenuItem clickedItem)
        {
            WindowManager.ShowWindow(OverlayContainerViewModel);
        }

        public override void Initialize()
        {
            Id = "Y_Overlay";
            // automatically update the DisplayName
            DisplayName = "Show Overlay";
        }
    }
}