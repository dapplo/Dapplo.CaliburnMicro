﻿// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;
using ToastNotifications;
using ToastNotifications.Core;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// The toast conductor handles IToast message which can be used to display toasts.
    /// It's also possible to import the ToastConductor directly and use ActivateItem on it.
    /// By default the setup is in such a way, that the toast arrive near the system tray
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    [Service(nameof(ToastConductor), nameof(CaliburnServices.CaliburnMicroBootstrapper), TaskSchedulerName = "ui")]
    public class ToastConductor: Conductor<IToast>.Collection.AllActive, IHandle<IToast>, IStartup
    {
        private readonly IEventAggregator _eventAggregator;
        private Notifier _notifier;

        /// <summary>
        /// Importing constructor to add the dependencies of the ToastConductor
        /// <param name="eventAggregator">IEventAggregator to subscribe to the IToast messages</param>
        /// <param name="notifier">Notifier configuration</param>
        /// </summary>
        public ToastConductor(
            IEventAggregator eventAggregator,
            Notifier notifier)
        {
            // Fail fast, if there is no Notifier than something was configured wrong
            _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            // Fail fast, if there is no IEventAggregator than something went wrong in the bootstrapper
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        /// <inheritdoc />
        public void Startup()
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
