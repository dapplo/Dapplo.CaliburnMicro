// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using Application.Demo.Languages;
using Application.Demo.UseCases.Toast.ViewModels;
using Autofac.Features.OwnedInstances;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.Toast
{
    /// <summary>
    ///     This provides the IMenuItem to show a ToastNotification
    /// It uses a ExportFactory for the ViewModel, the ViewModel should not be shared, and it will dispose everything as soon as the toast is deactivated.
    /// </summary>
    [Menu("contextmenu")]
    public sealed class ToastMenuItem : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private static readonly LogSource Log = new LogSource();

        public ToastMenuItem(
            IEventAggregator eventAggregator,
            Func<Owned<ToastExampleViewModel>> toastExampleViewModelFactory,
            IContextMenuTranslations contextMenuTranslations)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.Toast));

            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.MessageText
            };

            ClickAction = clickedItem =>
            {
                Log.Debug().WriteLine("Toast");
                ShowToast(eventAggregator, toastExampleViewModelFactory);
 
            };

            this.VisibleOnPermissions("Admin");
        }

        /// <summary>
        /// This takes care of creating the view model, publishing it, and disposing afterwards
        /// </summary>
        /// <param name="eventAggregator">IEventAggregator</param>
        /// <param name="toastExampleViewModelFactory">ExportFactory of type ToastExampleViewModel</param>
        private void ShowToast(IEventAggregator eventAggregator, Func<Owned<ToastExampleViewModel>> toastExampleViewModelFactory)
        {
            // Create the ViewModel "part"
            var message = toastExampleViewModelFactory();

            // Prepare to dispose the view model parts automatically if it's finished
            void DeactivateHandler(object sender, DeactivationEventArgs args)
            {
                message.Value.Deactivated -= DeactivateHandler;
                message.Dispose();
            }

            message.Value.Deactivated += DeactivateHandler;

            // Show the ViewModel as toast 
            eventAggregator.PublishOnCurrentThread(message.Value);
        }
    }
}