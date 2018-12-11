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

using System.Diagnostics.CodeAnalysis;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.CaliburnMicro.Metro.Configuration;
using Dapplo.Config.Ini;
using MahApps.Metro.Controls;

namespace Application.Demo.MetroAddon.Configurations.Impl
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class MetroConfigurationImpl : IniSectionBase<IMetroConfiguration>, IMetroConfiguration
    {
        private readonly MetroThemeManager _metroThemeManager;

        public MetroConfigurationImpl(MetroThemeManager metroThemeManager)
        {
            _metroThemeManager = metroThemeManager;
        }
        public HotKey HotKey { get; set; }

        public Themes Theme { get; set; }

        public ThemeAccents ThemeAccent { get; set; }

        #region Overrides of IniSectionBase<IMetroConfiguration>

        public override void AfterLoad()
        {
            // Correct wrong entries to their defaults
            if (ThemeAccent == ThemeAccents.Default)
            {
                ThemeAccent = ThemeAccents.Blue;
            }
            if (Theme == Themes.Default)
            {
                Theme = Themes.Light;
            }

            _metroThemeManager.ChangeTheme(Theme, ThemeAccent);

            base.AfterLoad();
        }

        #endregion
    }
}
