// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.MetroAddon.Services;
using Application.Demo.MetroAddon.ViewModels;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;
using Application.Demo.MetroAddon.Configurations;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.Config.Ini;
using Dapplo.Config.Language;
using Dapplo.CaliburnMicro.Metro.Configuration;
using Caliburn.Micro;

namespace Application.Demo.MetroAddon
{
    /// <inheritdoc />
    public class MetroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => Language<ICredentialsTranslations>.Create())
                .As<ICredentialsTranslations>()
                .As<ILanguage>()
                .SingleInstance();

            builder
                .Register(c =>
                {
                    var metroConfiguration = IniSection<IMetroConfiguration>.Create();

                    // add specific code
                    var metroThemeManager = c.Resolve<MetroThemeManager>();

                    metroConfiguration.RegisterAfterLoad(iniSection =>
                    {
                        if (!(iniSection is IMetroUiConfiguration metroConfig))
                        {
                            return;
                        }
                        // following must be done on the UI thread
                        Execute.OnUIThread(() =>
                        {
                            metroThemeManager.ChangeTheme(metroConfig.Theme, metroConfig.ThemeColor);
                        });
                    });
                    return metroConfiguration;
                })
                .As<IMetroUiConfiguration>()
                .As<IMetroConfiguration>()
                .As<IIniSection>()
                .SingleInstance();

            builder
                .Register(c => Language<IUiTranslations>.Create())
                .As<IUiTranslations>()
                .As<ILanguage>()
                .SingleInstance();
            
            builder.RegisterType<ConfigureDefaults>().As<IService>().SingleInstance();
            builder.RegisterType<ThemeConfigViewModel>().As<IConfigScreen>().SingleInstance();

            base.Load(builder);
        }
    }
}
