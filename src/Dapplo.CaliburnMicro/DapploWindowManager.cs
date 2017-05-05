//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors;
using Dapplo.Windows.Dpi.Wpf;

#endregion

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     This extends the WindowManager to add some additional settings
    /// </summary>
    public class DapploWindowManager : WindowManager
    {
        /// <summary>
        ///     Implement this to make specific configuration changes to your owned (dialog) window.
        /// </summary>
        public Action<Window> OnConfigureDialog { get; set; }

        /// <summary>
        ///     Implement this to make specific configuration changes to your window.
        /// </summary>
        public Action<Window> OnConfigureWindow { get; set; }


        /// <inheritdoc />
        public override void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            base.ShowPopup(rootModel, context, settings);
        }

        /// <inheritdoc />
        public override bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.ShowDialog(rootModel, context, settings);
        }

        /// <inheritdoc />
        public override Page CreatePage(object rootModel, object context, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreatePage(rootModel, context, settings);
        }

        /// <inheritdoc />
        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreateWindow(rootModel, isDialog, context, settings);
        }

        /// <inheritdoc />
        protected override Popup CreatePopup(object rootModel, IDictionary<string, object> settings)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }
            return base.CreatePopup(rootModel, settings);
        }

        /// <inheritdoc />
        public override void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            // Maybe the ViewModel supplies settings
            if (settings == null)
            {
                settings = (rootModel as IHaveSettings)?.Settings;
            }

            base.ShowWindow(rootModel, context, settings);
        }

        /// <summary>
        ///     Create the window type for this window manager
        /// </summary>
        /// <param name="model">object with the model</param>
        /// <param name="view">object with the view</param>
        /// <param name="isDialog">specifies if this is a dialog</param>
        /// <returns>Window</returns>
        protected virtual Window CreateCustomWindow(object model, object view, bool isDialog)
        {
            var result = view as Window ?? new Window
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            return result;
        }

        /// <inheritdoc />
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = CreateCustomWindow(model, view, isDialog);

            // Allow dialogs
            window.SetValue(View.IsGeneratedProperty, true);
            var inferOwnerOf = InferOwnerOf(window);
            if (inferOwnerOf != null && isDialog)
            {
                // "Dialog", center it on top of the owner
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Owner = inferOwnerOf;
                OnConfigureDialog?.Invoke(window);
            }
            else
            {
                // Free window, without owner
                OnConfigureWindow?.Invoke(window);
            }
            var haveIcon = model as IHaveIcon;
            if (haveIcon != null && window.Icon == null)
            {
                // Now use the attached behavior to set the icon
                window.SetCurrentValue(FrameworkElementIcon.ValueProperty, haveIcon.Icon);
            }
            // Make sure DPI handling is done correctly
            window.AttachWindowDpiHandler();
            // Just in case, make sure it's activated
            window.Activate();
            return window;

        }
    }
}