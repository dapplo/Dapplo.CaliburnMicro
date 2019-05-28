//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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
using System.Reactive.Linq;
using System.Windows;
using Dapplo.Log;
using Dapplo.Windows.Common.Structs;
using Dapplo.Windows.Desktop;
using Dapplo.Windows.Dpi;
using Dapplo.Windows.Shell32;
using ToastNotifications.Core;
using Dapplo.Windows.Shell32.Enums;
using Dapplo.Windows.User32;
using Dapplo.Windows.User32.Enums;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// A special IPositionProvider which considers the system tray location
    /// </summary>
    public sealed class SystemTrayPositionProvider : IPositionProvider
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IDisposable _subscription;
        private readonly int _xOffset;
        private readonly int _yOffset;

        /// <summary>
        /// Initialize the SystemTrayPositionProvider, this hooks all the event handlers
        /// </summary>
        /// <param name="xOffset">X-offset for the toasts relative to the location of the taskbar</param>
        /// <param name="yOffset">Y-offset for the toasts relative to the location of the taskbar</param>
        public SystemTrayPositionProvider(int xOffset = 0, int yOffset = 0)
        {
            _xOffset = xOffset;
            _yOffset = yOffset;

            // Make sure we generate an event to recalculate the location on "workarea" changes, use Dapplo.Windows.EnvironmentMonitor
            _subscription = EnvironmentMonitor.EnvironmentUpdateEvents
                // Only on SPI_SETWORKAREA changes
                .Where(args => args.SystemParametersInfoAction == SystemParametersInfoActions.SPI_SETWORKAREA)
                // Create UpdatePositionRequested event 
                .Subscribe(args =>
                {
                    // TODO: Make it also work for when the taskbar moves screens or a resolution change is made.
                    UpdateHeightRequested?.Invoke(this, EventArgs.Empty);
                    UpdateEjectDirectionRequested?.Invoke(this, EventArgs.Empty);
                    UpdatePositionRequested?.Invoke(this, EventArgs.Empty);
                });
        }

        /// <summary>
        /// Remove the EnvironmentMonitor.EnvironmentUpdateEvents subscription
        /// </summary>
        public void Dispose()
        {
            _subscription?.Dispose();
        }

        /// <summary>
        /// This takes the height and width of the Popup and will return the location where it should be displayed
        /// </summary>
        /// <param name="actualPopupWidth">double</param>
        /// <param name="actualPopupHeight">double</param>
        /// <returns>Point</returns>
        public Point GetPosition(double actualPopupWidth, double actualPopupHeight)
        {
            var taskbar = Shell32Api.TaskbarPosition;
            var taskbarBounds = taskbar.Bounds;

            var actualSize = new NativeSize((int)actualPopupWidth, (int)actualPopupHeight);

            // Use the DPI of the desktop
            var dpi = NativeDpiMethods.GetDpi(InteropWindowQuery.GetDesktopWindow().Handle);
            actualSize = DpiHandler.ScaleWithDpi(actualSize, dpi);
            int x, y;

            // Define the new position
            switch (taskbar.AppBarEdge)
            {
                case AppBarEdges.Left:
                    x = taskbarBounds.Right + _xOffset;
                    y = taskbarBounds.Bottom - _yOffset - actualSize.Height;
                    break;
                case AppBarEdges.Top:
                    x = taskbarBounds.Right - _xOffset - actualSize.Width;
                    y = taskbarBounds.Bottom + _yOffset;
                    break;
                case AppBarEdges.Right:
                    x = taskbarBounds.Left - _xOffset - actualSize.Width;
                    y = taskbarBounds.Bottom - _yOffset - actualSize.Height;
                    break;
                case AppBarEdges.Bottom:
                default:
                    x = taskbarBounds.Right - _xOffset - actualSize.Width;
                    y = taskbarBounds.Top - _yOffset - actualSize.Height;
                    break;
            }

            var position = DpiHandler.UnscaleWithDpi(new NativePoint(x, y), dpi);
            Log.Debug().WriteLine("Taskbar location {0} at {1}, calculate popup position: {2}", taskbar.AppBarEdge, taskbarBounds, position);
            return position;
        }

        /// <summary>
        /// Returns the height, this is needed to know how many toasts are visible
        /// </summary>
        /// <returns></returns>
        public double GetHeight()
        {
            var taskbarBounds = Shell32Api.TaskbarPosition.Bounds;

            var displayBounds = DisplayInfo.GetBounds(taskbarBounds.Location);
            return displayBounds.Height;
        }

        /// <summary>
        /// Owner window of the NotificationsWindow
        /// </summary>
        public Window ParentWindow { get; set; }

        /// <summary>
        /// Specifies in what direction the toasts are ejected
        /// </summary>
        public EjectDirection EjectDirection => Shell32Api.TaskbarPosition.AppBarEdge == AppBarEdges.Top ? EjectDirection.ToBottom: EjectDirection.ToTop;

        /// <summary>
        /// ToastNotifications framework registers this event, to get notified of changes which influence the position
        /// </summary>
        public event EventHandler UpdatePositionRequested;

        /// <summary>
        /// ToastNotifications framework registers this event, to get notified of changes which influence the direction
        /// </summary>
        public event EventHandler UpdateEjectDirectionRequested;

        /// <summary>
        /// ToastNotifications framework registers this event, to get notified of changes to the screen height
        /// </summary>
        public event EventHandler UpdateHeightRequested;
    }
}
