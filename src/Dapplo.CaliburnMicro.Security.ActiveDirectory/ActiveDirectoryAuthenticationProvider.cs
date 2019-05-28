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

using System;
using System.Collections.Generic;
using System.Linq;
using Dapplo.ActiveDirectory;
using Dapplo.ActiveDirectory.Entities;
using Dapplo.ActiveDirectory.Enums;
using Dapplo.CaliburnMicro.Security.ActiveDirectory.Entities;

namespace Dapplo.CaliburnMicro.Security.ActiveDirectory
{
    /// <summary>
    ///     IAuthenticationProvider which uses the Groups from the Active Directory for the current user
    /// </summary>
    /// <inheritdoc />
    public class ActiveDirectoryAuthenticationProvider : IAuthenticationProvider
    {
        private IList<string> _permissions;

        /// <summary>
        ///     Create the ActiveDirectoryAuthenticationProvider, and get the user details
        /// </summary>
        public ActiveDirectoryAuthenticationProvider()
        {
            Load();
        }

        /// <summary>
        /// The current user
        /// </summary>
        public IUser CurrentUser { get; private set; }

        /// <summary>
        /// (re)Load the actual AD groups
        /// </summary>
        public void Load()
        {
            // Read the current user from the AD
            CurrentUser = Query.ForUser(Environment.UserName).Execute<UserImpl>().FirstOrDefault();
            // From the groups, get the CN, place the result into the permissions
            _permissions = CurrentUser?.Groups.SelectMany(dn => dn.Where(v => v.Key == DistinguishedNameAttributes.CommonName).Select(v => v.Value.ToLowerInvariant())).ToList() ?? new List<string>();
        }

        /// <inheritdoc />
        public bool HasPermissions(IEnumerable<string> neededPermissions, PermissionOperations permissionOperation = PermissionOperations.Or)
        {
            // Argument check
            if (neededPermissions == null)
            {
                throw new ArgumentNullException(nameof(neededPermissions));
            }

            // Create a clean list of permissions needed
            var permissionsToCompare = neededPermissions.Where(s => !string.IsNullOrWhiteSpace(s)).Select(permission => permission.Trim().ToLowerInvariant()).ToList();

            if (permissionOperation == PermissionOperations.Or)
            {
                return permissionsToCompare.Any(permission => _permissions.Contains(permission));
            }
            return permissionsToCompare.All(permission => _permissions.Contains(permission));
        }
    }
}
