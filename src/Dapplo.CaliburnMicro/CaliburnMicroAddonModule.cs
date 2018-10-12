//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Configuration.Impl;
using Dapplo.CaliburnMicro.Configurers;

namespace Dapplo.CaliburnMicro
{
    /// <inheritdoc />
    public class CaliburnMicroAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UiConfigurationImpl>()
                .As<IUiConfiguration>()
                .SingleInstance();

            builder.RegisterType<CultureViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<DpiAwareViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<IconViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<PlacementViewConfigurer>()
                .As<IConfigureDialogViews>()
                .As<IConfigureWindowViews>()
                .SingleInstance();
            builder.RegisterType<DapploWindowManager>()
                .As<IWindowManager>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
