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
using System.Linq;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Tree;
using Dapplo.Utils;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Menu.ViewModels
{
	[Export]
	public class WindowWithMenuViewModel : Screen
	{
		private readonly Disposables _disposables = new Disposables();

		// ReSharper disable once CollectionNeverQueried.Global
		public ObservableCollection<ITreeNode<IMenuItem>> Items { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

		[Import]
		private IMenuTranslations MenuTranslations { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }
		
		[ImportMany("menu", typeof(IMenuItem))]
		private IEnumerable<IMenuItem> MenuItems { get; set; }

		protected override void OnActivate()
		{
			var items = MenuItems.ToList();
			var menuTranslationsObservable = MenuTranslations.ToObservable();
			_disposables.Add(menuTranslationsObservable);

			var contextMenuTranslationObservable = ContextMenuTranslations.ToObservable();
			_disposables.Add(contextMenuTranslationObservable);

			this.BindDisplayName(contextMenuTranslationObservable, nameof(IContextMenuTranslations.SomeWindow));
			var fileMenuItem = new MenuItem
			{
				Id = "1_File"
			};
			fileMenuItem.BindDisplayName(menuTranslationsObservable, nameof(IMenuTranslations.File));
			items.Add(fileMenuItem);

			var editMenuItem = new MenuItem
			{
				Id = "2_Edit"
			};
			editMenuItem.BindDisplayName(menuTranslationsObservable, nameof(IMenuTranslations.Edit));
			items.Add(editMenuItem);

			var aboutMenuItem = new MenuItem
			{
				Id = "3_About"
			};
			aboutMenuItem.BindDisplayName(menuTranslationsObservable, nameof(IMenuTranslations.About));
			items.Add(aboutMenuItem);

			items.Add(new MenuItem
			{
				IsSeparator = true,
				Id = "Y_Separator",
				ParentId = "1_File"
			});

			var exitMenuItem = new MenuItem
			{
				Id = "Z_Exit",
				ParentId = "1_File",
				ClickAction = clickedMenuItem => Dapplication.Current.Shutdown()
			};
			exitMenuItem.BindDisplayName(contextMenuTranslationObservable, nameof(IContextMenuTranslations.Exit));
			items.Add(exitMenuItem);

			// Make sure all items are initialized
			foreach (var menuItem in items)
			{
				menuItem.Initialize();
			}
			// Add the root elements to the Items
			foreach (var item in items.CreateTree())
			{
				Items.Add(item);
			}

			base.OnActivate();
		}

		/// <summary>
		/// Called when deactivating.
		/// Removes all event subscriptions
		/// </summary>
		/// <param name="close">Inidicates whether this instance will be closed.</param>
		protected override void OnDeactivate(bool close)
		{
			_disposables.Dispose();
			base.OnDeactivate(close);
		}
	}
}