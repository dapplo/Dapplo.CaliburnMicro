// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Controls.Primitives;

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <summary>
    ///     This is the interface to the tray icon
    /// </summary>
    public interface ITrayIcon : IDisposable
    {
        /// <summary>
        ///     Close the actual balloon, if there is any
        /// </summary>
        void CloseBalloon();

        /// <summary>
        ///     Hide the icon
        /// </summary>
        void Hide();

        /// <summary>
        ///     Show the icon
        /// </summary>
        void Show();

        /// <summary>
        ///     Show a balloon with title, message without icon
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowBalloonTip(string title, string message);

        /// <summary>
        ///     Show a custom balloon (ViewModel first), using the specified animation.
        ///     After the timeout, the balloon is removed.
        /// </summary>
        /// <typeparam name="TViewModel">Type for the ViewModel to show</typeparam>
        /// <param name="animation">PopupAnimation</param>
        /// <param name="timeout">TimeSpan</param>
        void ShowBalloonTip<TViewModel>(PopupAnimation animation = PopupAnimation.Scroll, TimeSpan? timeout = null) where TViewModel : class;

        /// <summary>
        ///     Show a custom balloon (ViewModel first), using the specified animation.
        ///     After the timeout, the balloon is removed.
        /// </summary>
        /// <typeparam name="TViewModel">Type for the ViewModel to show</typeparam>
        /// <param name="viewModel">Instance of the ViewModel</param>
        /// <param name="animation">PopupAnimation</param>
        /// <param name="timeout">TimeSpan</param>
        void ShowBalloonTip<TViewModel>(TViewModel viewModel, PopupAnimation animation = PopupAnimation.Scroll, TimeSpan? timeout = null) where TViewModel : class;

        /// <summary>
        ///     Show a balloon with title, message and an Error-icon
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowErrorBalloonTip(string title, string message);

        /// <summary>
        ///     Show a balloon with title, message and an Info-icon
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowInfoBalloonTip(string title, string message);

        /// <summary>
        ///     Show a balloon with title, message and an Warning-icon
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        void ShowWarningBalloonTip(string title, string message);
    }
}