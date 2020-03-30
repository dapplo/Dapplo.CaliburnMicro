//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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
using System.ComponentModel;
using System.Windows;
using Autofac;
using Dapplo.Addons.Bootstrapper;
using Dapplo.CaliburnMicro.Behaviors;
using Dapplo.CaliburnMicro.Extensions;

namespace Dapplo.CaliburnMicro.Security.Behaviors
{
    /// <summary>
    ///     Change the visibility of a UiElement depending on rights
    /// </summary>
    public static class AuthenticationVisibility
    {
        /// <summary>
        ///     The permission to check against
        /// </summary>
        public static readonly DependencyProperty PermissionsProperty = DependencyProperty.RegisterAttached(
            "Permissions",
            typeof(IEnumerable<string>),
            typeof(AuthenticationVisibility),
            new PropertyMetadata(OnArgumentsChanged));

        /// <summary>
        ///     The operation used when the permissions are checked
        /// </summary>
        public static readonly DependencyProperty PermissionsOperationProperty = DependencyProperty.RegisterAttached(
            "PermissionsOperation",
            typeof(PermissionOperations),
            typeof(AuthenticationVisibility),
            new PropertyMetadata(PermissionOperations.Or, OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the current user doesn't have the permission
        ///     Default is Visibility.Collapsed
        /// </summary>
        public static readonly DependencyProperty WhenPermissionsMissingProperty = DependencyProperty.RegisterAttached(
            "WhenPermissionsMissing",
            typeof(Visibility),
            typeof(AuthenticationVisibility),
            new PropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the permission is available
        ///     Default is Visibility.Visible
        /// </summary>
        public static readonly DependencyProperty WhenPermissionsProperty = DependencyProperty.RegisterAttached(
            "WhenPermissions",
            typeof(Visibility),
            typeof(AuthenticationVisibility),
            new PropertyMetadata(Visibility.Visible, OnArgumentsChanged));

        private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(uiElement => new AuthenticationVisibilityBehavior((UIElement) uiElement));

        /// <summary>
        ///     Returns the permission from the UIElement of the DependencyProperty specified in PermissionsProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>
        ///     IEnumerable with strings which represents the permission of the in PermissionsProperty specified
        ///     DependencyProperty
        /// </returns>
        public static IEnumerable<string> GetPermissions(UIElement uiElement)
        {
            return (IEnumerable<string>) uiElement.GetValue(PermissionsProperty);
        }

        /// <summary>
        ///     Gets the permissions Operation of the UIElement to the DependencyProperty specified in
        ///     PermissionsOperationProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        public static PermissionOperations GetPermissionsOperations(UIElement uiElement)
        {
            return (PermissionOperations) uiElement.GetValue(PermissionsOperationProperty);
        }

        /// <summary>
        ///     Returns the visibility which is used when the user has the specified permissions
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenPermissions(UIElement uiElement)
        {
            return (Visibility) uiElement.GetValue(WhenPermissionsProperty);
        }

        /// <summary>
        ///     Returns the visibility which is used when the user doesn't have the specified permissions
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenPermissionsMissing(UIElement uiElement)
        {
            return (Visibility) uiElement.GetValue(WhenPermissionsMissingProperty);
        }

        /// <summary>
        ///     This handles the fact that a dependency property has changed on a DependencyObject.
        ///     It will also register the behavior when it hasn't been yet
        /// </summary>
        /// <param name="dependencyObject">DependencyObject</param>
        /// <param name="dependencyPropertyChangedEventArgs">dependencyPropertyChangedEventArgs</param>
        private static void OnArgumentsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            Behavior.Update(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        /// <summary>
        ///     Sets the permission of the UIElement to the DependencyProperty specified in PermissionsProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="permissions">Permissions to assign to the in PermissionsProperty specified DependencyProperty</param>
        public static void SetPermissions(UIElement uiElement, IEnumerable<string> permissions)
        {
            uiElement.SetValue(PermissionsProperty, permissions);
        }

        /// <summary>
        ///     Sets the permission of the UIElement to the DependencyProperty specified in PermissionsProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="permissions">Permissions as string to assign to the in PermissionsProperty specified DependencyProperty</param>
        public static void SetPermissionsAsString(UIElement uiElement, string permissions)
        {
            uiElement.SetValue(PermissionsProperty, permissions.ToPermissions());
        }

        /// <summary>
        ///     Sets the permissions Operation of the UIElement to the DependencyProperty specified in
        ///     PermissionsOperationProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="permissionOperations">PermissionOperations</param>
        public static void SetPermissionsOperations(UIElement uiElement, PermissionOperations permissionOperations)
        {
            uiElement.SetValue(PermissionsOperationProperty, permissionOperations);
        }

        /// <summary>
        ///     Sets the visibility which is used when the user has the specified permissions
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenPermissions(UIElement uiElement, Visibility visibility)
        {
            uiElement.SetValue(WhenPermissionsProperty, visibility);
        }

        /// <summary>
        ///     Sets the visibility which is used when the user doesn't have the specified permissions
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenPermissionsMissing(UIElement uiElement, Visibility visibility)
        {
            uiElement.SetValue(WhenPermissionsMissingProperty, visibility);
        }

        /// <summary>
        ///     Implementation of the AuthenticationVisibilityBehavior
        /// </summary>
        private sealed class AuthenticationVisibilityBehavior : Behavior<UIElement>
        {
            private Lazy<IAuthenticationProvider> _authenticationProvider;
            private IDisposable _authenticationProviderSubscription;

            internal AuthenticationVisibilityBehavior(UIElement uiElement) : base(uiElement)
            {
            }

            protected override void Attach(UIElement host)
            {
                base.Attach(host);
                _authenticationProvider = ApplicationBootstrapper.Instance.Container.Resolve<Lazy<IAuthenticationProvider>>();

                // if INotifyPropertyChanged is implemented: Call update when the _authenticationProvider changes it's values, 
                var notifyPropertyChangedAuthenticationProvider = _authenticationProvider?.Value as INotifyPropertyChanged;
                _authenticationProviderSubscription = notifyPropertyChangedAuthenticationProvider?.OnPropertyChanged(nameof(IAuthenticationProvider.HasPermissions)).Subscribe(x => Update(host, null));
            }

            protected override void Detach(UIElement host)
            {
                base.Detach(host);
                _authenticationProviderSubscription?.Dispose();
            }

            /// <summary>
            ///     check if the supplied permission is available
            /// </summary>
            /// <param name="neededPermissions"></param>
            /// <param name="permissionOperation">PermissionOperations</param>
            /// <returns>true if the permission is available</returns>
            private bool HasPermissions(IEnumerable<string> neededPermissions, PermissionOperations permissionOperation = PermissionOperations.Or)
            {
                // Null permission is aways there
                if (neededPermissions == null)
                {
                    return true;
                }
                return _authenticationProvider?.Value.HasPermissions(neededPermissions, permissionOperation) ?? false;
            }

            /// <summary>
            ///     Update the Visibility of the UIElement
            /// </summary>
            /// <param name="uiElement">UIElement</param>
            /// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
            protected override void Update(UIElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                var permissions = GetPermissions(uiElement);
                var permissionsOperation = GetPermissionsOperations(uiElement);

                uiElement.Visibility = HasPermissions(permissions, permissionsOperation)
                    ? GetWhenPermissions(uiElement)
                    : GetWhenPermissionsMissing(uiElement);
            }
        }
    }
}