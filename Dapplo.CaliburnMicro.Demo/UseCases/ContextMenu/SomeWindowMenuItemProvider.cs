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
using Caliburn.Micro;
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
	/// This will add an extry for the SomeWindow to the context menu, implemented via the IMenuItemProvider
	/// </summary>
	[Export("contextmenu", typeof(IMenuItemProvider))]
	public sealed class SomeWindowMenuItemProvider : IMenuItemProvider
	{
		private static readonly LogSource Log = new LogSource();
		private MenuItem _menuItem;
		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		public WindowWithMenuViewModel WindowWithMenuViewModel { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }

		public IEnumerable<IMenuItem> ProvideMenuItems()
		{
			if (_menuItem == null)
			{
				_menuItem = new MenuItem
				{
					Icon = new PackIconMaterial
					{
						Kind = PackIconMaterialKind.ViewList
					},
					ClickAction = (clickedItem) =>
					{
						Log.Debug().WriteLine("SomeWindow");
						WindowManager.ShowWindow(WindowWithMenuViewModel);
					}
				};
				// Binding without disposing
				ContextMenuTranslations.CreateBinding(_menuItem, nameof(IContextMenuTranslations.SomeWindow));
			}
			yield return _menuItem;
		}
	}
}