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
using System.Globalization;
using System.Threading;
using System.Windows;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;
using Dapplo.Log.Loggers;

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

            // Add the directory where scanning takes place
#if DEBUG
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.Addon\bin\Debug");
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.MetroAddon\bin\Debug");
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.OverlayAddon\bin\Debug");
#else
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.Addon\bin\Release");
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.MetroAddon\bin\Release");
            application.Bootstrapper.AddScanDirectory(@"..\..\..\Application.Demo.OverlayAddon\bin\Release");
#endif

            // Load the Dapplo.CaliburnMicro.* assemblies
            application.Bootstrapper.FindAndLoadAssemblies("Dapplo.CaliburnMicro*");
            // Load the Application.Demo.* assemblies
            application.Bootstrapper.FindAndLoadAssemblies("Application.Demo.*");

            // Handle exceptions
            application.DisplayErrorView();
            application.Run();
        }
    }
}