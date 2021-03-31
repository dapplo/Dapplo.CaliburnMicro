// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Menu;

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <summary>
    ///     Marker interface which ViewModels for a TrayIcon must implement and export as.
    ///     This extends IViewAware, as this is needed to make it possible to find the ITrayIcon for a ViewModel.
    ///     Implementing this is easy, extend your ViewModel from ViewAware or Screen or another class in Caliburn.Micro.
    /// </summary>
    public interface ITrayIconViewModel : IViewAware
    {
        /// <summary>
        ///     Gives write access to the "underlying" TrayIcon.Icon via a FrameworkElement
        /// </summary>
        void SetIcon(FrameworkElement icon);

        /// <summary>
        ///     The ITrayIcon for the ViewModel
        /// </summary>
        ITrayIcon TrayIcon { get; }

        /// <summary>
        ///     These are the Context MenuItems for the system tray
        /// </summary>
        ObservableCollection<ITreeNode<IMenuItem>> TrayMenuItems { get; }

        /// <summary>
        ///     This is called when someone makes a a left-click on the icon
        /// </summary>
        void Click();

        /// <summary>
        ///     Hide the icon for this ViewModel
        /// </summary>
        void Hide();

        /// <summary>
        ///     Show the icon for this ViewModel
        /// </summary>
        void Show();
    }
}