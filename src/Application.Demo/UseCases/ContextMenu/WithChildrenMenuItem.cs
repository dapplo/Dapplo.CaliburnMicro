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
    ///     This will add an extry which shows children to the context menu
    /// This example is verbose, meaning the actualy methods are implemented vs. everything in the constructor
    /// </summary>
    [Export("contextmenu", typeof(IMenuItem))]
    public sealed class WithChildrenMenuItem : ClickableMenuItem
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
            Log.Debug().WriteLine("child {0} clicked", clickedItem.Id);
        }

        public override void Initialize()
        {
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.HumanChild
            };
            // automatically update the DisplayName
            var binding = ContextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.WithChildren));
            var menuItem = new MenuItem
            {
                Id = "1"
            };
            ChildNodes.Add(menuItem);

            binding.AddDisplayNameBinding(menuItem, nameof(IContextMenuTranslations.One));

            ChildNodes.Add(new MenuItem {Style = MenuItemStyles.Separator});

            menuItem = new ClickableMenuItem
            {
                Id = "2"
            };
            binding.AddDisplayNameBinding(menuItem, nameof(IContextMenuTranslations.Two));
            ChildNodes.Add(menuItem);

            menuItem = new MenuItem
            {
                Id = "3"
            };
            binding.AddDisplayNameBinding(menuItem, nameof(IContextMenuTranslations.Three));
            ChildNodes.Add(menuItem);
        }
    }
}