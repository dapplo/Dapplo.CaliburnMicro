//  Dapplo - building blocks for desktop applications
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

using System.ComponentModel;
using System.Runtime.Serialization;
using Dapplo.CaliburnMicro.ClickOnce.Configuration;
using Dapplo.Ini;

namespace Application.Demo.ClickOnce.Config
{
    [IniSection("ClickOnce")]
    public interface IClickOnceDemoConfiguration : IIniSection, IClickOnceConfiguration
    {
        /// <summary>
        /// The interfal between checks in minutes
        /// </summary>
        [Description("When set to true, the update check is done on startup, this does delay the starting of the application. (default = false)")]
        [DefaultValue(true)]
        [DataMember(EmitDefaultValue = false)]
        new bool CheckOnStart { get; set; }

        /// <summary>
        /// The interfal between checks in minutes
        /// </summary>
        [Description("The interfal between checks in minutes. (default = 1)")]
        [DefaultValue(1)]
        [DataMember(EmitDefaultValue = false)]
        new int CheckInterval { get; set; }
    }
}
