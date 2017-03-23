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
using Application.Demo.Languages;
using Application.Demo.UseCases.Configuration.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

#endregion

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry for the configuration to the context menu
    /// </summary>
    [Export("contextmenu", typeof(IMenuItem))]
    public sealed class ConfigMenuItem : MenuItem
    {
        private static readonly LogSource Log = new LogSource();

        [Import]
        private IContextMenuTranslations ContextMenuTranslations { get; set; }

        [Import]
        public ConfigViewModel DemoConfigViewModel { get; set; }

        [Import]
        public IWindowManager WindowManager { get; set; }

        public override void Click(IMenuItem clickedItem)
        {
            Log.Debug().WriteLine("Configure");
            // TODO: Closing the DemoConfigViewModel also closes other windows, check / fix
            if (WindowManager.ShowDialog(DemoConfigViewModel) == false)
            {
                Log.Warn().WriteLine("You cancelled the configuration");
            }
        }

        public override void Initialize()
        {
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Settings,
                Spin = true,
                SpinDuration = 3
            };
            HotKeyHint = "Alt+C";
            // automatically update the DisplayName
            ContextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Configure));
        }
    }
}