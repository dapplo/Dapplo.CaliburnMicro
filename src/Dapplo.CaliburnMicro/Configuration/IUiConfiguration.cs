// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using Dapplo.Config.Ini;
using Dapplo.Windows.User32.Structs;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    /// This is a configuration for some of the UI behavior
    /// </summary>
    [IniSection("Dapplo.Caliburn")]
    public interface IUiConfiguration : IIniSection
    {
        /// <summary>
        /// Defines the default value of the startup location for windows
        /// </summary>
        [Description("This defines the default startup location of newly created windows.")]
        [DefaultValue(WindowStartupLocation.CenterScreen)]
        [DataMember(EmitDefaultValue = false)]
        WindowStartupLocation DefaultWindowStartupLocation { get; set; }

        /// <summary>
        /// Defines if the locations of the windows are stored, so if they open again it's at the same location
        /// </summary>
        [Description("When set to true, the application will try to maintain the windows position of some windows.")]
        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        bool AreWindowLocationsStored { get; set; }

        /// <summary>
        /// A store for the window locations
        /// </summary>
        [Description("This stores the locations of all windows which maintain a position")]
        [DataMember(EmitDefaultValue = false)]
        IDictionary<string, WindowPlacement> WindowLocations { get; set; }
    }
}
