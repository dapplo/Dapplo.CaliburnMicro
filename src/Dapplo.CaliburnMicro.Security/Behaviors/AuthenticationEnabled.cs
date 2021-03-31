// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    ///     Change the IsEnabled of a UiElement depending on rights
    /// </summary>
    public static class AuthenticationEnabled
    {
        /// <summary>
        ///     The permission(s) to check against
        /// </summary>
        public static readonly DependencyProperty PermissionsProperty = DependencyProperty.RegisterAttached(
            "Permissions",
            typeof(IEnumerable<string>),
            typeof(AuthenticationEnabled),
            new PropertyMetadata(OnArgumentsChanged));

        /// <summary>
        ///     The operation used when the permissions are checked
        /// </summary>
        public static readonly DependencyProperty PermissionsOperationProperty = DependencyProperty.RegisterAttached(
            "PermissionsOperation",
            typeof(PermissionOperations),
            typeof(AuthenticationEnabled),
            new PropertyMetadata(PermissionOperations.Or, OnArgumentsChanged));

        /// <summary>
        ///     IsEnabled value to use when the current user doesn't have the permission
        ///     Default is false
        /// </summary>
        public static readonly DependencyProperty WhenPermissionsMissingProperty = DependencyProperty.RegisterAttached(
            "WhenPermissionsMissing",
            typeof(bool),
            typeof(AuthenticationEnabled),
            new PropertyMetadata(false, OnArgumentsChanged));

        /// <summary>
        ///     IsEnabled value to use when the permissions are available
        ///     Default is true
        /// </summary>
        public static readonly DependencyProperty WhenPermissionsProperty = DependencyProperty.RegisterAttached(
            "WhenPermissions",
            typeof(bool),
            typeof(AuthenticationEnabled),
            new PropertyMetadata(true, OnArgumentsChanged));

        private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(uiElement => new AuthenticationEnabledBehavior((UIElement) uiElement));

        /// <summary>
        ///     Returns the permissions from the UIElement of the DependencyProperty specified in PermissionsProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>string which represents the permissions of the in PermissionsProperty specified DependencyProperty</returns>
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
        ///     Returns the IsEnabled which is used when the user has the specified permission
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>bool</returns>
        public static bool GetWhenPermissions(UIElement uiElement)
        {
            return (bool) uiElement.GetValue(WhenPermissionsProperty);
        }

        /// <summary>
        ///     Returns the IsEnabled which is used when the user doesn't have the specified permission
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>bool</returns>
        public static bool GetWhenPermissionsMissing(UIElement uiElement)
        {
            return (bool) uiElement.GetValue(WhenPermissionsMissingProperty);
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
        ///     Sets the permissions of the UIElement to the DependencyProperty specified in PermissionsProperty.
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
        ///     Sets the IsEnabled which is used when the user has the specified permission
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="isEnabled">bool to use when matched</param>
        public static void SetWhenPermissions(UIElement uiElement, bool isEnabled)
        {
            uiElement.SetValue(WhenPermissionsProperty, isEnabled);
        }

        /// <summary>
        ///     Sets the IsEnabled which is used when the user doesn't have the specified permission
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="isEnabled">bool to use when matched</param>
        public static void SetWhenPermissionsMissing(UIElement uiElement, bool isEnabled)
        {
            uiElement.SetValue(WhenPermissionsMissingProperty, isEnabled);
        }

        /// <summary>
        ///     Implementation of the AuthenticationEnabledBehavior
        /// </summary>
        private sealed class AuthenticationEnabledBehavior : Behavior<UIElement>
        {
            private Lazy<IAuthenticationProvider> _authenticationProvider;
            private IDisposable _authenticationProviderSubscription;

            internal AuthenticationEnabledBehavior(UIElement uiElement) : base(uiElement)
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
            ///     check if the supplied permissions are available
            /// </summary>
            /// <param name="neededPermissions"></param>
            /// <param name="permissionOperation">PermissionOperations</param>
            /// <returns>true if the permissions are available</returns>
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
            ///     Update the IsEnabled value of the UIElement
            /// </summary>
            /// <param name="uiElement">UIElement</param>
            /// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
            protected override void Update(UIElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                var permissions = GetPermissions(uiElement);
                var permissionsOperation = GetPermissionsOperations(uiElement);

                uiElement.IsEnabled = HasPermissions(permissions, permissionsOperation)
                    ? GetWhenPermissions(uiElement)
                    : GetWhenPermissionsMissing(uiElement);
            }
        }
    }
}