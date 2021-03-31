// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Security;

namespace Application.Demo.Services
{
    /// <summary>
    ///     This exports a IAuthenticationProvider, which is used to show or hide elements in the UI depending on the available
    ///     rights
    /// </summary>
    public class AuthenticationProvider : SimpleAuthenticationProvider
    {
        public AuthenticationProvider()
        {
            AddPermission("Admin");
        }
    }
}