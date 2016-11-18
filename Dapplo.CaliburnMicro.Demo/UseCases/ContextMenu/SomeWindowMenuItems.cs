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
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors.Security;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.UseCases.Menu.ViewModels;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu
{
	/// <summary>
	/// This provides the IMenuItem to open the WindowWithMenuViewModel
	/// </summary>
	public sealed class SomeWindowMenuItems
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		public WindowWithMenuViewModel WindowWithMenuViewModel { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }

		/// <summary>
		/// This will add an extry for the SomeWindow to the context menu.
		/// As it's imported via "Lazy" the item will only be called on the UI thread!
		/// </summary>
		[Export("contextmenu", typeof(IMenuItem))]
		public IMenuItem SomeWindowMenuItem
		{
			get {
				var menuItem = new AuthenticatedMenuItem<Visibility>
				{
					Permission = "Admin",
					AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility,
					WhenPermission = Visibility.Visible,
					WhenPermissionMissing = Visibility.Collapsed,
					Icon = new PackIconMaterial
					{
						Kind = PackIconMaterialKind.ViewList
					},
					ClickAction = clickedItem =>
					{
						Log.Debug().WriteLine("SomeWindow");
						WindowManager.ShowWindow(WindowWithMenuViewModel);
					}
				};
				// Binding without disposing
				ContextMenuTranslations.CreateBinding(menuItem, nameof(IContextMenuTranslations.SomeWindow));
				return menuItem;
			}
		}
	}
}