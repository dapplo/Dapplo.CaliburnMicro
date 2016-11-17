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

using System.Windows;
using Dapplo.CaliburnMicro.Behaviors;

#endregion

namespace Dapplo.CaliburnMicro.Security
{
	/// <summary>
	///     Change the visibility of a UiElement depending on rights
	/// </summary>
	public static class AuthenticationVisibility
	{
		/// <summary>
		///     The permission to check against
		/// </summary>
		public static readonly DependencyProperty PermissionProperty = DependencyProperty.RegisterAttached(
			"Permission",
			typeof(string),
			typeof(AuthenticationVisibility),
			new PropertyMetadata(OnArgumentsChanged));

		/// <summary>
		///     Visibility to use when the current user doesn't have the permission
		///     Default is Visibility.Collapsed
		/// </summary>
		public static readonly DependencyProperty WhenPermissionMissingProperty = DependencyProperty.RegisterAttached(
			"WhenPermissionMissing",
			typeof(Visibility),
			typeof(AuthenticationVisibility),
			new PropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

		/// <summary>
		///     Visibility to use when the permission is available
		///     Default is Visibility.Visible
		/// </summary>
		public static readonly DependencyProperty WhenPermissionProperty = DependencyProperty.RegisterAttached(
			"WhenPermission",
			typeof(Visibility),
			typeof(AuthenticationVisibility),
			new PropertyMetadata(Visibility.Visible, OnArgumentsChanged));

		private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(host => new AuthenticationVisibilityBehavior((UIElement) host));

		/// <summary>
		///     This handles the fact that a dependency property has changed on a DependencyObject.
		///     It will also register the behavior when it hasn't been yet
		/// </summary>
		/// <param name="dependencyObject">DependencyObject</param>
		/// <param name="dependencyPropertyChangedEventArgs">dependencyPropertyChangedEventArgs</param>
		private static void OnArgumentsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			Behavior.Update(dependencyObject);
		}

		/// <summary>
		///     Returns the permission from the UIElement of the DependencyProperty specified in PermissionProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <returns>string which represents the permission of the in PermissionProperty specified DependencyProperty</returns>
		public static string GetPermission(UIElement uiElement)
		{
			return (string) uiElement.GetValue(PermissionProperty);
		}

		/// <summary>
		///     Sets the permission of the UIElement to the DependencyProperty specified in PermissionProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <param name="value">Permission to assign to the in PermissionProperty specified DependencyProperty</param>
		public static void SetPermission(UIElement uiElement, string value)
		{
			uiElement.SetValue(PermissionProperty, value);
		}

		/// <summary>
		///     Returns the visibility which is used when the user doesn't have the specified permission
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <returns>Visibility</returns>
		public static Visibility GetWhenPermissionMissing(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenPermissionMissingProperty);
		}

		/// <summary>
		///     Sets the visibility which is used when the user doesn't have the specified permission
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <param name="visibility">Visibility to use when matched</param>
		public static void SetWhenPermissionMissing(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenPermissionMissingProperty, visibility);
		}

		/// <summary>
		///     Returns the visibility which is used when the user has the specified permission
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <returns>Visibility</returns>
		public static Visibility GetWhenPermission(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenPermissionProperty);
		}

		/// <summary>
		///     Sets the visibility which is used when the user has the specified permission
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <param name="visibility">Visibility to use when matched</param>
		public static void SetWhenPermission(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenPermissionProperty, visibility);
		}

		/// <summary>
		///     Implementation of the AuthenticationVisibilityBehavior
		/// </summary>
		private sealed class AuthenticationVisibilityBehavior : Behavior<UIElement>
		{
			internal AuthenticationVisibilityBehavior(UIElement host) : base(host)
			{
			}

			/// <summary>
			///     check if the supplied permission is available
			/// </summary>
			/// <param name="permission"></param>
			/// <returns>true if the permission is available</returns>
			private bool HasPermission(string permission)
			{
				// Null permission is aways there
				if (permission == null)
				{
					return true;
				}
				var authenticationProvider = Dapplication.Current.Bootstrapper.GetExport<IAuthenticationProvider>();
				return authenticationProvider?.Value.HasPermission(permission) ?? false;
			}

			protected override void Update(UIElement host)
			{
				var permission = GetPermission(host);

				host.Visibility = HasPermission(permission)
					? GetWhenPermission(host)
					: GetWhenPermissionMissing(host);
			}
		}
	}
}