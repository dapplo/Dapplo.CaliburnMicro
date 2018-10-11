//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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

using System.ComponentModel;
using System.Runtime.Serialization;
using Dapplo.Config.Interfaces;

namespace Dapplo.CaliburnMicro.ClickOnce.Configuration
{
    /// <summary>
    /// Configuration for Click-Once
    /// </summary>
    public interface IClickOnceConfiguration : IDefaultValue
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
