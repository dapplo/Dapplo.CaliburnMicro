// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;

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