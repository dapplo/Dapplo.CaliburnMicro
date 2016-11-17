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
using System.Linq;
using System.Windows;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors.Security
{
	/// <summary>
	///     This attribute can be used to specify information for UI attributes, specifying if it's enabled/disabled depending
	///     on certain permissions
	/// TODO: Add a mode, or/and for the permissions
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class UiEnabledPermissionsAttribute : Attribute
	{
		private static readonly IDictionary<Type, UiEnabledPermissionsAttribute> AttributeCache = new Dictionary<Type, UiEnabledPermissionsAttribute>();
		/// <summary>
		/// checks if the supplied attribute is available for the type of the UIElement, and sets the behavior accordingly
		/// </summary>
		/// <param name="element"></param>
		public static void ApplyBehaviorWhenAttribute(UIElement element)
		{
			var uiType = element.GetType();
			UiEnabledPermissionsAttribute attribute;
			if (AttributeCache.TryGetValue(uiType, out attribute))
			{
				element.SetCurrentValue(AuthenticationEnabled.PermissionProperty, attribute.Permissions);
				if (!attribute.WhenPermission)
				{
					element.SetCurrentValue(AuthenticationEnabled.WhenPermissionProperty, attribute.WhenPermission);
				}
				if (attribute.WhenPermissionMissing)
				{
					element.SetCurrentValue(AuthenticationEnabled.WhenPermissionMissingProperty, attribute.WhenPermissionMissing);
				}
			}
			else
			{
				attribute = uiType.GetCustomAttributes(typeof(UiEnabledPermissionsAttribute), true).Cast<UiEnabledPermissionsAttribute>().FirstOrDefault();
				AttributeCache[uiType] = attribute;
			}

		}

		/// <summary>
		///     The permissions which are needed
		/// </summary>
		public string Permissions { get; set; }

		/// <summary>
		///     The value which is used when the permissions are missing
		/// </summary>
		public bool WhenPermissionMissing { get; set; } = false;

		/// <summary>
		///     The value which is used when the permission(s) is/are missing, default = true
		/// </summary>
		public bool WhenPermission { get; set; } = true;
	}
}