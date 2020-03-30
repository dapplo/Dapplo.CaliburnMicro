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