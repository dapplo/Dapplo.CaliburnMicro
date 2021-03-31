// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dapplo.CaliburnMicro.Security
{
    /// <summary>
    ///     Interface which all authentication providers must implement
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        ///     Returns if the current user has a certain permission
        /// </summary>
        /// <param name="neededPermissions">string with the name of the permission</param>
        /// <param name="permissionOperation">PermissionOperations, default Or</param>
        /// <returns>true if the current user has the specified permission</returns>
        bool HasPermissions(IEnumerable<string> neededPermissions, PermissionOperations permissionOperation = PermissionOperations.Or);
    }
}