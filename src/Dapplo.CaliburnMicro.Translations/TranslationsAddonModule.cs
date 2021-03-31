// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Dapplo.Addons;
using Dapplo.Config.Language;

namespace Dapplo.CaliburnMicro.Translations
{
    /// <inheritdoc />
    public class TranslationsAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            // Create a default configuration, if none exists
            builder.Register(context => LanguageConfigBuilder.Create().BuildLanguageConfig())
                .IfNotRegistered(typeof(LanguageConfig))
                .As<LanguageConfig>()
                .SingleInstance();

            builder.RegisterType<LanguageContainer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<LanguageService>()
                .As<IService>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
