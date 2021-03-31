// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Configurers;
using Dapplo.CaliburnMicro.Services;
using Dapplo.Config.Ini;

namespace Dapplo.CaliburnMicro
{
    /// <inheritdoc />
    public class CaliburnMicroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            // Create a default configuration, if none exists
            builder.Register(context => IniFileConfigBuilder.Create().BuildIniFileConfig())
                .IfNotRegistered(typeof(IniFileConfig))
                .As<IniFileConfig>()
                .SingleInstance();

            builder.RegisterType<ResourceManager>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<IniFileContainer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ConfigurationService>()
                .As<IService>()
                .SingleInstance();

            builder
                .Register(c => IniSection<IUiConfiguration>.Create())
                .As<IUiConfiguration>()
                .As<IIniSection>()
                .SingleInstance();

            builder.RegisterType<CultureViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<DpiAwareViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<IconViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<PlacementViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<DapploWindowManager>()
                .As<IWindowManager>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
