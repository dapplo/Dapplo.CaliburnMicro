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
using ToastNotifications.Position;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// The toast conductor is exported, and can be used to display toasts
    /// </summary>
    [Export]
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public class ToastConductor: Conductor<IToast>.Collection.AllActive
    {
        private readonly Notifier _notifier = new Notifier(configuration => {
            configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(10), MaximumNotificationCount.FromCount(15));
            configuration.PositionProvider = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 10);
            configuration.DisplayOptions.TopMost = true;
            configuration.Dispatcher = Application.Current.Dispatcher;
        });

        /// <summary>
        /// Make sure this is always activated
        /// </summary>
        public ToastConductor()
        {
            ScreenExtensions.TryActivate(this);
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
    }
}
