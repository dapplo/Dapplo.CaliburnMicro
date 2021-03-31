// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.Log;
using Dapplo.Windows.Common.Extensions;
using Dapplo.Windows.Extensions;
using Dapplo.Windows.User32;
using Dapplo.Windows.User32.Enums;

namespace Dapplo.CaliburnMicro.Configurers
{
    /// <summary>
    /// This takes care that windows can store their locations, the ViewModel needs to extend IMaintainPosition
    /// </summary>
    public class PlacementViewConfigurer : IConfigureWindowViews, IConfigureDialogViews
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IUiConfiguration _uiConfiguration;

        /// <summary>
        /// The constructor for the PlacementViewConfigurer
        /// </summary>
        /// <param name="uiConfiguration">IUiConfiguration</param>
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

            var screenBounds = DisplayInfo.ScreenBounds;
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
                    view.ApplyPlacement(placement);
                }
                view.LocationChanged -= eventHandlers[0];
                view.LocationChanged += eventHandlers[1];
                view.Closed += eventHandlers[2];
            };
            view.LocationChanged += eventHandlers[0];
        }
    }
}
