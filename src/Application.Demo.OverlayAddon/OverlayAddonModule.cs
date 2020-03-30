//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
