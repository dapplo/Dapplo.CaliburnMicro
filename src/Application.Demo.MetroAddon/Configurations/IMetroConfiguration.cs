// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Metro.Configuration;
using Dapplo.Config.Ini;
using MahApps.Metro.Controls;

namespace Application.Demo.MetroAddon.Configurations
{
    [IniSection("Metro")]
    public interface IMetroConfiguration : IIniSection, IMetroUiConfiguration
    {
        HotKey HotKey { get; set; }
    }
}