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

using System;
using System.ComponentModel.Composition;
using System.Windows;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.Log;
using Dapplo.Windows.Common.Extensions;
using Dapplo.Windows.Extensions;
using Dapplo.Windows.User32.Enums;
using Dapplo.Windows.User32.Structs;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that windows can store their locations, the ViewModel needs to extend IMaintainPosition
    /// </summary>
    [Export(typeof(IConfigureWindowViews))]
    [Export(typeof(IConfigureDialogViews))]
    public class PlacementViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IUiConfiguration _uiConfiguration;

        /// <summary>
        /// The constructor for the PlacementViewConfigurer
        /// </summary>
        /// <param name="uiConfiguration">IUiConfiguration</param>
        [ImportingConstructor]
        public PlacementViewConfigurer(IUiConfiguration uiConfiguration)
        {
            _uiConfiguration = uiConfiguration;
        }

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

        private void Configure(Window view, object viewModel = null)
        {
            // without VieWModel we cannot uniquely identify the window
            if (!(viewModel is IMaintainPosition) || _uiConfiguration == null || !_uiConfiguration.AreWindowLocationsStored)
            {
                return;
            }

            var windowName = viewModel.GetType().FullName;

            var screenBounds = DisplayInfo.GetAllScreenBounds();
            var hasPlacement = _uiConfiguration.WindowLocations.TryGetValue(windowName, out var placement);
            if (!hasPlacement || placement.ShowCmd == ShowWindowCommands.Normal && !screenBounds.Contains(placement.NormalPosition))
            {
                view.WindowStartupLocation = view.Owner != null ? WindowStartupLocation.CenterOwner : _uiConfiguration.DefaultWindowStartupLocation;
                hasPlacement = false;
            }
            if (hasPlacement)
            {
                view.WindowStartupLocation = WindowStartupLocation.Manual;
            }

            // Storage for the event handlers, so we can remove them again
            EventHandler[] eventHandlers = new EventHandler[3];

            // Store Placement
            eventHandlers[1] = (sender, args) =>
            {
                // ReSharper disable once InvokeAsExtensionMethod
                var newPlacement = WindowsExtensions.RetrievePlacement(view);
                if (newPlacement.ShowCmd == ShowWindowCommands.Hide)
                {
                    // Ignore
                    return;
                }
                Log.Debug().WriteLine("Stored placement {0} for Window {1}", newPlacement.NormalPosition, windowName);
                _uiConfiguration.WindowLocations[windowName] = newPlacement;
            };
            // Cleanup handlers
            eventHandlers[2] = (s, e) =>
            {
                view.LocationChanged -= eventHandlers[1];
                view.Closed -= eventHandlers[2];
            };

            //Initialize handlers
            eventHandlers[0] = (sender, args) =>
            {
                if (hasPlacement)
                {
                    // Make sure the placement is set
                    WindowsExtensions.ApplyPlacement(view, placement);
                }
                view.LocationChanged -= eventHandlers[0];
                view.LocationChanged += eventHandlers[1];
                view.Closed += eventHandlers[2];
            };
            view.LocationChanged += eventHandlers[0];
        }
    }
}
