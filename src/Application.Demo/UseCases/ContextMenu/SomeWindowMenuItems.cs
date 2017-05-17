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

using System.ComponentModel.Composition;
using System.Windows;
using Application.Demo.Languages;
using Application.Demo.UseCases.Menu.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

#endregion

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This provides the IMenuItem to open the WindowWithMenuViewModel
    /// </summary>
    [Export("contextmenu", typeof(IMenuItem))]
    public sealed class SomeWindowMenuItems : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private static readonly LogSource Log = new LogSource();

        [ImportingConstructor]
        public SomeWindowMenuItems(
            IWindowManager windowManager,
            IContextMenuTranslations contextMenuTranslations,
            WindowWithMenuViewModel windowWithMenuViewModel)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.SomeWindow));

            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.ViewList
            };
            ClickAction = clickedItem =>
            {
                Log.Debug().WriteLine("SomeWindow");
                windowManager.ShowWindow(windowWithMenuViewModel);
            };

            this.VisibleOnPermissions("Admin");

        }
    }
}