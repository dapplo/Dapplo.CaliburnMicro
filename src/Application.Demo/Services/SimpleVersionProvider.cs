// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Dapplo.CaliburnMicro;

namespace Application.Demo.Services
{
    /// <summary>
    /// A very simple version provider
    /// </summary>
    public class SimpleVersionProvider : IVersionProvider
    {
        public Version CurrentVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version;

        public Version LatestVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version;

        public bool IsUpdateAvailable => LatestVersion > CurrentVersion;
    }
}
