// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using Application.Demo.Languages;
using Application.Demo.UseCases.Menu.ViewModels;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.ContextMenu
{
    /// <summary>
    ///     This provides the IMenuItem to open the WindowWithMenuViewModel
    /// </summary>
    [Menu("contextmenu")]
    public sealed class SomeWindowMenuItems : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private static readonly LogSource Log = new LogSource();

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