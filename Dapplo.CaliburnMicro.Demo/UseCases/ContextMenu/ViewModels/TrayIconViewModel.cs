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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.ViewModels;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.Tree;
using Dapplo.Log.Facade;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class TrayIconViewModel : Screen, ITrayIconViewModel, IHandle<string>, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();

		[ImportMany]
		private IEnumerable<IMenuItem> ContextMenuItems { get; set; }

		public ObservableCollection<ITreeNode<IMenuItem>> Items { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

		[Import]
		public IContextMenuTranslations ContextMenuTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public ITrayIconManager TrayIconManager { get; set; }

		[Import]
		public IWindowManager WindowManager { get; set; }

		public void Handle(string message)
		{
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowBalloonTip("Event", message);
		}

		public void OnImportsSatisfied()
		{
			var items = ContextMenuItems.ToList();
			items.Add(new SeparatorMenuItem
			{
				Id = "Y_Separator"
			});
			foreach (var contextMenuItem in items.CreateTree())
			{
				Items.Add(contextMenuItem);
			}
		}

		public bool CanShowSomething()
		{
			return true;
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
	}
}