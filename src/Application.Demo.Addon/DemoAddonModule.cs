// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Addon.Languages;
using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.Config.Language;

namespace Application.Demo.Addon
{
    /// <summary>
    /// Setup the demo addon
    /// </summary>
    public class DemoAddonModule : AddonModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => Language<IAddonTranslations>.Create())
                .As<IAddonTranslations>()
                .As<ILanguage>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IConfigScreen>()
                .As<IConfigScreen>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
