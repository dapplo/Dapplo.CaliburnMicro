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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Dapplo.Addons;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Log;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <summary>
    ///     This extends the System.Windows.Application to make it easier to startup you application.
    ///     It will initialize MEF, Caliburn.Micro, handle exceptions and more.
    /// </summary>
    public sealed class Dapplication : Application
    {
        private static readonly LogSource Log = new LogSource();

        private readonly ApplicationBootstrapper _bootstrapper;

        /// <summary>
        ///     Create the Dapplication for the specified application name
        ///     The mutex is created and locked in the contructor, and some of your application logic might depend on this.
        ///     Additionally the current or a matching IniConfig and LanguageLoader are added to help resoving confiration and
        ///     language imports.
        /// </summary>
        /// <param name="applicationName">Name of your application</param>
        /// <param name="mutexId">
        ///     string with an ID for your mutex, preferably a Guid. If the mutex can't be locked, the
        ///     bootstapper will not  be able to "bootstrap".
        /// </param>
        /// <param name="global">Is the mutex a global or local block (false means only in this Windows session)</param>
        public Dapplication(string applicationName, string mutexId = null, bool global = false) : this(new ApplicationBootstrapper(applicationName, mutexId, global))
        {
        }

        /// <summary>
        ///     Create the Dapplication for the ApplicationBootstrapper
        /// </summary>
        /// <param name="applicationBootstrapper">a configured ApplicationBootstrapper</param>
        public Dapplication(ApplicationBootstrapper applicationBootstrapper)
        {
            _bootstrapper = applicationBootstrapper;
            Current = this;
            // Hook unhandled exceptions in the Dispatcher
            DispatcherUnhandledException += HandleDispatcherException;

            // Hook unhandled exceptions in the AppDomain
            AppDomain.CurrentDomain.UnhandledException += HandleAppDomainException;

            // Hook unhandled exceptions in tasks
            TaskScheduler.UnobservedTaskException += HandleTaskException;

            // Make the bootstrapper stop when the CurrentDispatcher is going to shutdown, this uses a little hack to make sure there is no block
            Dispatcher.CurrentDispatcher.ShutdownStarted += (s, e) => StopBootstrapperAsync().WaitWithNestedMessageLoop();
        }

        /// <summary>
        ///     Allows access to the Dapplo.Addons.ApplicationBootstrapper
        /// </summary>
        public IBootstrapper Bootstrapper => _bootstrapper;

        /// <summary>
        ///     Access the current Dapplication
        /// </summary>
        public new static Dapplication Current { get; private set; }

        /// <summary>
        ///     This is called when the application is alreay running
        ///     Facts:
        ///     1: it will be run on the UI thread and
        ///     2: Caliburn.Micro is actually fully configured
        ///     3: Dapplo Startup is NOT made, so you have no access to ILanguage etc (yet?)
        /// </summary>
        public Action OnAlreadyRunning { get; set; }

        /// <summary>
        ///     Make sure we startup everything after WPF instanciated
        /// </summary>
        /// <param name="startupEventArgs">StartupEventArgs</param>
        protected override async void OnStartup(StartupEventArgs startupEventArgs)
        {
            // Enable UI access for different Dapplo packages, especially the UiContext.RunOn
            // This only works here, not before the Application is started and not later
            UiContext.Initialize();
            
            // Load the Dapplo.CaliburnMicro.* assemblies
            _bootstrapper.FindAndLoadAssemblies("Dapplo.CaliburnMicro.*");

            // Prepare the bootstrapper
            await _bootstrapper.InitializeAsync();

            // Export the UI SynchronizationContext, this can be retrieved by specifying:
            // [Import("ui", typeof(SynchronizationContext))] on a SynchronizationContext property / constructor argument
            _bootstrapper.Export("ui", SynchronizationContext.Current);

            // The following makes sure that Caliburn.Micro is correctly initialized on the right thread and Execute.OnUIThread works
            var caliburnBootstrapper = _bootstrapper.GetExport<CaliburnMicroBootstrapper>().Value;
            caliburnBootstrapper.Initialize();

            // Now check if there is a lock, if so we invoke OnAlreadyRunning and return
            if (!_bootstrapper.IsMutexLocked)
            {
                OnAlreadyRunning?.Invoke();
                return;
            }

            // Start Dapplo, do not use configure-await false here, so the OnStartup doesn't have any issues
            await _bootstrapper.RunAsync();

            // This also triggers the Caliburn.Micro.BootstrapperBase.OnStartup
            base.OnStartup(startupEventArgs);
        }

        /// <summary>
        ///     Helper method to stop the bootstrapper, if needed
        /// </summary>
        private async Task StopBootstrapperAsync()
        {
            if (_bootstrapper.IsInitialized)
            {
                await _bootstrapper.StopAsync().ConfigureAwait(false);
            }
        }

        #region Error handling

        /// <summary>
        ///     This is called when exceptions occure inside a dispatched call
        /// </summary>
        public Action<Exception> OnUnhandledDispatcherException { get; set; }

        /// <summary>
        ///     This is called when Exceptions are not handled inside the applications dispatcher
        ///     It will call the OnUnhandledException action, which can be used to display a message.
        ///     Only when an action is assigned, and not throw an exception, the application will not be terminated.
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="eventArgs">DispatcherUnhandledExceptionEventArgs</param>
        private void HandleDispatcherException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            Log.Error().WriteLine(eventArgs.Exception, "Exception in Dispatcher");
            if (OnUnhandledDispatcherException == null)
            {
                return;
            }
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

        /// <summary>
        ///     This is called when exceptions occure inside the AppDomain (everywhere)
        ///     Exception is the reason, the boolean specifies if your application will be terminated.
        /// </summary>
        public Action<Exception, bool> OnUnhandledAppDomainException { get; set; }

        /// <summary>
        ///     This is called when Exceptions are not handled inside the (current) AppDomain
        ///     It will call the OnUnhandledAppDomainException action, which can be used to display a message.
        ///     Implementing an action can NOT prevent termination of your application!
        ///     It may, or may not, be terminated
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="eventArgs">UnhandledExceptionEventArgs</param>
        private void HandleAppDomainException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            if (eventArgs.IsTerminating)
            {
                Log.Error().WriteLine("An exception the the current AppDomain will terminate your application.");
            }
            var exception = eventArgs.ExceptionObject as Exception;
            Log.Error().WriteLine(exception, "Exception in AppDomain");
            if (OnUnhandledAppDomainException == null)
            {
                return;
            }
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

        /// <summary>
        ///     Specifies if an UnhandledTaskException is logged and processed
        /// </summary>
        public bool ObserveUnhandledTaskException { get; set; } = true;

        /// <summary>
        ///     This is called when exceptions occure inside Tasks (everywhere)
        /// </summary>
        public Action<Exception> OnUnhandledTaskException { get; set; }

        /// <summary>
        ///     This is called when Exceptions are not handled inside Tasks
        ///     It will call the OnUnhandledTaskException action, which can be used to display a message or do something else.
        ///     In .NET before 4.5 this would terminate your application, since 4.5 it does not.
        ///     Unless you change the configuration, see
        ///     <a
        ///         href="https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskscheduler.unobservedtaskexception.aspx">
        ///         here
        ///     </a>
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="eventArgs">UnobservedTaskExceptionEventArgs</param>
        private void HandleTaskException(object sender, UnobservedTaskExceptionEventArgs eventArgs)
        {
            Log.Error().WriteLine(eventArgs.Exception, "Exception in Task");
            if (!ObserveUnhandledTaskException || OnUnhandledTaskException == null)
            {
                return;
            }
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

        #endregion
    }
}