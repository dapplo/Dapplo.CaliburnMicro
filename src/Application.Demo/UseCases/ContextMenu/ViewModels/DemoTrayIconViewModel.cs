// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Application.Demo.Languages;
using Autofac.Features.AttributeFilters;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu.ViewModels
{
    /// <summary>
    ///     Implementation of the system-tray icon
    /// </summary>
    public class DemoTrayIconViewModel : TrayIconViewModel
    {
        private readonly IEnumerable<Lazy<IMenuItem>> _contextMenuItems;

        private readonly IContextMenuTranslations _contextMenuTranslations;

        public DemoTrayIconViewModel(
            ITrayIconManager trayIconManager,
            [MetadataFilter("Menu", "contextmenu")]
            IEnumerable<Lazy<IMenuItem>> contextMenuItems,
            IContextMenuTranslations contextMenuTranslations) : base(trayIconManager)
        {
            _contextMenuItems = contextMenuItems;
            _contextMenuTranslations = contextMenuTranslations;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // Set the title of the icon (the ToolTipText) to our IContextMenuTranslations.Title
            _contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Title));

            var items = new List<IMenuItem>();

            // Lazy values
            items.AddRange(_contextMenuItems.Select(lazy => lazy.Value));

            items.Add(new MenuItem
            {
                Style = MenuItemStyles.Separator,
                Id = "Y_Separator"
            });
            ConfigureMenuItems(items);

            // Make sure the margin is set, do this AFTER the icon are set
            items.ApplyIconMargin(new Thickness(2, 2, 2, 2));

            SetIcon(new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Apps,
                Background = Brushes.White,
                Foreground = Brushes.Black
            });
            Show();
        }
    }
}