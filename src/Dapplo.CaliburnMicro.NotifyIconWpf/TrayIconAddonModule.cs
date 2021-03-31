// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Dapplo.Addons;

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <inheritdoc />
    public class TrayIconAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrayIconManager>()
                .As<IService>()
                .As<ITrayIconManager>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
