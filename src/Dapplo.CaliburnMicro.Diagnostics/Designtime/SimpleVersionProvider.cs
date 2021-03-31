// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if DEBUG
using System;
using System.Reflection;

namespace Dapplo.CaliburnMicro.Diagnostics.Designtime
{
    /// <summary>
    /// A very simple version provider
    /// </summary>
    public class SimpleVersionProvider : IVersionProvider
    {
        /// <inheritdoc />
        public Version CurrentVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        /// <inheritdoc />
        public Version LatestVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        /// <inheritdoc />
        public bool IsUpdateAvailable => LatestVersion > CurrentVersion;
    }
}
#endif
