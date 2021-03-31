// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dapplo.CaliburnMicro.Security.Behaviors;

namespace Dapplo.CaliburnMicro.Security.DesignTime
{
    /// <summary>
    /// A design-time implementation of INeedAuthentication for the bool (IsEnabled)
    /// </summary>
    public class BoolAuthentication : INeedAuthentication<bool>
    {
        private BoolAuthentication()
        {

        }
        /// <inheritdoc />
        public AuthenticationTargetProperties AuthenticationTargetProperty { get; set; }
        /// <inheritdoc />
        public PermissionOperations PermissionOperation { get; set; }
        /// <inheritdoc />
        public IEnumerable<string> Permissions { get; set; }
        /// <inheritdoc />
        public bool WhenPermission { get; set; }
        /// <inheritdoc />
        public bool WhenPermissionMissing { get; set; }
    }
}
