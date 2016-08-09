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
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.ViewModels;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using Dapplo.Log.Facade;
using MahApps.Metro.IconPacks;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu.ViewModels
{
	[Export(typeof(ITrayIconViewModel))]
	public class DemoTrayIconViewModel : TrayIconViewModel, IHandle<string>, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();

		[ImportMany("contextmenu", typeof(IMenuItem))]
		private IEnumerable<IMenuItem> ContextMenuItems { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

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
			items.Add(new MenuItem
			{
				IsSeparator = true,
				Id = "Y_Separator"
			});
			ConfigureMenuItems(items);

		}

		protected override void OnActivate()
		{
			base.OnActivate();

			SetIcon(new PackIconMaterial
			{
				Kind = PackIconMaterialKind.Apps,
				Background = Brushes.White,
				Foreground = Brushes.Black,
			});
			Show();
			EventAggregator.Subscribe(this);
		}

		public override void Click() 
		{
			Log.Debug().WriteLine("ShowSomething");
			TrayIcon.ShowBalloonTip<NotificationExampleViewModel>();
		}
	}
}