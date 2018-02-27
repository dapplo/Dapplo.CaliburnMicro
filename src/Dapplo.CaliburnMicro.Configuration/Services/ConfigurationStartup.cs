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

#region using

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Ini;

#endregion

namespace Dapplo.CaliburnMicro.Configuration.Services
{
    /// <summary>
    ///     This registers a ServiceProviderExportProvider for providing IIniSection
    /// </summary>
    [StartupAction(StartupOrder = (int) CaliburnStartOrder.Bootstrapper + 1)]
    public class ConfigurationStartup : IAsyncStartupAction
    {
        private static readonly Type IniSectionType = typeof(IIniSection);
        private readonly IApplicationBootstrapper _applicationBootstrapper;
        /// <summary>
        /// Create the ConfigurationStartup
        /// </summary>
        /// <param name="applicationBootstrapper">IApplicationBootstrapper</param>
        [ImportingConstructor]
        public ConfigurationStartup(IApplicationBootstrapper applicationBootstrapper)
        {
            _applicationBootstrapper = applicationBootstrapper;
        }
        /// <summary>
        /// async Start of the IniConfig
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var iniConfig = IniConfig.Current;
            if (iniConfig == null)
            {
                iniConfig = new IniConfig(_applicationBootstrapper.ApplicationName, _applicationBootstrapper.ApplicationName);
                await iniConfig.LoadIfNeededAsync(cancellationToken);
            }
            _applicationBootstrapper.Export<IServiceProvider>(iniConfig);
            var iniSectionTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsInterface && type != IniSectionType && type.Namespace?.StartsWith("Dapplo.Ini") != true && IniSectionType.IsAssignableFrom(type)
                select type;
            foreach (var iniSectionType in iniSectionTypes)
            {
                iniConfig.Get(iniSectionType);
            }

        }
    }
}