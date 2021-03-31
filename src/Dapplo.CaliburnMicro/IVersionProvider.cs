// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    /// This interface is used to display the current version, and check the latest version
    /// </summary>
    public interface IVersionProvider
    {
        /// <summary>
        /// Returns the current version
        /// </summary>
        Version CurrentVersion { get; }

        /// <summary>
        /// Return the latest version
        /// </summary>
        Version LatestVersion { get; }

        /// <summary>
        /// Returns a boolean to specify if there is an update available
        /// </summary>
        bool IsUpdateAvailable { get; }
    }
}
