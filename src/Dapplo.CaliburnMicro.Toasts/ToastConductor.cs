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
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Caliburn.Micro;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// The toast conductor handles IToast message which can be used to display toasts.
    /// It's also possible to import the ToastConductor directly and use ActivateItem on it.
    /// </summary>
    [Export(typeof(IUiService))]
    [Export]
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public class ToastConductor: Conductor<IToast>.Collection.AllActive, IHandle<IToast>, IUiService
    {
        private readonly IEventAggregator _eventAggregator;
        private Notifier _notifier;

        /// <summary>
        /// Importing constructor to add the dependencies of the ToastConductor
        /// <param name="eventAggregator">IEventAggregator to subscribe to the IToast messages</param>
        /// <param name="notifier">optional Notifier configuration, if not supplied a default is used</param>
        /// </summary>
        [ImportingConstructor]
        public ToastConductor(
            IEventAggregator eventAggregator,
            [Import(AllowDefault = true)]
            Notifier notifier
            )
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }
            _notifier = notifier;
            _eventAggregator = eventAggregator;
            ScreenExtensions.TryActivate(this);
        }

        /// <inheritdoc />
        protected override void OnActivate()
        {
            if (_notifier == null)
            {
               
                _notifier = new Notifier(configuration => {
                    configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(15));
                    configuration.PositionProvider = new SystemTrayPositionProvider();
                    configuration.DisplayOptions.TopMost = true;
                    configuration.Dispatcher = Application.Current.Dispatcher;
                });
            }
            _eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        /// <inheritdoc />
        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            _notifier.Dispose();
            base.OnDeactivate(close);

        }

        /// <inheritdoc />
        protected override void OnActivationProcessed(IToast item, bool success)
        {
            // Make sure we hook the Unloaded event
            item.DisplayPart.Unloaded += InternalUnload;
            base.OnActivationProcessed(item, success);

            // Show the toast
            _notifier.Notify<NotificationDisplayPart>(() => item);
        }

        /// <summary>
        /// This takes care of deactivating correctly, as Caliburn.Micro wouldn't know when it's no longer visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void InternalUnload(object sender, RoutedEventArgs eventArgs)
        {
            var displayPart = sender as NotificationDisplayPart;
            // Make sure we unhook the Unloaded event, to prevent memory leaks
            if (displayPart != null)
            {
                displayPart.Unloaded -= InternalUnload;
            }

            var toast = displayPart?.DataContext as IToast;
            // Deactivate
            if (toast != null)
            {
                DeactivateItem(toast, true);
            }
        }

        /// <summary>Handles the IToast message, and will display the toast.</summary>
        /// <param name="message">IToast with the toast to show.</param>
        public void Handle(IToast message)
        {
            ActivateItem(message);
        }
    }
}
