// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Security
{
    /// <summary>
    ///     A simple implementation of the IAuthenticationProvider, can be used as a base for your own implementation
    /// </summary>
    public class SimpleAuthenticationProvider : PropertyChangedBase, IAuthenticationProvider
    {
        private readonly IList<string> _permissions = new List<string>();

        /// <summary>
        ///     List of available permissions
        /// </summary>
        public IEnumerable<string> Permissions => new ReadOnlyCollection<string>(_permissions);

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

        /// <summary>
        ///     Add a permission and inform via INotifyPropertyChanged events of changes
        /// </summary>
        /// <param name="permission">string</param>
        public void AddPermission(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
            {
                throw new ArgumentNullException(nameof(permission));
            }
            var newPermission = permission.Trim().ToLowerInvariant();
            _permissions.Add(newPermission);
            NotifyOfPropertyChange(nameof(HasPermissions));
        }

        /// <summary>
        ///     Remove a permission and inform via INotifyPropertyChanged events of changes
        /// </summary>
        /// <param name="permission">string</param>
        public void RemovePermission(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission))
            {
                throw new ArgumentNullException(nameof(permission));
            }
            var removingPermission = permission.Trim().ToLowerInvariant();
            _permissions.Remove(removingPermission);
            NotifyOfPropertyChange(nameof(HasPermissions));
        }
    }
}