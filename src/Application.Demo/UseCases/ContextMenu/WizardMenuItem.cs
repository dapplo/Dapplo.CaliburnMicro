//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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

using Application.Demo.Languages;
using Application.Demo.UseCases.Wizard.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This will add an entry for the wizard to the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class WizardMenuItem : AuthenticatedMenuItem<IMenuItem, bool>
    {
        public WizardMenuItem(
            IContextMenuTranslations contextMenuTranslations,
            IWindowManager windowManager,
            WizardExampleViewModel wizardExample)
        {
            // automatically update the DisplayName
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Wizard));
            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.AutoFix
            };
            ClickAction = clickedItem =>
            {
                windowManager.ShowDialog(wizardExample);
            };
            this.EnabledOnPermissions("Admin");
        }
    }
}