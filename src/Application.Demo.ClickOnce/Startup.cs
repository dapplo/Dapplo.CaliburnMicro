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
using System.Threading;
using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;
using Dapplo.Log.LogFile;

#endregion

namespace Application.Demo.ClickOnce
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
            LogSettings.RegisterDefaultLogger<FileLogger>(LogLevels.Verbose);
            //LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);

            // Use this to setup the culture of your UI
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var applicationConfig = ApplicationConfigBuilder
                .Create()
                .WithApplicationName("ClickOnceDemo")
                .WithMutex("2141D0DC-2B87-4B70-A8A7-A1EFDB588656")
                .WithCaliburnMicro()
                .WithAssemblyPatterns("Application.Demo*")
                .BuildApplicationConfig();

            var application = new Dapplication(applicationConfig)
            {
                ShutdownMode = ShutdownMode.OnLastWindowClose,
                OnAlreadyRunning = () =>
                {
                    MessageBox.Show("Already started, exiting");
                    return -1;
                }
            };
            // Handle exceptions
            application.DisplayErrorView();
            application.Run();
        }
    }
}