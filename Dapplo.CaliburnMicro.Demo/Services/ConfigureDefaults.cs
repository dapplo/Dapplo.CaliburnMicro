#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.CaliburnMicro.Metro;

#endregion

namespace Dapplo.CaliburnMicro.Demo.Services
{
	[StartupAction(StartupOrder = (int) CaliburnStartOrder.Bootstrapper + 1)]
	public class ConfigureDefaults : IStartupAction
	{
		[Import(typeof(IWindowManager))]
		private MetroWindowManager MetroWindowManager { get; set; }

		[Import]
		public IDemoConfiguration DemoConfiguration { get; set; }

		public Task StartAsync(CancellationToken token = new CancellationToken())
		{
			MetroWindowManager.ChangeTheme(DemoConfiguration.Theme);
			MetroWindowManager.ChangeThemeAccent(DemoConfiguration.ThemeAccent);
			return Task.FromResult(true);
		}
	}
}