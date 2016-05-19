//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro.Demo
// 
//  Dapplo.CaliburnMicro.Demo is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro.Demo is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro.Demo. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.LogFacade;
using Dapplo.CaliburnMicro.Demo.Models;
using Caliburn.Micro;
using System.Windows;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class TrayIconViewModel : Screen, ITrayIconViewModel
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		public ITrayIconManager TrayIconManager { get; set; }

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		public bool CanShowSomething()
		{
			return true;
		}

		public void Configure()
		{
			Log.Debug().WriteLine("Configure");
		}

		public void Exit()
		{
			Log.Debug().WriteLine("Exit");
			Application.Current.Shutdown();
		}

		public void ShowSomething()
		{
			Log.Debug().WriteLine("ShowSomething");
			// Lookup my tray icon
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowInfoBalloonTip("Clicked", "You clicked the icon");
		}

		public void Update()
		{
			Log.Debug().WriteLine("Update");
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.Show();
		}
	}
}