// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

                    using var stream = resourceProvider.LocateResourceAsStream(ThisAssembly, @"HostConfigs\Windows10Toast.json");
                    using var streamReader = new StreamReader(stream);
                    return AdaptiveHostConfig.FromJson(streamReader.ReadToEnd());
                })
                .IfNotRegistered(typeof(AdaptiveHostConfig))
                .As<AdaptiveHostConfig>()
                .SingleInstance();
        }
    }
}
