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
using System.Reflection;
using System.Windows;
using Dapplo.Addons.Bootstrapper;
using Dapplo.LogFacade;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro
{
	/// <summary>
	///     This extends the System.Windows.Application to make it easier to startup you application.
	///     It will initialize MEF, Caliburn.Micro, handle exceptions and more.
	/// </summary>
	public class Dapplication : Application
	{
		private static readonly LogSource Log = new LogSource();

		private readonly ApplicationBootstrapper _bootstrapper;

		/// <summary>
		///     Create the Dapplication for the specified application name
		///     The mutex is created and locked in the contructor, and some of your application logic might depend on this.
		/// </summary>
		/// <param name="applicationName">Name of your application</param>
		/// <param name="mutexId">
		///     string with an ID for your mutex, preferably a Guid. If the mutex can't be locked, the
		///     bootstapper will not  be able to "bootstrap".
		/// </param>
		/// <param name="global">Is the mutex a global or local block (false means only in this Windows session)</param>
		public Dapplication(string applicationName, string mutexId = null, bool global = false)
		{
			_bootstrapper = new ApplicationBootstrapper(applicationName, mutexId, global);
		}

		/// <summary>
		///     Allows access to the Dapplo.Addons.ApplicationBootstrapper
		/// </summary>
		public ApplicationBootstrapper Bootstrapper { get; set; }

		/// <summary>
		///     This is called when the application is alreay running
		/// </summary>
		public Action OnAlreadyRunning { get; set; }

		/// <summary>
		///     Add the assemblies (with parts) found in the specified directory
		/// </summary>
		/// <param name="directory">Directory to scan</param>
		/// <param name="pattern">Pattern to use for the scan, default is "*.dll"</param>
		public void Add(string directory, string pattern = "*.dll")
		{
			_bootstrapper.Add(directory, pattern);
		}

		/// <summary>
		///     Add an assembly to the bootstrapper
		/// </summary>
		/// <param name="assembly">Assembly to add</param>
		public void Add(Assembly assembly)
		{
			_bootstrapper.Add(assembly);
		}

		/// <summary>
		///     Make sure the bootstrapper is stopped,
		/// </summary>
		/// <param name="e"></param>
		protected override void OnExit(ExitEventArgs e)
		{
			Log.Info().WriteLine("Stopping the Bootstrapper, if the application hangs here is a problem with a ShutdownAsync!!");
			_bootstrapper.StopAsync().Wait();
			base.OnExit(e);
		}

		/// <summary>
		///     Make sure we startup everything after WPF instanciated
		/// </summary>
		/// <param name="startupEventArgs">StartupEventArgs</param>
		protected override async void OnStartup(StartupEventArgs startupEventArgs)
		{
			// Hook unhandled exceptions
			DispatcherUnhandledException += (sender, eventArgs) =>
			{
				eventArgs.Handled = true;
				OnUnhandledException(eventArgs.Exception);
			};

			// Enable UI access for different Dapplo packages, especially the UiContext.RunOn
			// This only works here, not before the Application is started
			UiContext.Initialize();

			if (!_bootstrapper.IsMutexLocked)
			{
				OnAlreadyRunning?.Invoke();
				return;
			}
			base.OnStartup(startupEventArgs);
			await _bootstrapper.RunAsync().ConfigureAwait(false);
		}

		/// <summary>
		///     Override to handle exceptions.
		/// </summary>
		/// <param name="exception">Exception</param>
		protected virtual void OnUnhandledException(Exception exception)
		{
			Log.Error().WriteLine(exception);
		}
	}
}