// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry which shows children to the context menu
    /// This example is verbose, meaning the actualy methods are implemented vs. everything in the constructor
    /// </summary>
    [Menu("contextmenu")]
    public sealed class WithChildrenMenuItem : ClickableMenuItem
    {
        private static readonly LogSource Log = new LogSource();

        private readonly IContextMenuTranslations _contextMenuTranslations;

        public WithChildrenMenuItem(
            IContextMenuTranslations contextMenuTranslations            )
        {
            _contextMenuTranslations = contextMenuTranslations;
        }

        public override void Click(IMenuItem clickedItem)
        {
            Log.Debug().WriteLine("From base: child {0} clicked", clickedItem.Id);
        }

        public override void Initialize()
        {
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.HumanChild
            };
            // automatically update the DisplayName
            var binding = _contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.WithChildren));
            var menuItem = new MenuItem
            {
                Id = "1"
            };
            ChildNodes.Add(menuItem);

            binding.AddDisplayNameBinding(menuItem, nameof(IContextMenuTranslations.One));

            ChildNodes.Add(new MenuItem {Style = MenuItemStyles.Separator});

            menuItem = new ClickableMenuItem
            {
                Id = "2",
                ClickAction = item => Log.Debug().WriteLine("From 2 - child {0} clicked", item.Id)
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