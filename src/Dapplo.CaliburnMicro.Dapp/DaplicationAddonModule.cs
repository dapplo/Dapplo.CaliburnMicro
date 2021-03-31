// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.Dapp.Services;

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <inheritdoc />
    public class DaplicationAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShellStartup>()
                .As<IService>()
                .SingleInstance();

            // Export the DapploWindowManager if no other IWindowManager is registered
            builder.RegisterType<DapploWindowManager>()
                .As<IWindowManager>()
                .IfNotRegistered(typeof(IWindowManager))
                .SingleInstance();

            // Export the EventAggregator if no other IEventAggregator is registered
            builder.RegisterType<EventAggregator>()
                .As<IEventAggregator>()
                .IfNotRegistered(typeof(IEventAggregator))
                .SingleInstance();

            base.Load(builder);
        }
    }
}
