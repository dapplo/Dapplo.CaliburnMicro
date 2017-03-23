#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;

#endregion

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
		public IEnumerable<string> Permissions => new ReadOnlyCollection<string>( _permissions);

		/// <summary>
		/// Add a permission and inform via INotifyPropertyChanged events of changes
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
		/// Remove a permission and inform via INotifyPropertyChanged events of changes
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