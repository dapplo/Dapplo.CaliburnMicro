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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;
using Dapplo.Log.Loggers;
using Dapplo.Utils.Resolving;

#endregion

namespace Application.Demo
{
    /// <summary>
    ///     This takes care or starting the Application
    /// </summary>
    public static class Startup
    {
        private static readonly LogSource Log = new LogSource();
        /// <summary>
        ///     Start the application
        /// </summary>
        [STAThread]
        public static void Main()
        {
#if DEBUG
            // Initialize a debug logger for Dapplo packages
            LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);
#endif

            var cultureInfo = CultureInfo.GetCultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var application = new Dapplication("Application.Demo", "f32dbad8-9904-473e-86e2-19275c2d06a5")
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };

            bool didWeLoad = false;
            foreach (var assembly in LoadEmbedded())
            {
                assembly.Register();
                application.Bootstrapper.Add(assembly);
                didWeLoad = true;
            }
            if (!didWeLoad)
            {
                // Load the Dapplo.CaliburnMicro.* assemblies
                application.Bootstrapper.FindAndLoadAssemblies("Dapplo.*");
                // Load the Application.Demo.* assemblies
                application.Bootstrapper.FindAndLoadAssemblies("Application.Demo.*");
            }

            // Handle exceptions
            application.DisplayErrorView();
            application.Run();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> LoadEmbedded()
        {
            var assemblyLoader = Type.GetType("Costura.AssemblyLoader");

            var assembliesAsResourcesFieldInfo = assemblyLoader?.GetField("assemblyNames", BindingFlags.Static | BindingFlags.NonPublic);
            if (assembliesAsResourcesFieldInfo == null)
            {
                return Enumerable.Empty<Assembly>();
            }

            var assembliesAsResources = (IDictionary<string, string>)assembliesAsResourcesFieldInfo.GetValue(null);

            var symbolsAsResourcesFieldInfo = assemblyLoader.GetField("symbolNames", BindingFlags.Static | BindingFlags.NonPublic);
            var symbolsAsResources = (IDictionary<string, string>)symbolsAsResourcesFieldInfo?.GetValue(null);

            var readFromEmbeddedResourcesMethodInfo = assemblyLoader.GetMethod("ReadFromEmbeddedResources", BindingFlags.Static | BindingFlags.NonPublic);
            if (readFromEmbeddedResourcesMethodInfo == null || symbolsAsResources == null)
            {
                return Enumerable.Empty<Assembly>();
            }
            return LoadAssemblies(assembliesAsResources, readFromEmbeddedResourcesMethodInfo, symbolsAsResources);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembliesAsResources"></param>
        /// <param name="readFromEmbeddedResourcesMethodInfo"></param>
        /// <param name="symbolsAsResources"></param>
        /// <returns></returns>
        private static IEnumerable<Assembly> LoadAssemblies(IDictionary<string, string> assembliesAsResources, MethodInfo readFromEmbeddedResourcesMethodInfo, IDictionary<string, string> symbolsAsResources)
        {
            return assembliesAsResources.Select(assemblyKeyValuePair =>
            {
                var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLowerInvariant().Contains($"{assemblyKeyValuePair.Key},"));
                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }
                Log.Debug().WriteLine("Forcing load from Costura packed assembly '{0}'", assemblyKeyValuePair.Key);

                return readFromEmbeddedResourcesMethodInfo.Invoke(null, new object[] { assembliesAsResources, symbolsAsResources, new AssemblyName(assemblyKeyValuePair.Key) }) as Assembly;
            }).Where(assembly => assembly != null);
        }
    }
}