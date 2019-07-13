//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System;
using Dapplo.Config.Ini;
using Dapplo.Config.Intercepting;

namespace Application.Demo.MetroAddon.Configurations
{
    internal class MetroConfigurationImpl : IniSection<IMetroConfiguration>
    {
        public Action<IIniSection> OnAfterLoad { get; set; }
        public Action<IIniSection> OnAfterSave { get; set; }
        public Action<IIniSection> OnBeforeSave { get; set; }

        /// <summary>
        /// Factory for IniSection implementations
        /// </summary>
        /// <returns>TInterface</returns>
        public static IMetroConfiguration Create(out MetroConfigurationImpl metroConfiguration)
        {
            metroConfiguration = new MetroConfigurationImpl();
            return ConfigProxy.Create<IMetroConfiguration>(metroConfiguration);
        }

        public override void AfterLoad(IIniSection iniSection)
        {
            OnAfterLoad?.Invoke(iniSection);
        }

        public override void AfterSave(IIniSection iniSection)
        {
            OnAfterSave?.Invoke(iniSection);
        }

        public override void BeforeSave(IIniSection iniSection)
        {
            OnBeforeSave?.Invoke(iniSection);
        }
    }
}
