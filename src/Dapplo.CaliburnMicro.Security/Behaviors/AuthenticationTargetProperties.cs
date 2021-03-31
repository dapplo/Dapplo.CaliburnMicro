// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Security.Behaviors
{
    /// <summary>
    ///     This defines the property which is managed by authentication
    /// </summary>
    public enum AuthenticationTargetProperties
    {
        /// <summary>
        ///     No influence
        /// </summary>
        None,

        /// <summary>
        ///     Changes the IsEnabled
        /// </summary>
        IsEnabled,

        /// <summary>
        ///     Changes the visibility
        /// </summary>
        Visibility
    }
}