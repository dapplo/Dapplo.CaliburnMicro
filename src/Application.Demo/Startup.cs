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

#region using

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using Dapplo.Microsoft.Extensions.Hosting.Plugins;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
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
        private const string AppSettingsFilePrefix = "appsettings";
        private const string HostSettingsFile = "hostsettings.json";
        private const string Prefix = "PREFIX_";

        public static async Task Main(string[] args)
        {
            var executableLocation = Path.GetDirectoryName(typeof(Startup).Assembly.Location);

            var host = new HostBuilder()
                .ConfigureWpf()
                .ConfigureLogging()
                .ConfigureConfiguration(args)
                .ConfigureSingleInstance(builder =>
                {
                    builder.MutexId = "{f32dbad8-9904-473e-86e2-19275c2d06a5}";
                    builder.WhenNotFirstInstance = (hostingEnvironment, logger) =>
                    {
                        // This is called when an instance was already started, this is in the second instance
                        logger.LogWarning("Application {0} already running.", hostingEnvironment.ApplicationName);
                    };
                })
                .ConfigurePlugins(pluginBuilder =>
                {
                    // Specify the location from where the DLL's are "globbed"
                    pluginBuilder.AddScanDirectories(Path.Combine(executableLocation, @"..\..\..\..\"));
                    // Add the framework libraries which can be found with the specified globs
                    pluginBuilder.IncludeFrameworks(@"**\bin\**\Application.Demo.*.dll");
                    // Add the plugins which can be found with the specified globs
                    pluginBuilder.IncludePlugins(@"**\bin\**\Dapplo.CaliburnMicro.*.dll");
                })
                .ConfigureCaliburnMicro<MainViewModel>()
                .ConfigureServices(serviceCollection =>
                {
                    // Make OtherWindow available for DI to MainWindow
                    serviceCollection.AddTransient<OtherViewModel>();
                })
                .UseWpfLifetime()
                .Build();

            Console.WriteLine("Run!");
            await host.RunAsync();
        }

        /// <summary>
        /// Configure the loggers
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConsole();
                configLogging.AddDebug();
            });
        }

        /// <summary>
        /// Configure the configuration
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder ConfigureConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile(HostSettingsFile, optional: true);
                configHost.AddEnvironmentVariables(prefix: Prefix);
                configHost.AddCommandLine(args);
            })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile(AppSettingsFilePrefix + ".json", optional: true);
                    if (!string.IsNullOrEmpty(hostContext.HostingEnvironment.EnvironmentName))
                    {
                        configApp.AddJsonFile(AppSettingsFilePrefix + $".{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    }
                    configApp.AddEnvironmentVariables(prefix: Prefix);
                    configApp.AddCommandLine(args);
                });
        }
    }
}