// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Dapplo.CaliburnMicro.Configuration;

namespace Dapplo.CaliburnMicro.Metro
{
    /// <summary>
    ///     This (slightly modified) comes from
    ///     <a href="https://github.com/ziyasal/Caliburn.Metro/blob/master/Caliburn.Metro.Core/MetroWindowManager.cs">here</a>
    ///     and providers a Caliburn.Micro IWindowManager implementation. The Dapplo.CaliburnMicro.CaliburnMicroBootstrapper
    ///     will
    ///     take care of taking this (if available) and the MetroWindowManager will take care of instantiating a MetroWindow.
    ///     Note: Currently there is no support for the DialogCoordinator yet..
    ///     For more information see <a href="https://gist.github.com/ButchersBoy/4a7272f3ac104c5b1a54">here</a> and
    ///     <a href="https://dragablz.net/2015/05/29/using-mahapps-dialog-boxes-in-a-mvvm-setup/">here</a>
    /// </summary>
    public sealed class MetroWindowManager : DapploWindowManager
    {
        /// <summary>
        /// Default constructor taking care of initialization
        /// </summary>
        public MetroWindowManager(
            IEnumerable<IConfigureWindowViews> configureWindows,
            IEnumerable<IConfigureDialogViews> configureDialogs,
            IUiConfiguration uiConfiguration = null
        ) : base(configureWindows, configureDialogs, uiConfiguration)
        {
        }

        /// <summary>
        ///     Create a MetroWindow
        /// </summary>
        /// <param name="model">the model as object</param>
        /// <param name="view">the view as object</param>
        /// <param name="isDialog">Is this for a dialog?</param>
        /// <returns></returns>
        protected override Window CreateCustomWindow(object model, object view, bool isDialog)
        {
            var result = view as Window ?? new MetroWindow
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            bool isMetroWindow = result is MetroWindow;
            bool isWindow = view is Window;
            if (isMetroWindow || !isWindow)
            {
                result.SetResourceReference(Control.BorderBrushProperty, "AccentColorBrush");
                result.SetValue(Control.BorderThicknessProperty, new Thickness(1));
            }
            // Allow dialogs
            if (isDialog)
            {
                result.SetValue(DialogParticipation.RegisterProperty, model);
            }
            result.SetValue(View.IsGeneratedProperty, true);

            return result;
        }
    }
}