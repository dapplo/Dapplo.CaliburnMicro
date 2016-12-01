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
using Dapplo.CaliburnMicro.Behaviors.Security;
using Dapplo.CaliburnMicro.Security;

#endregion

namespace Dapplo.CaliburnMicro.Tree
{
	/// <summary>
	///     An extension of the TreeScreen which adds authentication
	/// </summary>
	public abstract class AuthenticatedTreeScreen<TTreeScreen, TWhen> : TreeScreen<TTreeScreen>, IChangeableNeedAuthentication<TWhen>
	{
		private TWhen _whenPermissionMissing;
		private TWhen _whenPermission;
		private IEnumerable<string> _permissions = new List<string>();
		private PermissionOperations _permissionOperation = PermissionOperations.Or;
		private AuthenticationTargetProperties _authenticationTargetProperty = AuthenticationTargetProperties.None;

		/// <inheritdoc />
		public AuthenticationTargetProperties AuthenticationTargetProperty
		{
			get { return _authenticationTargetProperty; }
			set
			{
				_authenticationTargetProperty = value;
				NotifyOfPropertyChange(nameof(AuthenticationTargetProperty));
			}
		}

		/// <inheritdoc />
		public PermissionOperations PermissionOperation
		{
			get { return _permissionOperation; }
			set
			{
				_permissionOperation = value;
				NotifyOfPropertyChange(nameof(PermissionOperation));
			}
		}

		/// <inheritdoc />
		public IEnumerable<string> Permissions
		{
			get
			{
				return _permissions;
			}
			set
			{
				_permissions = value;
				NotifyOfPropertyChange(nameof(Permissions));
			}
		}

		/// <inheritdoc />
		public TWhen WhenPermission
		{
			get
			{
				return _whenPermission;
			}
			set
			{
				_whenPermission = value;
				NotifyOfPropertyChange(nameof(WhenPermission));
			}
		}

		/// <inheritdoc />
		public TWhen WhenPermissionMissing
		{
			get
			{
				return _whenPermissionMissing;
			}
			set
			{
				_whenPermissionMissing = value;
				NotifyOfPropertyChange(nameof(WhenPermissionMissing));
			}
		}
	}
}