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

using System;
using System.Diagnostics;
using System.Windows;
using Dapplo.Log.Facade;
using Dapplo.Log.Loggers;

#endregion

namespace Dapplo.CaliburnMicro.Demo
{
	/// <summary>
	///     This takes care or starting the Application
	/// </summary>
	public class Startup
	{
		/// <summary>
		///     Start the application
		/// </summary>
		[STAThread, DebuggerNonUserCode]
		public static void Main()
		{
#if DEBUG
			// Initialize a debug logger for Dapplo packages
			LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);
#endif
			var application = new Dapplication("Dapplo.CaliburnMicro.Demo", "f32dbad8-9904-473e-86e2-19275c2d06a5")
			{
				ShutdownMode = ShutdownMode.OnExplicitShutdown,
			};
			application.Add(@".", "Dapplo.CaliburnMicro.dll");
			// Comment this if no TrayIcons should be used
			application.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			// Comment this to use the default window manager
			application.Add(@".", "Dapplo.CaliburnMicro.Metro.dll");
#if DEBUG
			application.Add(@"..\..\..\Dapplo.CaliburnMicro.Demo.Addon\bin\Debug", "Dapplo.CaliburnMicro.Demo.Addon.dll");
#else
			application.Add(@"..\..\..\Dapplo.CaliburnMicro.Demo.Addon\bin\Release", "Dapplo.CaliburnMicro.Demo.Addon.dll");
#endif
			application.Add(typeof(Startup).Assembly);

			application.Run();
		}
	}
}