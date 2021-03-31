// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Media;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry for the title of the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class TitleMenuItem : MenuItem
    {
        /// <summary>
        /// Configure the title menu item
        /// </summary>
        /// <param name="contextMenuTranslations">IContextMenuTranslations</param>
        public TitleMenuItem(IContextMenuTranslations contextMenuTranslations)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Title));
            Id = "A_Title";
            Style = MenuItemStyles.Title;

            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Exclamation
            };
            this.ApplyIconForegroundColor(Brushes.DarkRed);
        }
    }
}