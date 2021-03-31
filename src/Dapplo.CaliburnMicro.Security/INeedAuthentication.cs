// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security
{
    /// <summary>
    ///     Base interface which an authenticated thing needs to implement
    /// </summary>
    public interface INeedAuthentication
    {
        /// <summary>
        ///     This defines the property which is managed by authentication
        /// </summary>
        AuthenticationTargetProperties AuthenticationTargetProperty { get; }

        /// <summary>
        ///     Describes the operation which is used when checking permissions.
        /// </summary>
        PermissionOperations PermissionOperation { get; }

        /// <summary>
        ///     Permission(s) for which the item is managed
        /// </summary>
        IEnumerable<string> Permissions { get; }
    }

    /// <summary>
    ///     Base interface which an authenticated thing needs to implement
    /// </summary>
    /// <typeparam name="TWhen"></typeparam>
    public interface INeedAuthentication<out TWhen> : INeedAuthentication
    {
        /// <summary>
        ///     What should be used when the permission is available
        /// </summary>
        TWhen WhenPermission { get; }

        /// <summary>
        ///     What should be used when the permission is not available
        /// </summary>
        TWhen WhenPermissionMissing { get; }
    }
}