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
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Dapplo.Log.Facade;
using Dapplo.Utils.Embedded;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Dapplo.CaliburnMicro.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Metro
{
	/// <summary>
	///     This (slightly modified) comes from
	///     <a href="https://github.com/ziyasal/Caliburn.Metro/blob/master/Caliburn.Metro.Core/MetroWindowManager.cs">here</a>
	///     and providers a Caliburn.Micro IWindowManager implementation. The Dapplo.CaliburnMicro.CaliburnMicroBootstrapper
	///     will
	///     take care of taking this (if available) and the MetroWindowManager will take care of instanciating a MetroWindow.
	///     Note: Currently there is no support for the DialogCoordinator yet..
	///     For more information see <a href="https://gist.github.com/ButchersBoy/4a7272f3ac104c5b1a54">here</a> and
	///     <a href="https://dragablz.net/2015/05/29/using-mahapps-dialog-boxes-in-a-mvvm-setup/">here</a>
	/// </summary>
	[Export(typeof(IWindowManager))]
	public class MetroWindowManager : WindowManager, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();
		private static readonly string[] Styles =
		{
			"Colors", "Fonts", "Controls", "Controls.AnimatedSingleRowTabControl"
		};

		/// <summary>
		///     The current theme accent
		/// </summary>
		public ThemeAccents ThemeAccent { get; private set; }

		/// <summary>
		///     The current theme
		/// </summary>
		public Themes Theme { get; private set; }

		/// <summary>
		///     Export the IDialogCoordinator of MahApps, so ViewModels can open MahApps dialogs
		/// </summary>
		[Export]
		public IDialogCoordinator MahAppsDialogCoordinator => DialogCoordinator.Instance;

		/// <summary>
		///     Implement this to make specific configuration changes to your window.
		/// </summary>
		public Action<MetroWindow> OnConfigureWindow { get; set; }

		/// <summary>
		///     Implement this to make specific configuration changes to your owned (dialog) window.
		/// </summary>
		public Action<MetroWindow> OnConfigureOwnedWindow { get; set; }

		/// <summary>
		///     Called when a part's imports have been satisfied and it is safe to use.
		/// </summary>
		public void OnImportsSatisfied()
		{
			foreach (var style in Styles)
			{
				AddMahappsStyle(style);
			}
			// Just in case, remove them before adding
			RemoveMahappsStyle($"Accents/{Theme}");
			RemoveMahappsStyle($"Accents/{ThemeAccent}");

			if (ThemeAccent == ThemeAccents.Default)
			{
				ThemeAccent = ThemeAccents.Blue;
			}
			if (Theme == Themes.Default)
			{
				Theme = Themes.BaseLight;
			}

			AddMahappsStyle($"Accents/{ThemeAccent}");
			AddMahappsStyle($"Accents/{Theme}");
		}

		/// <summary>
		///     Add a ResourceDictionary for the specified MahApps style
		///     The Uri to the source is created by CreateMahappStyleUri
		/// </summary>
		/// <param name="style">
		///     Style name, this is actually what is added behind
		///     pack://application:,,,/MahApps.Metro;component/Styles/ (and .xaml is added)
		/// </param>
		public void AddMahappsStyle(string style)
		{
			var packUri = CreateMahappStyleUri(style);
			// TODO: Fix Dapplo.Utils resource checking
			if (!packUri.EmbeddedResourceExists())
			{
				Log.Warn().WriteLine("Style {0} might not be available as {1}.", style, packUri);
			}
			AddResourceDictionary(packUri);
		}

		/// <summary>
		///     Add a single ResourceDictionary for the supplied source
		///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
		/// </summary>
		/// <param name="source">Uri, e.g. /Resources/Icons.xaml or </param>
		public void AddResourceDictionary(Uri source)
		{
			if (Application.Current.Resources.MergedDictionaries.All(x => x.Source != source))
			{
				var resourceDictionary = new ResourceDictionary
				{
					Source = source
				};
				Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
			}
		}

		/// <summary>
		///     Remove all ResourceDictionaries for the specified MahApps style
		///     The Uri to the source is created by CreateMahappStyleUri
		/// </summary>
		/// <param name="style">string</param>
		public void RemoveMahappsStyle(string style)
		{
			var mahappsStyleUri = CreateMahappStyleUri(style);
			foreach (var resourceDirectory in Application.Current.Resources.MergedDictionaries.ToList())
			{
				if (resourceDirectory.Source == mahappsStyleUri)
				{
					Application.Current.Resources.MergedDictionaries.Remove(resourceDirectory);
				}
			}
		}

		/// <summary>
		///     Create a MetroWindow
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
					Content = view,
					SizeToContent = SizeToContent.WidthAndHeight
				};
			}
			result?.SetResourceReference(Control.BorderBrushProperty, "AccentColorBrush");
			result?.SetValue(Control.BorderThicknessProperty, new Thickness(1));

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

			// Allow dialogs
			window.SetValue(DialogParticipation.RegisterProperty, model);
			window.SetValue(View.IsGeneratedProperty, true);
			inferOwnerOf = InferOwnerOf(window);
			if (inferOwnerOf != null)
			{
				// "Dialog", center it on top of the owner
				window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				window.Owner = inferOwnerOf;
				OnConfigureOwnedWindow?.Invoke(window);
			}
			else
			{
				// Free window, without owner
				window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				OnConfigureWindow?.Invoke(window);
			}
			var haveIcon = model as IHaveIcon;
			if (haveIcon != null && window.Icon == null)
			{
				window.Icon = haveIcon.Icon.ToBitmapSource(new Size(256,256));
			}
			// Just in case, make sure it's activated
			window.Activate();
			return window;
		}

		/// <summary>
		///     Create a MapApps Uri for the supplied style
		/// </summary>
		/// <param name="style">e.g. Fonts or Controls</param>
		/// <returns></returns>
		public static Uri CreateMahappStyleUri(string style)
		{
			return new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml", UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		///     Change the current theme
		/// </summary>
		/// <param name="theme"></param>
		public void ChangeTheme(Themes theme)
		{
			RemoveMahappsStyle($"Accents/{Theme}");
			Theme = theme;
			AddMahappsStyle($"Accents/{Theme}");
		}

		/// <summary>
		///     Change the current theme accent
		/// </summary>
		/// <param name="themeAccent">ThemeAccents</param>
		public void ChangeThemeAccent(ThemeAccents themeAccent)
		{
			RemoveMahappsStyle($"Accents/{ThemeAccent}");
			ThemeAccent = themeAccent;
			AddMahappsStyle($"Accents/{ThemeAccent}");
		}
	}
}