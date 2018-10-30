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

#region using

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;

#if DEBUG
using Dapplo.Log.Loggers;
#endif

#endregion

namespace Application.Demo
{
    /// <summary>
    ///     This takes care or starting the Application
    /// </summary>
    public static class Startup
    {
        /// <summary>
        ///     Start the application
        /// </summary>
        [STAThread]
        public static void Main()
        {

            var applicationConfig = ApplicationConfigBuilder.
                Create()
                //.WithoutAsyncAssemblyLoading()
                // Make sure the bootstrapper knows where to find it's DLL files
                .WithScanDirectories(
                    ScanLocations.GenerateScanDirectories(
#if !NET461
                    "net461",
#else
                    "netcoreapp3.0",
#endif
                        "Application.Demo.Addon",
                        "Application.Demo.MetroAddon",
                        "Application.Demo.OverlayAddon").ToArray()
                )
                .WithApplicationName("Application.Demo")
                .WithMutex("f32dbad8-9904-473e-86e2-19275c2d06a5")
                // Enable CaliburnMicro
                .WithCaliburnMicro()
                .WithoutCopyOfEmbeddedAssemblies()
                //.WithoutStrictChecking()
                // Load the Application.Demo.* assemblies
                .WithAssemblyPatterns("Application.Demo.*").BuildApplicationConfig();
            Start(applicationConfig);
        }

        private static void Start(ApplicationConfig applicationConfig)
        {
            // Make sure the log entries are demystified
            //LogSettings.ExceptionToStacktrace = exception => exception.ToStringDemystified();
#if DEBUG
            // Initialize a debug logger for Dapplo packages
            LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);
#endif

            // Use this to setup the culture of your UI
            var cultureInfo = CultureInfo.GetCultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var application = new Dapplication(applicationConfig)
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };
            // Handle exceptions
            //application.DisplayErrorView();

            // Run!!!
            application.Run();
        }
    }
}