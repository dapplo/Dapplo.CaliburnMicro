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
using Dapplo.Log.Facade;
using Dapplo.Utils;
using System.Threading.Tasks;
using System.Windows.Threading;

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
			// Hook unhandled exceptions in the Dispatcher
			DispatcherUnhandledException += HandleDispatcherException;

			// Hook unhandled exceptions in the AppDomain
			AppDomain.CurrentDomain.UnhandledException += HandleAppDomainException;

			// Hook unhandled exceptions in tasks
			TaskScheduler.UnobservedTaskException += HandleTaskException;

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
		///     This is called when the application is alreay running
		/// </summary>
		public Action<Exception> OnUnhandledException { get; set; }

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
		///     Make sure the Dapplication is stopped, the bootstrapper is shutdown.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnExit(ExitEventArgs e)
		{
			Log.Info().WriteLine("Stopping the Dapplication.");
			Dispatcher.Invoke(async () => await _bootstrapper.StopAsync().ConfigureAwait(false));
			base.OnExit(e);
		}

		/// <summary>
		///     Make sure we startup everything after WPF instanciated
		/// </summary>
		/// <param name="startupEventArgs">StartupEventArgs</param>
		protected override async void OnStartup(StartupEventArgs startupEventArgs)
		{
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

		#region Error handling
		/// <summary>
		/// This is called when exceptions occure inside a dispatched call
		/// </summary>
		public Action<Exception> OnUnhandledDispatcherException { get; set; }

		/// <summary>
		/// This is called when Exceptions are not handled inside the applications dispatcher
		/// It will call the OnUnhandledException action, which can be used to display a message.
		/// Only when an action is assigned, and not throw an exception, the application will not be terminated.
		/// </summary>
		/// <param name="sender">Sender of this event</param>
		/// <param name="eventArgs">DispatcherUnhandledExceptionEventArgs</param>
		protected virtual void HandleDispatcherException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
		{
			Log.Error().WriteLine(eventArgs.Exception, "Exception in Dispatcher");
			if (OnUnhandledDispatcherException != null)
			{
				// Make sure this doesn't cause any additional exceptions
				try
				{
					OnUnhandledDispatcherException?.Invoke(eventArgs.Exception);
					// Signal that the exception was handled, to prevent the application from crashing.
					eventArgs.Handled = true;
				}
				catch (Exception callerException)
				{
					Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledDispatcherException invokation");
				}
			}
		}

		/// <summary>
		/// This is called when exceptions occure inside the AppDomain (everywhere)
		/// Exception is the reason, the boolean specifies if your application will be terminated.
		/// </summary>
		public Action<Exception, bool> OnUnhandledAppDomainException { get; set; }

		/// <summary>
		/// This is called when Exceptions are not handled inside the (current) AppDomain
		/// It will call the OnUnhandledAppDomainException action, which can be used to display a message.
		/// Implementing an action can NOT prevent termination of your application!
		/// It may, or may not, be terminated
		/// </summary>
		/// <param name="sender">Sender of this event</param>
		/// <param name="eventArgs">UnhandledExceptionEventArgs</param>
		protected virtual void HandleAppDomainException(object sender, UnhandledExceptionEventArgs eventArgs)
		{
			if (eventArgs.IsTerminating)
			{
				Log.Error().WriteLine("An exception the the current AppDomain will terminate your application.");
			}
			var exception = eventArgs.ExceptionObject as Exception;
			Log.Error().WriteLine(exception, "Exception in AppDomain");
			if (OnUnhandledAppDomainException != null)
			{
				// Make sure this doesn't cause any additional exceptions
				try
				{
					OnUnhandledAppDomainException?.Invoke(exception, eventArgs.IsTerminating);
				}
				catch (Exception callerException)
				{
					Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledDispatcherException invokation");
				}
			}
		}

		/// <summary>
		/// Specifies if an UnhandledTaskException is logged and 
		/// </summary>
		public bool ObserveUnhandledTaskException { get; set; } = true;

		/// <summary>
		/// This is called when exceptions occure inside Tasks (everywhere)
		/// </summary>
		public Action<Exception> OnUnhandledTaskException { get; set; }

		/// <summary>
		/// This is called when Exceptions are not handled inside Tasks
		/// It will call the OnUnhandledTaskException action, which can be used to display a message or do something else.
		/// In .NET before 4.5 this would terminate your application, since 4.5 it does not.
		/// Unless you change the configuration, see <a href="https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskscheduler.unobservedtaskexception.aspx">here</a>
		/// </summary>
		/// <param name="sender">Sender of this event</param>
		/// <param name="eventArgs">UnobservedTaskExceptionEventArgs</param>
		protected virtual void HandleTaskException(object sender, UnobservedTaskExceptionEventArgs eventArgs)
		{
			Log.Error().WriteLine(eventArgs.Exception, "Exception in Task");
			if (OnUnhandledTaskException != null)
			{
				// Make sure this doesn't cause any additional exceptions
				try
				{
					OnUnhandledTaskException?.Invoke(eventArgs.Exception);
					// Specify that the task exception is observed, this is no longer needed but anyway...
					eventArgs.SetObserved();
				}
				catch (Exception callerException)
				{
					Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledTaskException invokation");
				}
			}
		}
		#endregion
	}
}