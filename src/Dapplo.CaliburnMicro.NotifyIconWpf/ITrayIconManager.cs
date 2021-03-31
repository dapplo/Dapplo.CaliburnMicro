// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <summary>
    ///     This can be used to locate the ITrayIcon for a ViewModel
    /// </summary>
    public interface ITrayIconManager
    {
        /// <summary>
        /// Provide all available tray icons
        /// </summary>
        IEnumerable<ITrayIcon> TrayIcons { get; }

        /// <summary>
        ///     Get the ITrayIcon belonging to the specified ITrayIconViewModel instance
        /// </summary>
        /// <param name="trayIconViewModel">ViewModel instance to get the ITrayIcon for</param>
        /// <returns>ITrayIcon</returns>
        ITrayIcon GetTrayIconFor(ITrayIconViewModel trayIconViewModel);
    }
}