// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using ToastNotifications.Events;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// This allows all keys to be handled by a notification
    /// </summary>
    public class AllowAllKeyInputEventHandler : IKeyboardEventHandler
    {
        /// <inheritdoc />
        public void Handle(KeyEventArgs eventArgs)
        {
        }
    }
}
