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
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Menu.ViewModels
{
	[Export]
	public class WindowWithMenuViewModel : Screen, IPartImportsSatisfiedNotification
	{
		public ObservableCollection<ITreeNode<IMenuItem>> Items { get; } = new ObservableCollection<ITreeNode<IMenuItem>>();

		[ImportMany("menu", typeof(IMenuItem))]
		private IEnumerable<IMenuItem> MenuItems { get; set; }

		public void OnImportsSatisfied()
		{
			var items = MenuItems.ToList();

			items.Add(new MenuItem
			{
				Id = "1_File",
				DisplayName= "File"
			});
			items.Add(new MenuItem
			{
				Id = "2_Edit",
				DisplayName = "Edit"
			});
			items.Add(new MenuItem
			{
				Id = "3_About",
				DisplayName = "About"
			});

			items.Add(new SeparatorMenuItem
			{
				Id = "Y_Separator", ParentId = "1_File"
			});
			items.Add(new MenuItem
			{
				Id = "Z_Edit",
				ParentId = "1_File",
				DisplayName = "Exit",
				ClickAction = (menuItem) => Dapplication.Current.Shutdown()
			});

			foreach (var menuItem in items.CreateTree())
			{
				Items.Add(menuItem);
			}
		}
	}
}