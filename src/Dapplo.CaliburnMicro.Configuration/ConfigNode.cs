// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     A node for the config screens, this has empty and sealed transactional methods
    /// </summary>
    public class ConfigNode : ConfigScreen
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