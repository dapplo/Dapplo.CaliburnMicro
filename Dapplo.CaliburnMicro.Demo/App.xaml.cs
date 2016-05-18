//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro.Demo
// 
//  Dapplo.CaliburnMicro.Demo is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro.Demo is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro.Demo. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.LogFacade;
using Dapplo.LogFacade.Loggers;

#endregion

namespace Dapplo.CaliburnMicro.Demo
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private readonly ApplicationBootstrapper _bootstrapper = new ApplicationBootstrapper("Demo", "1234456789");

		public App()
		{
			InitializeComponent();
		}

		private async void App_OnStartup(object sender, StartupEventArgs e)
		{
			LogSettings.Logger = new DebugLogger { Level = LogLevel.Verbose };
#if DEBUG
			_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro\bin\Debug", "Dapplo.CaliburnMicro.dll");
			_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.MahApps\bin\Debug", "Dapplo.CaliburnMicro.MahApps.dll");
			_bootstrapper.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			//_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.DemoAddon\bin\Debug", "Dapplo.CaliburnMicro.DemoAddon.dll");
#else
			_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro\bin\Release", "Dapplo.CaliburnMicro.dll");
			_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.MahApps\bin\Release", "Dapplo.CaliburnMicro.MahApps.dll");
			_bootstrapper.Add(@".", "Dapplo.CaliburnMicro.NotifyIconWpf.dll");
			//_bootstrapper.Add(@"..\..\..\Dapplo.CaliburnMicro.DemoAddon\bin\Release", "Dapplo.CaliburnMicro.DemoAddon.dll");
#endif
			await _bootstrapper.RunAsync();
		}
	}
}