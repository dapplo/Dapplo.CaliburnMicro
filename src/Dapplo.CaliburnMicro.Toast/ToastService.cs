//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.Addons;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using System.Windows;

namespace Dapplo.CaliburnMicro.Toast
{
    /// <summary>
    /// This initializes the toast functionality
    /// </summary>
    [StartupAction]
    public class ToastService : IStartupAction, IHandle<INotification>
    {
        private readonly Notifier _notifier;
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public ToastService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _notifier = new Notifier(configuration => {
                configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(5), MaximumNotificationCount.FromCount(15));
                configuration.PositionProvider = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 10);
                configuration.Dispatcher = Application.Current.Dispatcher;
            });
        }

        /// <summary>
        /// Register this as a INotification handler
        /// </summary>
        public void Start()
        {
            _eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Handle the message, pass it through to the notifier
        /// </summary>
        /// <param name="viewModel"></param>
        public void Handle(INotification viewModel)
        {
            _notifier.Notify<NotificationDisplayPart>(() => viewModel);
        }
    }
}
