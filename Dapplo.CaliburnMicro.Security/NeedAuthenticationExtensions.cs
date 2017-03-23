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

using System.Collections.Generic;
using System.Windows;
using System.Linq;
using Dapplo.CaliburnMicro.Security.Behaviors;

#endregion

namespace Dapplo.CaliburnMicro.Security
{
	/// <summary>
	///     Helper extensions to set the default values for INeedAuthentication
	/// </summary>
	public static class NeedAuthenticationExtensions
	{
		/// <summary>
		/// Create an IEnumerable of string from a string
		/// </summary>
		/// <param name="permissions">string with multiple permissions</param>
		/// <param name="delimiter">default ,</param>
		/// <returns>IEnumerable of string</returns>
		public static IEnumerable<string> ToPermissions(this string permissions, char delimiter = ',')
		{
			var permissionsList = permissions?.Split(delimiter)
				.Where(permission => !string.IsNullOrWhiteSpace(permission))
				.Select(permission => permission.Trim().ToLowerInvariant())
				.Where(permission => !string.IsNullOrEmpty(permission)).ToList();

			return permissionsList ?? Enumerable.Empty<string>();
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be visible when permission is available, and hidden when not
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="permissions">The permission(s)</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermissions(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, string permissions, Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.VisibleOnPermissions(permissions.ToPermissions(), hidden);
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be visible when permission is available, and hidden when not
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="permissions">The permission(s)</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermissions(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, IEnumerable<string> permissions = null,
			Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility;
			visibilityAuthentication.WhenPermission = Visibility.Visible;
			visibilityAuthentication.WhenPermissionMissing = hidden;

			if (permissions != null)
			{
				visibilityAuthentication.Permissions = permissions;
			}
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be visible when permission is missing, and hidden when available
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="permissions">The permission(s)</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermissionsMissing(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, string permissions, Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.VisibleOnPermissionsMissing(permissions.ToPermissions(), hidden);
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be visible when permission is missing, and hidden when available
		/// </summary>
		/// <param name="visibilityAuthentication">INeedAuthentication of type Visibility</param>
		/// <param name="permissions">The permission(s)</param>
		/// <param name="hidden">Visibility for hidden</param>
		public static void VisibleOnPermissionsMissing(this IChangeableNeedAuthentication<Visibility> visibilityAuthentication, IEnumerable<string> permissions = null,
			Visibility hidden = Visibility.Collapsed)
		{
			visibilityAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.Visibility;
			visibilityAuthentication.WhenPermission = hidden;
			visibilityAuthentication.WhenPermissionMissing = Visibility.Visible;

			if (permissions != null)
			{
				visibilityAuthentication.Permissions = permissions;
			}
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be enabled when permission is available, and disabled when not
		/// </summary>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		/// <param name="permissions">The permission(s)</param>
		public static void EnabledOnPermissions(this IChangeableNeedAuthentication<bool> enabledAuthentication, string permissions)
		{
			enabledAuthentication.EnabledOnPermissions(permissions.ToPermissions());
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be enabled when permission is available, and disabled when not
		/// </summary>
		/// <param name="permissions">The permission(s)</param>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		public static void EnabledOnPermissions(this IChangeableNeedAuthentication<bool> enabledAuthentication, IEnumerable<string> permissions = null)
		{
			enabledAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.IsEnabled;
			enabledAuthentication.WhenPermission = true;
			enabledAuthentication.WhenPermissionMissing = false;

			if (permissions != null)
			{
				enabledAuthentication.Permissions = permissions;
			}
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be disabled when permission is available, and enabled when not
		/// </summary>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		/// <param name="permissions">The permission(s)</param>
		public static void EnabledOnPermissionsMissing(this IChangeableNeedAuthentication<bool> enabledAuthentication, string permissions)
		{
			enabledAuthentication.EnabledOnPermissionsMissing(permissions.ToPermissions());
		}

		/// <summary>
		///     Configure the IChangeableNeedAuthentication to be disabled when permission is available, and enabled when not
		/// </summary>
		/// <param name="enabledAuthentication">INeedAuthentication of type bool</param>
		/// <param name="permissions">The permission(s)</param>
		public static void EnabledOnPermissionsMissing(this IChangeableNeedAuthentication<bool> enabledAuthentication, IEnumerable<string> permissions = null)
		{
			enabledAuthentication.AuthenticationTargetProperty = AuthenticationTargetProperties.IsEnabled;
			enabledAuthentication.WhenPermission = false;
			enabledAuthentication.WhenPermissionMissing = true;

			if (permissions != null)
			{
				enabledAuthentication.Permissions = permissions;
			}
		}
	}
}