// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     An authenticatable config node, which is an extension of the AuthenticatedConfigScreen with empty (and sealed) transaction methods.
    /// </summary>
    public class AuthenticatedConfigNode<TWhen> : AuthenticatedConfigScreen<TWhen>
    {
        /// <inheritdoc />
        public sealed override void Rollback()
        {
        }

        /// <inheritdoc />
        public sealed override void Terminate()
        {
        }

        /// <inheritdoc />
        public sealed override void Commit()
        {
        }
    }
}