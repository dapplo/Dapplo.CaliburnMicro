// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// Information on ClickOnce status
    /// </summary>
    public interface IClickOnceService : IVersionProvider
    {
        /// <summary>
        /// Is this a ClickOnce application?
        /// </summary>
        bool IsClickOnce { get; }

        /// <summary>
        /// The time the last check was made on
        /// </summary>
        DateTimeOffset LastCheckedOn { get; }

        /// <summary>
        /// Trigger an application restart
        /// </summary>
        void Restart();
    }
}
