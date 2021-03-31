// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ControlzEx.Theming;

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
            "Controls", "Fonts", "Controls.TabControl"
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
            ThemeManager.Current.DetectTheme(Application.Current);
            Theme newTheme = ThemeManager.Current.GetTheme($"{theme}.{themeColor}");
            ChangeTheme(newTheme);
        }

        /// <summary>
        ///     Change the current theme
        /// </summary>
        /// <param name="themeName">string</param>
        public void ChangeTheme(string themeName)
        {
            var newTheme = string.IsNullOrEmpty(themeName) ? ThemeManager.Current.Themes.First() : ThemeManager.Current.GetTheme(themeName);
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
            ThemeManager.Current.ChangeTheme(Application.Current, theme);
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
        public static IEnumerable<string> AvailableThemes => ThemeManager.Current.Themes.Select(theme => theme.BaseColorScheme).Distinct();

        /// <summary>
        /// The available theme colors
        /// </summary>
        public static IEnumerable<string> AvailableThemeColors => ThemeManager.Current.Themes.Select(theme => theme.ColorScheme).Distinct();
    }
}