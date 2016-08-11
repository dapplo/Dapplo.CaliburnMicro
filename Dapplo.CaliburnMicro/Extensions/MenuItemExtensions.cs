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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Menu;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	///     Extensions to simplify the usage of IMenuItem
	/// </summary>
	public static class MenuItemExtensions
	{
		/// <summary>
		///     Apply the specified Thickness as margin to the Icons in the IEnumerable with IMenuItem
		/// </summary>
		/// <param name="items">IEnumerable with IMenuItem</param>
		/// <param name="thickness">Thickness for the marging</param>
		public static void ApplyIconMargin(this IEnumerable<IMenuItem> items, Thickness thickness)
		{
			foreach (var menuItem in items)
			{
				menuItem.ApplyIconMargin(thickness);
			}
		}

		/// <summary>
		///     Apply the specified Thickness as margin to the Icon in the IMenuItem
		/// </summary>
		/// <param name="item">IMenuItem</param>
		/// <param name="thickness">Thickness for the marging</param>
		public static void ApplyIconMargin(this IMenuItem item, Thickness thickness)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			if (item.Icon != null)
			{
				item.Icon.Margin = thickness;
			}
		}

		/// <summary>
		///     Apply the specified Brush as Foreground for the Icons in the IEnumerable with IMenuItem
		/// </summary>
		/// <param name="items">IEnumerable with IMenuItem</param>
		/// <param name="foregroundBrush">Brush for the Foreground</param>
		public static void ApplyIconForegroundColor(this IEnumerable<IMenuItem> items, Brush foregroundBrush)
		{
			foreach (var menuItem in items)
			{
				menuItem.ApplyIconForegroundColor(foregroundBrush);
			}
		}

		/// <summary>
		///     Apply the specified Brush as Foreground for the icon of the IMenuItem
		/// </summary>
		/// <param name="item">IMenuItem</param>
		/// <param name="foregroundBrush">Brush for the Foreground</param>
		public static void ApplyIconForegroundColor(this IMenuItem item, Brush foregroundBrush)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			if (item.Icon != null)
			{
				item.Icon.Foreground = foregroundBrush;
			}
		}
	}
}