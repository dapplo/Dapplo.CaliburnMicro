// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Security
{
    /// <summary>
    ///     Describes the operation which is used when checking permissions.
    /// </summary>
    public enum PermissionOperations
    {
        /// <summary>
        ///     Defines that the permission protected item needs one of the specified permissions.
        ///     This should be the default
        /// </summary>
        Or,

        /// <summary>
        ///     Defines that the permission protected item needs all of the specified permissions.
        /// </summary>
        And
    }
}