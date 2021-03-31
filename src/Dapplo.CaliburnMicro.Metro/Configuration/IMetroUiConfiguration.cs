// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Dapplo.CaliburnMicro.Metro.Configuration
{
    /// <summary>
    /// This is the configuration for the Metro UI settings
    /// </summary>
    public interface IMetroUiConfiguration
    {
        /// <summary>
        ///     The theme
        /// </summary>
        [DefaultValue("Light")]
        string Theme { get; set; }

        /// <summary>
        ///     The color for the theme
        /// </summary>
        [DefaultValue("Orange")]
        string ThemeColor { get; set; }

    }
}
