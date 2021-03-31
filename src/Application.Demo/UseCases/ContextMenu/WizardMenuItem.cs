// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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