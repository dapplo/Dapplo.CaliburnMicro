//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security.Behaviors;

#endregion

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