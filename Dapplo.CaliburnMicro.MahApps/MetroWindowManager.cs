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
//  along with Dapplo.CaliburnMicro If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Collections.Generic;

#endregion

namespace Dapplo.CaliburnMicro.MahApps
{
	/// <summary>
	///     This comes from https://github.com/ziyasal/Caliburn.Metro/blob/master/Caliburn.Metro.Core/MetroWindowManager.cs
	/// </summary>
	[Export(typeof(IWindowManager))]
	public class MetroWindowManager : WindowManager
	{
		private readonly IList<ResourceDictionary> _resourceDictionaries = LoadResources();

		/// <summary>
		/// Add a single ResourceDictionary so it will be used by this MetroWindowManager
		/// An example would be for the Icons.xaml (which is in MahApps.Metro.Resources)
		/// </summary>
		/// <param name="resourceDictionary"></param>
		public void AddResourceDictionary(ResourceDictionary resourceDictionary)
		{
			_resourceDictionaries.Add(resourceDictionary);
		}

		/// <summary>
		/// Set the ResourceDictionary for the newly generated window
		/// </summary>
		/// <param name="window"></param>
		private void AddMetroResources(MetroWindow window)
		{
			foreach (var dictionary in _resourceDictionaries)
			{
				window.Resources.MergedDictionaries.Add(dictionary);
			}
		}

		/// <summary>
		/// Implement this to make specific configuration changes to your window.
		/// </summary>
		/// <param name="window"></param>
		public virtual void ConfigureWindow(MetroWindow window)
		{
		}

		/// <summary>
		/// Create a MetroWindow
		/// </summary>
		/// <param name="view"></param>
		/// <param name="windowIsView"></param>
		/// <returns></returns>
		public virtual MetroWindow CreateCustomWindow(object view, bool windowIsView)
		{
			MetroWindow result;
			if (windowIsView)
			{
				result = view as MetroWindow;
			}
			else
			{
				result = new MetroWindow
				{
					Content = view
				};
			}

			AddMetroResources(result);
			return result;
		}

		/// <summary>Makes sure the view is a window or is wrapped by one.</summary>
		/// <param name="model">The view model.</param>
		/// <param name="view">The view.</param>
		/// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
		/// <returns>The window.</returns>
		protected override Window EnsureWindow(object model, object view, bool isDialog)
		{
			MetroWindow window = null;
			Window inferOwnerOf;
			if (view is MetroWindow)
			{
				window = CreateCustomWindow(view, true);
				inferOwnerOf = InferOwnerOf(window);
				if (inferOwnerOf != null && isDialog)
				{
					window.Owner = inferOwnerOf;
				}
			}

			if (window == null)
			{
				window = CreateCustomWindow(view, false);
			}

			ConfigureWindow(window);
			window.SetValue(View.IsGeneratedProperty, true);
			inferOwnerOf = InferOwnerOf(window);
			if (inferOwnerOf != null)
			{
				window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				window.Owner = inferOwnerOf;
			}
			else
			{
				window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}

			return window;
		}

		/// <summary>
		/// Load the resources, like styles etc, to make the available to the MetroWindow
		/// </summary>
		/// <returns></returns>
		private static IList<ResourceDictionary>LoadResources()
		{
			string[] styles = { "Colors", "Fonts", "Controls", "Controls.AnimatedSingleRowTabControl", "Accents/Blue" };
			return (
				from style in styles
				select new ResourceDictionary { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml", UriKind.RelativeOrAbsolute) }
			).ToList();
		}
	}
}