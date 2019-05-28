//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

#region using

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;

#endregion

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <summary>
    ///     This is the TrayIcon, which makes the TaskbarIcon usable from Caliburn
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public class TrayIcon : TaskbarIcon, ITrayIcon
    {
        /// <summary>
        ///     Show the icon
        /// </summary>
        public void Show()
        {
            Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Hide the icon
        /// </summary>
        public void Hide()
        {
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Show a custom balloon (ViewModel first), using the specified animation.
        ///     After the timeout, the balloon is removed.
        /// </summary>
        /// <typeparam name="TViewModel">Type for the ViewModel to show</typeparam>
        /// <param name="animation">PopupAnimation</param>
        /// <param name="timeout">TimeSpan</param>
        public void ShowBalloonTip<TViewModel>(PopupAnimation animation = PopupAnimation.Scroll, TimeSpan? timeout = default) where TViewModel : class
        {
            var viewModel = IoC.Get<TViewModel>();
            ShowBalloonTip(viewModel, animation, timeout);
        }

        /// <summary>
        ///     Show a custom balloon (ViewModel first), using the specified animation.
        ///     After the timeout, the balloon is removed.
        /// </summary>
        /// <typeparam name="TViewModel">Type for the ViewModel to show</typeparam>
        /// <param name="viewModel">instance of your ViewModel</param>
        /// <param name="animation">PopupAnimation</param>
        /// <param name="timeout">TimeSpan</param>
        public void ShowBalloonTip<TViewModel>(TViewModel viewModel, PopupAnimation animation = PopupAnimation.Scroll, TimeSpan? timeout = default) where TViewModel : class
        {
            var view = ViewLocator.LocateForModel(viewModel, null, null);
            ViewModelBinder.Bind(viewModel, view, null);

            ShowCustomBalloon(view, animation, timeout.HasValue ? (int) timeout.Value.TotalMilliseconds : (int) TimeSpan.FromSeconds(4).TotalMilliseconds);
        }

        /// <summary>
        ///     Show a balloon with title, message and the default info icon
        /// </summary>
        /// <param name="title">Title for the standard balloon</param>
        /// <param name="message">Message for the standard balloon</param>
        public void ShowInfoBalloonTip(string title, string message)
        {
            ShowBalloonTip(title, message, BalloonIcon.Info);
        }

        /// <summary>
        ///     Show a balloon with title, message and the default error icon
        /// </summary>
        /// <param name="title">Title for the standard balloon</param>
        /// <param name="message">Message for the standard balloon</param>
        public void ShowErrorBalloonTip(string title, string message)
        {
            ShowBalloonTip(title, message, BalloonIcon.Error);
        }

        /// <summary>
        ///     Show a balloon with title, message and the default warning icon
        /// </summary>
        /// <param name="title">Title for the standard balloon</param>
        /// <param name="message">Message for the standard balloon</param>
        public void ShowWarningBalloonTip(string title, string message)
        {
            ShowBalloonTip(title, message, BalloonIcon.Warning);
        }

        /// <summary>
        ///     Show a balloon with title, message and no icon
        /// </summary>
        /// <param name="title">Title for the standard balloon</param>
        /// <param name="message">Message for the standard balloon</param>
        public void ShowBalloonTip(string title, string message)
        {
            ShowBalloonTip(title, message, BalloonIcon.None);
        }
    }
}