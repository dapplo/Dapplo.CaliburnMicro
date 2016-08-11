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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Menu
{
	/// <summary>
	///     This defines an IMenuItem
	/// </summary>
	public interface IMenuItem : ITreeNode<IMenuItem>, INotifyPropertyChanged, IHaveDisplayName
	{
		/// <summary>
		/// The initialize is called from the UI Thread before the menu-item is added to a context menu.
		/// This allows for any initialization, like icons etc, to be made
		/// </summary>
		void Initialize();

		/// <summary>
		///     Returns if the IMenuItem can be selected (visible but not usable)
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		///     Returns if the IMenuItem is visible (not visible and not usable)
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		///     The icon for the IMenuItem
		/// </summary>
		Control Icon { get; set; }

		/// <summary>
		///     Is called when the IMenuItem it clicked
		/// </summary>
		void Click(IMenuItem clickedItem);
	}
}