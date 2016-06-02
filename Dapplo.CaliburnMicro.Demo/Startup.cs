//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
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

using Dapplo.Addons.Bootstrapper;
using Dapplo.LogFacade;
using Dapplo.Utils;
using System;
using System.Diagnostics;
using System.Windows;
using Dapplo.LogFacade.Loggers;

#endregion

namespace Dapplo.CaliburnMicro.Demo
{
	/// <summary>
	/// This takes care or starting the Application
	/// </summary>
	public class Startup : Application
	{
		private static readonly LogSource Log = new LogSource();

		private readonly ApplicationBootstrapper _bootstrapper = new ApplicationBootstrapper("Dapplo.CaliburnMicro.Demo", "f32dbad8-9904-473e-86e2-19275c2d06a5");

		/// <summary>
		/// Start the application
		/// </summary>
		[STAThread, DebuggerNonUserCode]
		public static void Main()
		{
#if DEBUG
			// Initialize a debug logger for Dapplo packages
			LogSettings.Logger = new DebugLogger { Level = LogLevel.Verbose };
#endif
			var application = new Startup
			{
				ShutdownMode = ShutdownMode.OnLastWindowClose
			};
			application.Run();
		}

		/// <summary>
		/// Make sure we startup everything after WPF instanciated
		/// </summary>
		/// <param name="startupEventArgs">StartupEventArgs</param>
		protected override async void OnStartup(StartupEventArgs startupEventArgs)
		{
			base.OnStartup(startupEventArgs);

			UiContext.Initialize();

			_bootstrapper.Add(@".", "Dapplo.CaliburnMicro.dll");
			// Comment this if no TrayIcons should be used
			_bootstrapper.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			// Comment this to use the default window manager
			_bootstrapper.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");
#if DEBUG
			//_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.DemoAddon\bin\Debug", "Dapplo.CaliburnMicro.DemoAddon.dll");
#else
	//_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.DemoAddon\bin\Release", "Dapplo.CaliburnMicro.DemoAddon.dll");
#endif
			_bootstrapper.Add(GetType().Assembly);

			await _bootstrapper.RunAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Make sure the bootstrapper is stopped,
		/// </summary>
		/// <param name="e"></param>
		protected override void OnExit(ExitEventArgs e)
		{
			Log.Info().WriteLine("Stopping the Bootstrapper, if the application hangs here is a problem with a ShutdownAsync!!");
			_bootstrapper.StopAsync().Wait();
			base.OnExit(e);
		}
	}
}
