//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro;

namespace Dapplo.CaliburnMicro.Metro
{
    /// <summary>
    /// This 
    /// </summary>
    public sealed class MetroThemeManager
    {
        private readonly ResourceManager _resourceManager;

        private static readonly string[] Styles =
        {
            "Controls", "Fonts", "Controls.AnimatedSingleRowTabControl"
        };

        private string _theme;
        private string _themeColor;

        /// <summary>
        /// Default constructor taking care of initialization
        /// </summary>
        public MetroThemeManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            int order = 0;
            _resourceManager.AddResourceDictionaries(Styles.Select(style => (CreateMahappStyleUri(style), -100 + order++) ));
        }

        /// <summary>
        ///     Add a ResourceDictionary for the specified MahApps style
        ///     The Uri to the source is created by CreateMahappStyleUri
        /// </summary>
        /// <param name="style">
        ///     Style name, this is actually what is added behind
        ///     pack://application:,,,/MahApps.Metro;component/Styles/ (and .xaml is added)
        /// </param>
        /// <param name="order">int order for the ResourceManager</param>
        public void AddMahappsStyle(string style, int order = 0)
        {
            var packUri = CreateMahappStyleUri(style);
            _resourceManager.AddResourceDictionary(packUri, order);
        }

        /// <summary>
        ///     Remove all ResourceDictionaries for the specified MahApps style
        ///     The Uri to the source is created by CreateMahappStyleUri
        /// </summary>
        /// <param name="style">string</param>
        public void RemoveMahappsStyle(string style)
        {
            var mahappsStyleUri = CreateMahappStyleUri(style);
            _resourceManager.DeleteResourceDictionary(mahappsStyleUri);
        }

        /// <summary>
        ///     Change the current theme
        /// </summary>
        /// <param name="theme">string</param>
        /// <param name="themeColor">string</param>
        public void ChangeTheme(string theme, string themeColor)
        {
            if (string.IsNullOrEmpty(theme))
            {
                theme = _theme;
            }
            if (string.IsNullOrEmpty(themeColor))
            {
                themeColor = _themeColor;
            }
            var currentTheme = ThemeManager.DetectTheme(Application.Current);
            Theme newTheme = ThemeManager.GetTheme($"{theme}.{themeColor}");
            ChangeTheme(newTheme);
        }

        /// <summary>
        ///     Change the current theme
        /// </summary>
        /// <param name="themeName">string</param>
        public void ChangeTheme(string themeName)
        {
            var newTheme = string.IsNullOrEmpty(themeName) ? ThemeManager.Themes.First() : ThemeManager.GetTheme(themeName);
            ChangeTheme(newTheme);
        }

        /// <summary>
        ///     Change the current theme
        /// </summary>
        /// <param name="theme">Theme</param>
        public void ChangeTheme(Theme theme)
        {
            _theme = theme.BaseColorScheme;
            _themeColor = theme.ColorScheme;
            foreach (var sourceUri in _resourceManager.Resources.ToList().Select(resource => resource.Source).Where(source => source.AbsoluteUri.Contains("theme")))
            {
                _resourceManager.DeleteResourceDictionary(sourceUri);
            }
            _resourceManager.AddResourceDictionary(theme.Resources.Source, 0, false);
            ThemeManager.ChangeTheme(Application.Current, theme);
        }

        /// <summary>
        ///     Create a MapApps Uri for the supplied style
        /// </summary>
        /// <param name="style">e.g. Fonts or Controls</param>
        /// <returns>Uri</returns>
        public static Uri CreateMahappStyleUri(string style)
        {
            return new Uri($@"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// The available themes
        /// </summary>
        public static IEnumerable<string> AvailableThemes => ThemeManager.Themes.Select(theme => theme.BaseColorScheme).Distinct();

        /// <summary>
        /// The available theme colors
        /// </summary>
        public static IEnumerable<string> AvailableThemeColors => ThemeManager.Themes.Select(theme => theme.ColorScheme).Distinct();
    }
}