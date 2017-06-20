//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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
using System.ComponentModel.Composition;
using Application.Demo.MetroAddon.Configurations;
using Caliburn.Micro;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Metro;

#endregion

namespace Application.Demo.MetroAddon.Services
{
    [Export(typeof(IUiService))]
    public class ConfigureDefaults : IUiService
    {
        [ImportingConstructor]
        public ConfigureDefaults(
            [Import(typeof(IWindowManager))]
            MetroWindowManager metroWindowManager,
            IMetroConfiguration metroConfiguration
            )
        {
            var demoUri = new Uri("pack://application:,,,/Application.Demo;component/DemoResourceDirectory.xaml", UriKind.RelativeOrAbsolute);
            metroWindowManager.AddResourceDictionary(demoUri);

            metroWindowManager.ChangeTheme(metroConfiguration.Theme);
            metroWindowManager.ChangeThemeAccent(metroConfiguration.ThemeAccent);

            // Override the ConfigView with a much nicer looking version
            ViewLocator.NameTransformer.AddRule(@"^Application\.Demo\.UseCases\.Configuration\.ViewModels\.ConfigViewModel$", "Application.Demo.MetroAddon.Views.ConfigView");
        }
    }
}