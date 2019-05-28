//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using Application.Demo.MetroAddon.Services;
using Application.Demo.MetroAddon.ViewModels;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;
using Application.Demo.MetroAddon.Configurations.Impl;
using Application.Demo.MetroAddon.Configurations;
using Dapplo.Config.Ini;
using Dapplo.Config.Language;
using Dapplo.CaliburnMicro.Metro.Configuration;

namespace Application.Demo.MetroAddon
{
    /// <inheritdoc />
    public class MetroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<CredentialsTranslationsImpl>()
                .As<ICredentialsTranslations>()
                .As<ILanguage>()
                .SingleInstance();

            builder
                .RegisterType<MetroConfigurationImpl>()
                .As<IMetroConfiguration>()
                .As<IIniSection>()
                .As<IMetroUiConfiguration>()
                .SingleInstance();

            builder
                .RegisterType<UiTranslationsImpl>()
                .As<IUiTranslations>()
                .As<ILanguage>()
                .SingleInstance();
            
            builder.RegisterType<ConfigureDefaults>().As<IService>().SingleInstance();
            builder.RegisterType<ThemeConfigViewModel>().As<IConfigScreen>().SingleInstance();

            base.Load(builder);
        }
    }
}
