// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Autofac;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.CaliburnMicro.Diagnostics.ViewModels;

namespace Dapplo.CaliburnMicro.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public static class DapplicationExtensions
    {
        /// <summary>
        /// Handle exceptions, and display them in a view.
        /// Make sure that 
        /// </summary>
        /// <param name="application">Dapplication to handle the exceptions for</param>
        public static void DisplayErrorView(this Dapplication application)
        {
            application.OnUnhandledAppDomainException += (exception, b) => DisplayErrorViewModel(exception);
            application.OnUnhandledDispatcherException += DisplayErrorViewModel;
            application.OnUnhandledTaskException += DisplayErrorViewModel;
        }

        private static void DisplayErrorViewModel(Exception exception)
        {
            var windowManager = Dapplication.Current.Bootstrapper.Container.Resolve<IWindowManager>();
            var errorViewModel = Dapplication.Current.Bootstrapper.Container.Resolve<ErrorViewModel>();
            if (windowManager == null || errorViewModel == null)
            {
                return;
            }

            errorViewModel.SetExceptionToDisplay(exception);
            windowManager.ShowWindow(errorViewModel);
        }
    }
}
