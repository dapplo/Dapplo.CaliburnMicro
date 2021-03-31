// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using Dapplo.Windows.Dpi.Wpf;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that every window is DPI aware
    /// </summary>
    public class DpiAwareViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        /// <inheritdoc />
        public void ConfigureWindowView(Window view, object viewModel = null)
        {
            Configure(view, viewModel);
        }

        /// <inheritdoc />
        public void ConfigureDialogView(Window view, object viewModel = null)
        {
            Configure(view, viewModel);
        }

        private void Configure(Window view, object viewModel)
        {
            if (viewModel is IScaleWithDpiChanges)
            {
                // Make sure DPI handling is active
                view.AttachDpiHandler();
            }
        }
    }
}
