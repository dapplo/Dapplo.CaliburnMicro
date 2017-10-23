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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;
using System.Diagnostics.CodeAnalysis;
using Dapplo.Addons.Bootstrapper.Resolving;

#endregion

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <summary>
    ///     An implementation of the Caliburn Micro Bootstrapper which is started from the Dapplo ApplicationBootstrapper (MEF)
    ///     and uses this.
    /// </summary>
    [ShutdownAction(ShutdownOrder = (int) CaliburnStartOrder.Bootstrapper)]
    [Export]
    public class CaliburnMicroBootstrapper : BootstrapperBase, IAsyncShutdownAction
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IBootstrapper _bootstrapper;

        /// <summary>
        /// CaliburnMicroBootstrapper
        /// </summary>
        /// <param name="bootstrapper">Used to inject, export and locate</param>
        [ImportingConstructor]
        public CaliburnMicroBootstrapper(
            IBootstrapper bootstrapper
            )
        {
            _bootstrapper = bootstrapper;
        }
        /// <summary>
        ///     Shutdown Caliburn
        /// </summary>
        /// <param name="token">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ShutdownAsync(CancellationToken token = default(CancellationToken))
        {
            Log.Debug().WriteLine("Starting shutdown");
            await Execute.OnUIThreadAsync(() => { OnExit(this, new EventArgs()); }).ConfigureAwait(false);
            Log.Debug().WriteLine("finished shutdown");
        }

        /// <summary>
        ///     Fill imports of the supplied instance
        /// </summary>
        /// <param name="instance">some object to fill</param>
        protected override void BuildUp(object instance)
        {
            _bootstrapper.ProvideDependencies(instance);
        }

        /// <summary>
        ///     Configure the Dapplo.Addon.Bootstrapper with the AssemblySource.Instance values
        /// </summary>
        [SuppressMessage("Sonar Code Smell", "S2696:Instance members should not write to static fields", Justification = "This is the only location where it makes sense.")]
        protected override void Configure()
        {
            LogManager.GetLog = type => new CaliburnLogger(type);

            foreach (var assembly in AssemblySource.Instance)
            {
                _bootstrapper.Add(assembly);
            }

            ConfigureViewLocator();

            // TODO: Documentation
            // Test if there is a IWindowManager available, if not use the default
            var windowManagers = _bootstrapper.GetExports<IWindowManager>();
            if (!windowManagers.Any())
            {
                _bootstrapper.Export<IWindowManager>(new DapploWindowManager());
            }

            // TODO: Documentation
            // Test if there is a IEventAggregator available, if not use the default
            var eventAggregators = _bootstrapper.GetExports<IEventAggregator>();
            if (!eventAggregators.Any())
            {
                _bootstrapper.Export<IEventAggregator>(new EventAggregator());
            }

            // TODO: Documentation
            // This make it possible to pass the data-context of the originally clicked object in the Message.Attach event bubbling.
            // E.G. the parent Menu-Item Click will get the Child MenuItem that was actually clicked.
            MessageBinder.SpecialValues.Add("$originalDataContext", context =>
            {
                var routedEventArgs = context.EventArgs as RoutedEventArgs;
                var frameworkElement = routedEventArgs?.OriginalSource as FrameworkElement;
                return frameworkElement?.DataContext;
            });
        }

        /// <summary>
        ///     Add logic to find the base viewtype if the default locator can't find a view.
        /// </summary>
        [SuppressMessage("Sonar Code Smell", "S2696:Instance members should not write to static fields", Justification = "This is the only location where it makes sense.")]
        private void ConfigureViewLocator()
        {
            var defaultLocator = ViewLocator.LocateTypeForModelType;
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = defaultLocator(modelType, displayLocation, context);
                bool initialViewFound = viewType != null;

                if (initialViewFound)
                {
                    return viewType;
                }
                Log.Verbose().WriteLine("No view for {0}, looking into base types.", modelType);
                var currentModelType = modelType;
                while (viewType == null && currentModelType != null && currentModelType != typeof(object))
                {
                    currentModelType = currentModelType.BaseType;
                    viewType = defaultLocator(currentModelType, displayLocation, context);
                }
                if (viewType != null)
                {
                    Log.Verbose().WriteLine("Found view for {0} in base type {1}, the view is {2}", modelType, currentModelType, viewType);
                }

                return viewType;
            };
        }

        /// <summary>
        ///     Return all instances of a certain service type
        /// </summary>
        /// <param name="service">Type</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _bootstrapper.GetExports(service).Select(x => x.Value);
        }

        /// <summary>
        ///     Locate an instance of a service, used in Caliburn.
        /// </summary>
        /// <param name="service">Type for the service to locate</param>
        /// <param name="contractName">string with the name of the contract</param>
        /// <returns>instance of the service</returns>
        [SuppressMessage("Sonar Code Smell", "S927:Name parameter to match the base definition", Justification = "The base name is not so clear.")]
        protected override object GetInstance(Type service, string contractName)
        {
            var contract = string.IsNullOrEmpty(contractName) ? AttributedModelServices.GetContractName(service) : contractName;
            return _bootstrapper.GetExport(service, contract);
        }

        /// <summary>
        ///     Return all assemblies that the Dapplo bootstrapper knows, this is used to find your views and viewmodels
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return AssemblyResolver.AssemblyCache;
        }
    }
}