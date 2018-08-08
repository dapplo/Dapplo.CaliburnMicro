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

using Application.Demo.Services;
using Application.Demo.UseCases.Configuration.ViewModels;
using Application.Demo.UseCases.ContextMenu.ViewModels;
using Application.Demo.UseCases.Menu.ViewModels;
using Application.Demo.UseCases.Toast.ViewModels;
using Application.Demo.UseCases.Wizard.ViewModels;
using Autofac;
using Autofac.Features.AttributeFilters;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.NotifyIconWpf;
using Dapplo.CaliburnMicro.Security;
using Dapplo.CaliburnMicro.Toasts;
using Dapplo.CaliburnMicro.Wizard;
using ToastNotifications.Events;

namespace Application.Demo
{
    /// <inheritdoc />
    public class DemoAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DemoTrayIconViewModel>()
                .As<ITrayIconViewModel>()
                .WithAttributeFiltering()
                .SingleInstance();
            builder.RegisterType<AuthenticationProvider>()
                .As<IAuthenticationProvider>()
                .SingleInstance();
            builder.RegisterType<NotifyOfStartupReady>()
                .As<IService>()
                .SingleInstance();
            builder.RegisterType<SimpleVersionProvider>()
                .As<IVersionProvider>()
                .SingleInstance();

            // All IMenuItem with the context they belong to
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IMenuItem>()
                .As<IMenuItem>()
                .SingleInstance();

            // All config screens
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IConfigScreen>()
                .As<IConfigScreen>()
                .AsSelf()
                .SingleInstance();

            // All wizard screens
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IWizardScreen>()
                .As<IWizardScreen>()
                .SingleInstance();

            builder.RegisterType<ConfigViewModel>()
                .AsSelf()
                .SingleInstance();

            // View models
            builder.RegisterType<WindowWithMenuViewModel>()
                .AsSelf()
                .WithAttributeFiltering()
                .SingleInstance();

            builder.RegisterType<StartupReadyToastViewModel>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<WizardExampleViewModel>()
                .AsSelf()
                .SingleInstance();
            
            // Not a single instance
            builder.RegisterType<ToastExampleViewModel>()
                .AsSelf();

            builder.RegisterType<AllowAllKeyInputEventHandler>()
                .As<IKeyboardEventHandler>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
