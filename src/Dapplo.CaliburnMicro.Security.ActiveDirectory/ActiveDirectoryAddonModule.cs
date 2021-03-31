// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Dapplo.Addons;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory
{
    /// <inheritdoc />
    public class ActiveDirectoryAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActiveDirectoryAuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .AsSelf()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
