//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Dapplo.Addons.Bootstrapper;

#endregion

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <summary>
    ///     An implementation of the Caliburn Micro Bootstrapper which is started from the Dapplo ApplicationBootstrapper (MEF)
    ///     and uses this.
    /// </summary>
    [Service(nameof(CaliburnServices.CaliburnMicroBootstrapper), TaskSchedulerName = "ui")]
    public class CaliburnMicroBootstrapper : BootstrapperBase, IShutdown
    {
        private readonly ApplicationBootstrapper _bootstrapper;
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        /// CaliburnMicroBootstrapper
        /// </summary>
        /// <param name="bootstrapper">Used to register, inject, export and locate</param>
        public CaliburnMicroBootstrapper(ApplicationBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        /// <inheritdoc />
        public void Shutdown()
        {
            Log.Debug().WriteLine("Starting shutdown");
            OnExit(this, new EventArgs());
            Log.Debug().WriteLine("finished shutdown");
        }

        /// <summary>
        ///     Fill imports of the supplied instance
        /// </summary>
        /// <param name="instance">some object to fill</param>
        protected override void BuildUp(object instance)
        {
            _bootstrapper.Container.InjectProperties(instance);
        }

        /// <summary>
        ///     Configure the Dapplo.Addon.Bootstrapper with the AssemblySource.Instance values
        /// </summary>
        [SuppressMessage("Sonar Code Smell", "S2696:Instance members should not write to static fields", Justification = "This is the only location where it makes sense.")]
        protected override void Configure()
        {
            LogManager.GetLog = type => new CaliburnLogger(type);

            ConfigureViewLocator();

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
                while (viewType == null && currentModelType != null && currentModelType != typeof(object) && currentModelType != typeof(Screen))
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
            return _bootstrapper.Container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
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
            if (string.IsNullOrWhiteSpace(contractName) && _bootstrapper.Container.TryResolve(service, out var instance))
            {
                return instance;
            }

            if (_bootstrapper.Container.TryResolveNamed(contractName, service, out instance))
            {
                return instance;
            }

            throw new Exception($"Could not locate any instances of contract {contractName ?? service.Name}.");
        }

        /// <inheritdoc />
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return _bootstrapper.LoadedAssemblies;
        }
    }
}