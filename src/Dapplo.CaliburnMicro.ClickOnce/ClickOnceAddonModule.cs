// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Autofac.Features.AttributeFilters;
using Dapplo.Addons;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <inheritdoc />
    public class ClickOnceAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClickOnceService>()
                .As<IService>()
                .As<IClickOnceService>()
                .As<IVersionProvider>()
                .WithAttributeFiltering()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
