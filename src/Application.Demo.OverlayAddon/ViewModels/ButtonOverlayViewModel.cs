// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using Dapplo.CaliburnMicro.Overlays;
using Dapplo.CaliburnMicro.Overlays.ViewModels;

namespace Application.Demo.OverlayAddon.ViewModels
{
    [Overlay("demo")]
    public sealed class ButtonOverlayViewModel : OverlayViewModel
    {
        public ButtonOverlayViewModel()
        {
            Left = 400;
            Top = 300;
        }

        public void GotMe()
        {
            MessageBox.Show("You got me!");
        }
    }
}
