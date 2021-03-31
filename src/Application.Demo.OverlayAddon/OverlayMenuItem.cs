// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Menu;
using Application.Demo.OverlayAddon.ViewModels;
using Caliburn.Micro;
using System.Windows;
using Dapplo.CaliburnMicro.Security;

namespace Application.Demo.OverlayAddon
{
    /// <summary>
    ///     This will add an extry for the exit to the context menu
    /// </summary>
    [Menu("contextmenu")]
    public sealed class OverlayMenuItem : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private readonly IWindowManager _windowManager;
        private readonly DemoOverlayContainerViewModel _demoOverlayContainerViewModel;

        public OverlayMenuItem(
            IWindowManager windowManager,
            DemoOverlayContainerViewModel demoOverlayContainerViewModel)
        {
            _windowManager = windowManager;
            _demoOverlayContainerViewModel = demoOverlayContainerViewModel;

            this.VisibleOnPermissions("Admin");
        }

        public override void Click(IMenuItem clickedItem)
        {
            _windowManager.ShowWindow(_demoOverlayContainerViewModel);
        }

        public override void Initialize()
        {
            Id = "Y_Overlay";
            // automatically update the DisplayName
            DisplayName = "Show Overlay";
        }
    }
}