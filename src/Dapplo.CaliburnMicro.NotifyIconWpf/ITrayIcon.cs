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

#region using

using System;
using System.Windows.Controls.Primitives;

#endregion

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