// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
