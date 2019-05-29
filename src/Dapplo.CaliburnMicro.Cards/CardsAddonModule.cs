//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using Dapplo.Addons;
using AdaptiveCards.Rendering;
using Autofac;
using System.IO;

namespace Dapplo.CaliburnMicro.Cards
{
    /// <inheritdoc />
    public class CardsAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            // Create a default AdaptiveHostConfig if no other is specified
            builder.Register(context =>
                {
                    var resourceProvider = context.Resolve<IResourceProvider>();
                    var resources = resourceProvider.GetCachedManifestResourceNames(ThisAssembly);

                    using (var stream = resourceProvider.LocateResourceAsStream(ThisAssembly, @"HostConfigs\Windows10Toast.json"))
                    using (var streamReader = new StreamReader(stream))
                    {
                        return AdaptiveHostConfig.FromJson(streamReader.ReadToEnd());
                    }
                })
                .IfNotRegistered(typeof(AdaptiveHostConfig))
                .As<AdaptiveHostConfig>()
                .SingleInstance();
        }
    }
}
