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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Disposables;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Menu.ViewModels
{
	[Export]
	public class WindowWithMenuViewModel : Screen
	{
		/// <summary>
		/// Here all disposables are registered, so we can clean the up
		/// </summary>
		private CompositeDisposable _disposables;

		// ReSharper disable once CollectionNeverQueried.Global
		public ObservableCollection<ITreeNode<IMenuItem>> Items { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

		[Import]
		private IMenuTranslations MenuTranslations { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }
		
		[ImportMany("menu", typeof(IMenuItem))]
		private IEnumerable<Lazy<IMenuItem>> MenuItems { get; set; }

		protected override void OnActivate()
		{
			// Prepare disposables
			_disposables?.Dispose();
			_disposables = new CompositeDisposable();

			// Remove all items, so we can build them
			Items.Clear();

			var contextMenuNameBinding = ContextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.SomeWindow));

			// Make sure the contextMenuNameBinding is disposed when this is no longer active
			_disposables.Add(contextMenuNameBinding);
			
			var items = MenuItems.Select(x => x.Value).ToList();
			var fileMenuItem = new MenuItem
			{
				Id = "1_File"
			};
			var menuNameBinding = MenuTranslations.CreateDisplayNameBinding(fileMenuItem, nameof(IMenuTranslations.File));
			// Make sure the menuNameBinding is disposed when this is no longer active
			_disposables.Add(menuNameBinding);
			items.Add(fileMenuItem);

			var editMenuItem = new MenuItem
			{
				Id = "2_Edit"
			};
			menuNameBinding.AddDisplayNameBinding(editMenuItem, nameof(IMenuTranslations.Edit));
			items.Add(editMenuItem);

			var aboutMenuItem = new MenuItem
			{
				Id = "3_About"
			};
			menuNameBinding.AddDisplayNameBinding(aboutMenuItem, nameof(IMenuTranslations.About));
			items.Add(aboutMenuItem);

			items.Add(new MenuItem
			{
				Style = MenuItemStyles.Separator,
				Id = "Y_Separator",
				ParentId = "1_File"
			});

			var exitMenuItem = new MenuItem
			{
				Id = "Z_Exit",
				ParentId = "1_File",
				ClickAction = clickedMenuItem => Dapplication.Current.Shutdown()
			};
			contextMenuNameBinding.AddDisplayNameBinding(exitMenuItem, nameof(IContextMenuTranslations.Exit));
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