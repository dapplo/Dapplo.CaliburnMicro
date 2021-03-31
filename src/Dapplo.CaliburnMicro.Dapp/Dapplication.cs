﻿// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Dapplo.Addons;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <summary>
    ///     This extends the System.Windows.Application to make it easier to startup you application.
    ///     It will initialize MEF, Caliburn.Micro, handle exceptions and more.
    /// </summary>
    public sealed class Dapplication : Application
    {
        private static readonly LogSource Log = new LogSource();
        private CaliburnMicroBootstrapper _caliburnMicroBootstrapper;
        private readonly ApplicationBootstrapper _bootstrapper;

        /// <summary>
        ///     Create the Dapplication with the specified ApplicationConfig
        ///     Take care that you called WithCaliburnMicro on your builder, or added the needed assemblies yourself
        /// </summary>
        /// <param name="applicationConfig">ApplicationConfig</param>
        public Dapplication(ApplicationConfig applicationConfig)
        {
            _bootstrapper = new ApplicationBootstrapper(applicationConfig);

            Current = this;
            // Hook unhandled exceptions in the Dispatcher
            DispatcherUnhandledException += HandleDispatcherException;

            // Hook unhandled exceptions in the AppDomain
            AppDomain.CurrentDomain.UnhandledException += HandleAppDomainException;

            // Hook unhandled exceptions in tasks
            TaskScheduler.UnobservedTaskException += HandleTaskException;

            // Make the bootstrapper stop when the CurrentDispatcher is going to shutdown, this uses a little hack to make sure there is no block
            Dispatcher.CurrentDispatcher.ShutdownStarted += (s, e) =>
            {
                StopBootstrapperAsync().WaitWithNestedMessageLoop();
            };
        }

        /// <summary>
        ///     Allows access to the Dapplo.Addons.ApplicationBootstrapper
        /// </summary>
        public ApplicationBootstrapper Bootstrapper => _bootstrapper;

        /// <summary>
        /// Returns true if the application was already running, only works when a mutex is used
        /// </summary>
        public bool WasAlreadyRunning => _bootstrapper.IsAlreadyRunning;

        /// <summary>
        ///     Access the current Dapplication
        /// </summary>
        public new static Dapplication Current { get; private set; }

        /// <summary>
        ///     This function is called when the application is already running
        ///     Facts:
        ///     1: it will be run on the UI thread and
        ///     2: Caliburn.Micro is actually fully configured
        ///     3: Dapplo Startup is NOT made, so you have no access to ILanguage etc (yet?)
        ///     4: The application will be shut down for you, when the function returns and uses the returned error code for the exit
        /// </summary>
        public Func<int> OnAlreadyRunning { get; set; }

        /// <summary>
        ///     Make sure we startup everything after WPF instantiated
        /// </summary>
        /// <param name="e">StartupEventArgs</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            Log.Debug().WriteLine("Starting application startup");

            _bootstrapper.Configure();

            // Register the UI SynchronizationContext, this can be retrieved by specifying the name "ui" for the argument
            _bootstrapper.Builder.RegisterInstance(SynchronizationContext.Current).Named<SynchronizationContext>("ui");
            _bootstrapper.Builder.RegisterInstance(TaskScheduler.FromCurrentSynchronizationContext()).Named<TaskScheduler>("ui");
            // The following makes sure that Caliburn.Micro is correctly initialized on the right thread and Execute.OnUIThread works
            // Very important to do this, after all assemblies are loaded!
            _caliburnMicroBootstrapper = new CaliburnMicroBootstrapper(_bootstrapper);
            _bootstrapper.Builder.RegisterInstance(_caliburnMicroBootstrapper).As<IService>().SingleInstance();

            // Prepare the bootstrapper
            await _bootstrapper.InitializeAsync().ConfigureAwait(true);

            _caliburnMicroBootstrapper.Initialize();

            // Now check if there is a lock, if so we invoke OnAlreadyRunning and return
            if (_bootstrapper.IsAlreadyRunning)
            {
                var exitCode = OnAlreadyRunning?.Invoke() ?? -1;
                Shutdown(exitCode);
                return;
            }

            // Start Dapplo, services which need a UI context need to configure this
            await _bootstrapper.StartupAsync().ConfigureAwait(true);

            Log.Debug().WriteLine("Finished application startup");
            // This also triggers the Caliburn.Micro.BootstrapperBase.OnStartup
            base.OnStartup(e);
        }

        /// <summary>
        ///     Helper method to stop the bootstrapper, if needed
        /// </summary>
        private async Task StopBootstrapperAsync()
        {
            // TODO: What to disable here? Ist un-registering the exception handlers here smart?
            // Unhook unhandled exceptions in the Dispatcher
            DispatcherUnhandledException -= HandleDispatcherException;

            // Unhook unhandled exceptions in the AppDomain
            AppDomain.CurrentDomain.UnhandledException -= HandleAppDomainException;

            // Unhook unhandled exceptions in tasks
            TaskScheduler.UnobservedTaskException -= HandleTaskException;

            // Shutdown Caliburn Micro
            _caliburnMicroBootstrapper.Shutdown();

            await _bootstrapper.ShutdownAsync().ConfigureAwait(true);
            // Make sure everything is disposed (and all disposables which were registered via _bootstrapper.RegisterForDisposal() are called)
            _bootstrapper.Dispose();
        }

        /// <summary>
        ///     This is called when exceptions occur inside a dispatched call
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
                Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledDispatcherException invocation");
            }
        }

        /// <summary>
        ///     This is called when exceptions occur inside the AppDomain (everywhere)
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
                Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledDispatcherException invocation");
            }
        }

        /// <summary>
        ///     Specifies if an UnhandledTaskException is logged and processed
        /// </summary>
        public bool ObserveUnhandledTaskException { get; set; } = true;

        /// <summary>
        ///     This is called when exceptions occur inside Tasks (everywhere)
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
                Log.Error().WriteLine(callerException, "An exception was thrown in the OnUnhandledTaskException invocation");
            }
        }
    }
}