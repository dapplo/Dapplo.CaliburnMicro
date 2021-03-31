// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     An extension of the ConfigScreen which adds authentication
    /// </summary>
    public abstract class AuthenticatedConfigScreen<TWhen> : ConfigScreen, IChangeableNeedAuthentication<TWhen>
    {
        private AuthenticationTargetProperties _authenticationTargetProperty = AuthenticationTargetProperties.None;
        private PermissionOperations _permissionOperation = PermissionOperations.Or;
        private IEnumerable<string> _permissions = new List<string>();
        private TWhen _whenPermission;
        private TWhen _whenPermissionMissing;

        /// <inheritdoc cref="INeedAuthentication"/>
        public AuthenticationTargetProperties AuthenticationTargetProperty
        {
            get => _authenticationTargetProperty;
            set
            {
                _authenticationTargetProperty = value;
                NotifyOfPropertyChange(nameof(AuthenticationTargetProperty));
            }
        }

        /// <inheritdoc cref="INeedAuthentication"/>
        public PermissionOperations PermissionOperation
        {
            get => _permissionOperation;
            set
            {
                _permissionOperation = value;
                NotifyOfPropertyChange(nameof(PermissionOperation));
            }
        }

        /// <inheritdoc cref="INeedAuthentication"/>
        public IEnumerable<string> Permissions
        {
            get => _permissions;
            set
            {
                _permissions = value;
                NotifyOfPropertyChange(nameof(Permissions));
            }
        }

        /// <inheritdoc cref="INeedAuthentication" />
        public TWhen WhenPermission
        {
            get => _whenPermission;
            set
            {
                _whenPermission = value;
                NotifyOfPropertyChange(nameof(WhenPermission));
            }
        }

        /// <inheritdoc cref="INeedAuthentication" />
        public TWhen WhenPermissionMissing
        {
            get => _whenPermissionMissing;
            set
            {
                _whenPermissionMissing = value;
                NotifyOfPropertyChange(nameof(WhenPermissionMissing));
            }
        }
    }
}