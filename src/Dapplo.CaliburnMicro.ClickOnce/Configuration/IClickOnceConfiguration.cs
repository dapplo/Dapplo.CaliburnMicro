// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.Serialization;

namespace Dapplo.CaliburnMicro.ClickOnce.Configuration
{
    /// <summary>
    /// Configuration for Click-Once
    /// </summary>
    public interface IClickOnceConfiguration
    {
        /// <summary>
        /// When set to true, the update check is done on startup, this does delay the starting of the application.
        /// </summary>
        [Description("When set to true, the update check is done on startup, this does delay the starting of the application. (default = false)")]
        [DefaultValue(false)]
        [DataMember(EmitDefaultValue = false)]
        bool CheckOnStart { get; set; }

        /// <summary>
        /// Do we need to check for updates in the background.
        /// </summary>
        [Description("Do we need to check for updates in the background. (default = true)")]
        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        bool EnableBackgroundUpdateCheck { get; set; }

        /// <summary>
        /// The interfal between checks in minutes
        /// </summary>
        [Description("The interfal between checks in minutes. (default = 60)")]
        [DefaultValue(60)]
        [DataMember(EmitDefaultValue = false)]
        int CheckInterval { get; set; }

        /// <summary>
        /// Is a found update automatically applied?
        /// </summary>
        [Description("Is a found update automatically applied? (default = true)")]
        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        bool AutoUpdate { get; set; }

        /// <summary>
        /// Does the application restart automatically after updating?
        /// </summary>
        [Description("Does the application restart automatically after updating? (default = false)")]
        [DefaultValue(false)]
        [DataMember(EmitDefaultValue = false)]
        bool AutoRestart { get; set; }
    }
}
