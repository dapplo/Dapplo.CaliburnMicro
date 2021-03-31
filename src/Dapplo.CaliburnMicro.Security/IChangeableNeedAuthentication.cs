// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security
{
    /// <summary>
    ///     Base interface which an authenticated thing needs to implement
    /// </summary>
    public interface IChangeableNeedAuthentication : INeedAuthentication
    {
        /// <summary>
        ///     This defines the property which is managed by authentication
        /// </summary>
        new AuthenticationTargetProperties AuthenticationTargetProperty { get; set; }

        /// <summary>
        ///     Describes the operation which is used when checking permissions.
        /// </summary>
        new PermissionOperations PermissionOperation { get; set; }

        /// <summary>
        ///     Permission(s) for which the item is managed
        /// </summary>
        new IEnumerable<string> Permissions { get; set; }
    }

    /// <summary>
    ///     Base interface which an authenticated thing needs to implement
    /// </summary>
    /// <typeparam name="TWhen"></typeparam>
    public interface IChangeableNeedAuthentication<TWhen> : INeedAuthentication<TWhen>, IChangeableNeedAuthentication
    {
        /// <summary>
        ///     What should be used when the permission is available
        /// </summary>
        new TWhen WhenPermission { get; set; }

        /// <summary>
        ///     What should be used when the permission is not available
        /// </summary>
        new TWhen WhenPermissionMissing { get; set; }
    }
}