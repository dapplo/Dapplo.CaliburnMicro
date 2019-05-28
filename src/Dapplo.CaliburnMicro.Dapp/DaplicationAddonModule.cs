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
