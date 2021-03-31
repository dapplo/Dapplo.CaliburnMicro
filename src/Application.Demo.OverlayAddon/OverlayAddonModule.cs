// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.OverlayAddon.ViewModels;
using Autofac;
using Autofac.Features.AttributeFilters;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Overlays;

namespace Application.Demo.OverlayAddon
{
    /// <inheritdoc />
    public class OverlayAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OverlayMenuItem>()
                .As<IMenuItem>()
                // Only when scanning the attributes are taken
                .WithMetadata("Context", "contextmenu")
                .SingleInstance();

            // All IOverlay for the demo
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IOverlay>()
                .As<IOverlay>()
                .SingleInstance();

            builder.RegisterType<DemoOverlayContainerViewModel>()
                .WithAttributeFiltering()
                .AsSelf()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
