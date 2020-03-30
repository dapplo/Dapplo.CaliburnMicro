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

using System;
using Autofac;
using Dapplo.Addons;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Events;
using ToastNotifications.Lifetime;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <inheritdoc />
    public class ToastAddonModule : AddonModule
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ToastConductor>()
                .As<IService>()
                .AsSelf()
                .SingleInstance();

            builder.Register(context => new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(15)))
                .IfNotRegistered(typeof(INotificationsLifetimeSupervisor))
                .As<INotificationsLifetimeSupervisor>()
                .SingleInstance();

            builder.Register(context => new SystemTrayPositionProvider())
                .IfNotRegistered(typeof(IPositionProvider))
                .As<IPositionProvider>()
                .SingleInstance();

            builder.Register(context => new DisplayOptions {TopMost = true, Width = 250})
                .IfNotRegistered(typeof(DisplayOptions))
                .As<DisplayOptions>()
                .SingleInstance();

            // Notifier
            builder.Register(context =>
            {
                var displayOptions = context.Resolve<DisplayOptions>();
                var notificationsLifetimeSupervisor = context.Resolve<INotificationsLifetimeSupervisor>();
                var positionProvider = context.Resolve<IPositionProvider>();
                var keyboardEventHandler = context.ResolveOptional<IKeyboardEventHandler>();
                return new Notifier(configuration =>
                {
                    configuration.DisplayOptions.Width = displayOptions.Width;
                    configuration.DisplayOptions.TopMost = displayOptions.TopMost;
                    configuration.LifetimeSupervisor = notificationsLifetimeSupervisor;
                    configuration.PositionProvider = positionProvider;
                    if (keyboardEventHandler != null)
                    {
                        configuration.KeyboardEventHandler = keyboardEventHandler;
                    }
                    configuration.Dispatcher = System.Windows.Application.Current.Dispatcher;
                });})
                .IfNotRegistered(typeof(Notifier))
                .As<Notifier>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
