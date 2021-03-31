// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that every window is using the current culture for binding
    /// </summary>
    public class CultureViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        /// <inheritdoc />
        public void ConfigureWindowView(Window view, object viewModel = null)
        {
            Configure(view);
        }

        /// <inheritdoc />
        public void ConfigureDialogView(Window view, object viewModel = null)
        {
            Configure(view);
        }

        private void Configure(Window view)
        {
            // Make sure the current culture is used by default for binding / formatting
            view.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
        }
    }
}
