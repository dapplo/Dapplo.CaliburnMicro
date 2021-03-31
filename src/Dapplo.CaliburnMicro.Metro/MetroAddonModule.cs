// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Metro.Configuration;
using Dapplo.Config;

namespace Dapplo.CaliburnMicro.Metro
{
    /// <inheritdoc />
    public class MetroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MetroWindowManager>()
                .AsSelf()
                .As<IWindowManager>()
                .SingleInstance();

            builder.RegisterType<MetroThemeManager>()
                .AsSelf()
               .SingleInstance();
            
            builder
                .Register(c => DictionaryConfiguration<IMetroUiConfiguration>.Create())
                .IfNotRegistered(typeof(IMetroUiConfiguration))
                .As<IMetroUiConfiguration>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
