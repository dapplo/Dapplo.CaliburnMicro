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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels;
using Dapplo.CaliburnMicro.Demo.UseCases.Wizard.ViewModels;
using Dapplo.CaliburnMicro.Demo.ViewModels;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.Log.Facade;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class TrayIconViewModel : Screen, ITrayIconViewModel, IHandle<string>, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();

		[ImportMany]
		private IEnumerable<IMenuItem> ContextMenuItems { get; set; }

		public ObservableCollection<IMenuItem> Items { get; } = new ObservableCollection<IMenuItem>();

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public ITrayIconManager TrayIconManager { get; set; }

		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		public ConfigViewModel ConfigViewModel { get; set; }

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
			// TODO: Closing the ConfigViewModel also closes other windows, check / fix
			if (WindowManager.ShowDialog(ConfigViewModel) == false)
			{
				// Lookup my tray icon
				var trayIcon = TrayIconManager.GetTrayIconFor(this);
				trayIcon.ShowBalloonTip("Cancelled", "You cancelled the configuration");
			}
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

		public void OnImportsSatisfied()
		{
			foreach (var contextMenuItem in ContextMenuItems.CreateTree())
			{
				Items.Add(contextMenuItem);
			}
		}
	}
}