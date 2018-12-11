//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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
using System.Linq;
using System.Windows;
using Dapplo.Addons;
using Dapplo.Log;
using Dapplo.CaliburnMicro.Metro.Configuration;
using MahApps.Metro;

#endregion

namespace Dapplo.CaliburnMicro.Metro
{
    /// <summary>
    /// This 
    /// </summary>
    public sealed class MetroThemeManager
    {
        private readonly IResourceProvider _resourceProvider;
        private static readonly LogSource Log = new LogSource();

        private static readonly string[] Styles =
        {
            "Controls", "Fonts", "Controls.AnimatedSingleRowTabControl"
        };

        private ThemeAccents? _themeAccent;
        private Themes? _theme;

        /// <summary>
        /// Default constructor taking care of initialization
        /// </summary>
        public MetroThemeManager(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            foreach (var style in Styles)
            {
                AddMahappsStyle(style);
            }
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
            if (!_resourceProvider.EmbeddedResourceExists(packUri))
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
            if (Application.Current.Resources.MergedDictionaries.Any(x => x.Source == source))
            {
                return;
            }

            var resourceDictionary = new ResourceDictionary
            {
                Source = source
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
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
        ///     Change the current theme
        /// </summary>
        /// <param name="theme">Themes</param>
        /// <param name="themeAccent">ThemeAccents</param>
        public void ChangeTheme(Themes? theme, ThemeAccents? themeAccent)
        {
            if (_themeAccent.HasValue && _theme.HasValue)
            {
                // Remove current
                RemoveMahappsStyle($"Themes/{_theme.Value}.{_themeAccent.Value}");
            }

            if (theme.HasValue)
            {
                _theme = theme;
            }
            if(themeAccent.HasValue)
            {
                _themeAccent = themeAccent;
            }

            if (!_themeAccent.HasValue || !_theme.HasValue)
            {
                return;
            }

            // Apply the new Theme information
            var themeName = $"{_theme.Value}.{_themeAccent.Value}";
            var themeString = $"Themes/{themeName}";
            AddMahappsStyle(themeString);
            ThemeManager.ChangeTheme(Application.Current, themeName);
        }

        /// <summary>
        ///     Create a MapApps Uri for the supplied style
        /// </summary>
        /// <param name="style">e.g. Fonts or Controls</param>
        /// <returns></returns>
        public static Uri CreateMahappStyleUri(string style)
        {
            return new Uri($@"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}