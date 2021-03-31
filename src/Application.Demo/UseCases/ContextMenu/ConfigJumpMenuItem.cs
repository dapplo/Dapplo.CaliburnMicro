// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Media;
using Application.Demo.Languages;
using Application.Demo.UseCases.Configuration.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an extry for the title of the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class ConfigJumpMenuItem : ClickableMenuItem
    {
        public ConfigJumpMenuItem(
            IContextMenuTranslations contextMenuTranslations,
            IWindowManager windowManager,
            ConfigViewModel configViewModel,
            LanguageConfigViewModel languageConfigViewModel
            )
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.JumpToConfigure));
            Id = "G_ConfigJump";
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Cog,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
            ClickAction = clickedItem =>
            {
                configViewModel.CurrentConfigScreen = languageConfigViewModel;
                if (!configViewModel.IsActive)
                {
                    windowManager.ShowDialog(configViewModel);
                }
            };
            this.ApplyIconForegroundColor(Brushes.Gray);
        }
    }
}