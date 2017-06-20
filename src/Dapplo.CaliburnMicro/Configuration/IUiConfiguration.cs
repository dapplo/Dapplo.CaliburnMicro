﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using Dapplo.Ini;
using Dapplo.InterfaceImpl.Extensions;
using Dapplo.Windows.User32.Structs;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    /// This is a configuration for some of the UI behavior
    /// </summary>
    [IniSection("Dapplo.Caliburn")]
    public interface IUiConfiguration : IIniSection, INotifyPropertyChanged, IDefaultValue
    {
        /// <summary>
        /// Defines the default value of the startup location for windows
        /// </summary>
        [DefaultValue(WindowStartupLocation.CenterScreen)]
        [DataMember(EmitDefaultValue = false)]
        WindowStartupLocation StartupLocation { get; set; }

        /// <summary>
        /// Defines if the locations of the windows are stored, so if they open again it's at the same location
        /// </summary>
        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        bool AreWindowLocationsStored { get; set; }

        /// <summary>
        /// A store for the window locations
        /// </summary>
        [Description("Store for the window placements")]
        [DataMember(EmitDefaultValue = false)]
        IDictionary<string, WindowPlacement> WindowLocations { get; set; }
    }
}
