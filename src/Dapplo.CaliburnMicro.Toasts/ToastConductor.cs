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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// The toast conductor handles IToast message which can be used to display toasts.
    /// It's also possible to import the ToastConductor directly and use ActivateItem on it.
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    [ServiceOrder(CaliburnStartOrder.User)]
    public class ToastConductor: Conductor<IToast>.Collection.AllActive, IHandle<IToast>, IUiStartup
    {
        private readonly IEventAggregator _eventAggregator;
        private Notifier _notifier;

        /// <summary>
        /// Importing constructor to add the dependencies of the ToastConductor
        /// <param name="eventAggregator">IEventAggregator to subscribe to the IToast messages</param>
        /// <param name="notifier">optional Notifier configuration, if not supplied a default is used</param>
        /// </summary>
        public ToastConductor(
            IEventAggregator eventAggregator,
            Notifier notifier = null)
        {
            // Fail fast, if there is no IEventAggregator than something went wrong in the bootstrapper
            _notifier = notifier;
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            
        }

        /// <inheritdoc />
        public void Start()
        {
            ScreenExtensions.TryActivate(this);
        }

        /// <summary>
        /// Activate the Conductor for IToast
        /// Checks if a Notifier is available, else it will create one itself.
        /// Subscribe to the IEventAggregator, to handle IToast messages.
        /// </summary>
        protected override void OnActivate()
        {
            if (_notifier == null)
            {
                _notifier = new Notifier(configuration => {
                    configuration.DisplayOptions.TopMost = true;
                    configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(15));
                    //configuration.PositionProvider = new SystemTrayPositionProvider();
                    configuration.PositionProvider = new PrimaryScreenPositionProvider(
                        corner: Corner.BottomRight,
                        offsetX: 10,
                        offsetY: 10);
                    configuration.DisplayOptions.TopMost = true;
                    configuration.Dispatcher = Application.Current.Dispatcher;
                });
            }
            _eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        /// <summary>
        /// Remove the IToast subscription via the IEventAggregator and dispose the notifier.
        /// </summary>
        /// <param name="close"></param>
        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            _notifier.Dispose();
            _notifier = null;
            base.OnDeactivate(close);
        }

        /// <summary>
        /// Override to make sure that the View (NotificationDisplayPart) is located and bound with the ViewModel (IToast)
        /// </summary>
        /// <param name="item">IToast</param>
        public override void ActivateItem(IToast item)
        {
            var view = ViewLocator.LocateForModel(item, null, null);
            if (view == null)
            {
                throw new NotSupportedException($"Couldn't locate view for {GetType()}");
            }
            ViewModelBinder.Bind(item, view, null);

            base.ActivateItem(item);
        }

        /// <summary>
        /// Override to make sure the Unloaded event of the view is hooked, so we can deactivate the item.
        /// Also, as the INotifier is showing the items, and not the IWindowManager, we need to call the INotifier here.
        /// </summary>
        /// <param name="item">IToast</param>
        /// <param name="success">bool which tells us if the activation worked</param>
        protected override void OnActivationProcessed(IToast item, bool success)
        {
            // Make sure we hook the Unloaded event
            if (success)
            {
                item.DisplayPart.Unloaded += InternalUnload;

            }
            base.OnActivationProcessed(item, success);

            // Show the toast via the notifier
            if (success)
            {
                _notifier.Notify<NotificationDisplayPart>(() => item);
            }
        }

        /// <summary>
        /// This takes care of deactivating correctly, as Caliburn.Micro wouldn't know when it's no longer visible
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="eventArgs">RoutedEventArgs</param>
        private void InternalUnload(object sender, RoutedEventArgs eventArgs)
        {
            var displayPart = sender as NotificationDisplayPart;
            // Make sure we unhook the Unloaded event, to prevent memory leaks
            if (displayPart != null)
            {
                displayPart.Unloaded -= InternalUnload;
            }

            // Deactivate the item
            if (displayPart?.DataContext is IToast toast)
            {
                DeactivateItem(toast, true);
            }
        }

        /// <summary>
        /// Handles the IToast message, and will display the toast.
        /// </summary>
        /// <param name="message">IToast with the toast to show.</param>
        public void Handle(IToast message)
        {
            ActivateItem(message);
        }

    }
}
