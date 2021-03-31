// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Diagnostics.ViewModels;

namespace Dapplo.CaliburnMicro.Diagnostics
{
    /// <inheritdoc />
    public class DiagnosticsAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ErrorViewModel>()
                .AsSelf()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
