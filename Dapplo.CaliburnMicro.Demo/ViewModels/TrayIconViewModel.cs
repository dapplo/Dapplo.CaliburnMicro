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

using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.Log.Facade;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class TrayIconViewModel : Screen, ITrayIconViewModel, IHandle<string>
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public ITrayIconManager TrayIconManager { get; set; }

		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		public IShell Shell { get; set; }

		public void Handle(string message)
		{
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowBalloonTip("Event", message);
		}

		public bool CanShowSomething()
		{
			return true;
		}

		public void Configure()
		{
			Log.Debug().WriteLine("Configure");
			WindowManager.ShowWindow(Shell);
		}

		public void Exit()
		{
			Log.Debug().WriteLine("Exit");
			Application.Current.Shutdown();
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.Show();
			EventAggregator.Subscribe(this);
		}

		public void ShowSomething()
		{
			Log.Debug().WriteLine("ShowSomething");
			// Lookup my tray icon
			var trayIcon = TrayIconManager.GetTrayIconFor(this);

			trayIcon.ShowBalloonTip<NotificationExampleViewModel>();
		}

		public void Update()
		{
			Log.Debug().WriteLine("Update");
		}
	}
}