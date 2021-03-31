// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Behaviors;
using System.Windows;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that windows become their icons
    /// </summary>
    public class IconViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
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

        private void Configure(Window window, object viewModel = null)
        {
            if (viewModel is IHaveIcon haveIcon && window.Icon == null)
            {
                // Now use the attached behavior to set the icon
                window.SetCurrentValue(FrameworkElementIcon.ValueProperty, haveIcon.Icon);
            }
        }
    }
}
