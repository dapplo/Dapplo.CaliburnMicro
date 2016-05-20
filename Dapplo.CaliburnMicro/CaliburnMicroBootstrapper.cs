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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;

#endregion

namespace Dapplo.CaliburnMicro
{
	/// <summary>
	///     An implementation of the Caliburn Micro Bootstrapper which is started from the Dapplo ApplicationBootstrapper (MEF)
	///     and uses this.
	/// </summary>
	[StartupAction(StartupOrder = (int) CaliburnStartOrder.Bootstrapper)]
	public class CaliburnMicroBootstrapper : BootstrapperBase, IStartupAction
	{
		[Import]
		private IServiceExporter ServiceExporter { get; set; }

		[Import]
		private IServiceLocator ServiceLocator { get; set; }

		[Import]
		private IServiceRepository ServiceRepository { get; set; }

		/// <summary>
		///     Initialize the Caliburn bootstrapper from the Dapplo startup
		/// </summary>
		/// <param name="token">CancellationToken</param>
		public Task StartAsync(CancellationToken token = default(CancellationToken))
		{
			LogManager.GetLog = type => new CaliburnLogger(type);

			Initialize();

			OnStartup(this, null);

			return Task.FromResult(true);
		}

		/// <summary>
		///     Fill imports of the supplied instance
		/// </summary>
		/// <param name="instance">some object to fill</param>
		protected override void BuildUp(object instance)
		{
			ServiceLocator.FillImports(instance);
		}

		/// <summary>
		///     Configure the Dapplo.Addon.Bootstrapper with the AssemblySource.Instance values
		/// </summary>
		protected override void Configure()
		{
			foreach (var assembly in AssemblySource.Instance)
			{
				ServiceRepository.Add(assembly);
			}
			// Test if there is a IWindowManager available, if not use the default
			var windowManagers = ServiceLocator.GetExports<IWindowManager>();
			if (!windowManagers.Any())
			{
				ServiceExporter.Export<IWindowManager>(new WindowManager());
			}

			// Test if there is a IEventAggregator available, if not use the default
			var eventAggregators = ServiceLocator.GetExports<IEventAggregator>();
			if (!eventAggregators.Any())
			{
				ServiceExporter.Export<IEventAggregator>(new EventAggregator());
			}
		}

		/// <summary>
		///     Return all instances of a certain service type
		/// </summary>
		/// <param name="serviceType"></param>
		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return ServiceLocator.GetExports(serviceType).Select(x => x.Value);
		}

		/// <summary>
		///     Locate an instance of a service, used in Caliburn.
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="key"></param>
		/// <returns>instance</returns>
		protected override object GetInstance(Type serviceType, string key)
		{
			var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
			return ServiceLocator.GetExport(serviceType, contract);
		}

		/// <summary>
		///     This is the startup of the Caliburn bootstrapper, here the only implementation of IShell is displayed as root view
		/// </summary>
		/// <param name="sender">object, as it's called internally this is actually null</param>
		/// <param name="e">StartupEventArgs, as it's called internally this is actually null</param>
		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			// Display the IShell viewmodel
			DisplayRootViewFor<IShell>();
		}

		/// <summary>
		///     Return all assemblies that the Dapplo Bootstrapper knows of
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable<Assembly> SelectAssemblies()
		{
			return ServiceRepository.KnownAssemblies;
		}
	}
}