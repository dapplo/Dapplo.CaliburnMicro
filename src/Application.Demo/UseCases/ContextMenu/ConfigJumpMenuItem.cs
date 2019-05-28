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

#region using

using System.Windows;
using System.Windows.Media;
using Application.Demo.Languages;
using Application.Demo.UseCases.Configuration.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using MahApps.Metro.IconPacks;

#endregion

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
                Kind = PackIconMaterialKind.Settings,
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