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
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.ViewModels;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;
using Dapplo.Log;
using MahApps.Metro.IconPacks;
using Dapplo.CaliburnMicro.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu.ViewModels
{
	/// <summary>
	/// Implementation of the system-tray icon
	/// </summary>
	[Export(typeof(ITrayIconViewModel))]
	public class DemoTrayIconViewModel : TrayIconViewModel, IHandle<string>
	{
		private static readonly LogSource Log = new LogSource();

		[ImportMany("contextmenu", typeof(IMenuItem))]
		private IEnumerable<IMenuItem> ContextMenuItems { get; set; }


		[ImportMany("contextmenu", typeof(IMenuItemProvider))]
		private IEnumerable<IMenuItemProvider> ContextMenuItemProviders { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }

		public void Handle(string message)
		{
			var trayIcon = TrayIconManager.GetTrayIconFor(this);
			trayIcon.ShowBalloonTip("Event", message);
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			// Set the title of the icon (the ToolTipText) to our IContextMenuTranslations.Title
			this.BindDisplayName(ContextMenuTranslations, nameof(IContextMenuTranslations.Title));

			var items = new List<IMenuItem>();
			items.AddRange(ContextMenuItems);
			// Use IMenuItemProvider, if any
			items.AddRange(ContextMenuItemProviders.SelectMany(itemProvider => itemProvider.ProvideMenuItems()));
			items.Add(new MenuItem
			{
				Style = MenuItemStyles.Separator,
				Id = "Y_Separator"
			});
			ConfigureMenuItems(items);

			// Make sure the margin is set, do this AFTER the icon are set
			items.ApplyIconMargin(new Thickness(2, 2, 2, 2));

			IconBehavior.SetIcon(TrayIcon as FrameworkElement, new PackIconMaterial
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