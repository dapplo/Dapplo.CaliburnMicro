//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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
