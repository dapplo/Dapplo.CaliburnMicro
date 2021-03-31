// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;

#if DEBUG
using Dapplo.Log.Loggers;
#endif

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
#if NET471
                    "net471",
#else
                    "netcoreapp3.1",
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
#if NET471
                .WithoutCopyOfAssembliesToProbingPath()
#endif
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

            var application = new Dapplication(applicationConfig)
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown
            };
            // Handle exceptions
            application.DisplayErrorView();

            // Run!!!
            application.Run();
        }
    }
}