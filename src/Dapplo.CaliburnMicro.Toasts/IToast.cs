// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;
using ToastNotifications.Core;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// This is the base interface for toasts
    /// </summary>
    public interface IToast : IScreen, INotification
    {
        /// <summary>
        /// This contains the MessageOptions for the ToastNotification 
        /// </summary>
        MessageOptions Options { get; set; }
    }
}
