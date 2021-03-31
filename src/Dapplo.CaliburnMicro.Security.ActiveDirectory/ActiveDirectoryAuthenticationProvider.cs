// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dapplo.ActiveDirectory;
using Dapplo.ActiveDirectory.Entities;
using Dapplo.ActiveDirectory.Enums;

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
            CurrentUser = Query.ForUser(Environment.UserName).Execute<IUser>().FirstOrDefault();
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
