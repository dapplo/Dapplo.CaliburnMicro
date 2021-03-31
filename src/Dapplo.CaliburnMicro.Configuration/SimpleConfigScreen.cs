// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     A simple implementation of the ConfigScreen, this implements empty transactional methods which can be overriden when needed
    /// </summary>
    public class SimpleConfigScreen : ConfigScreen
    {
        /// <summary>
        ///     Terminate is called (must!) for every ITreeScreen when the parent IConfig Terminate is called.
        ///     No matter if this config screen was every shown and what reason there is to leave the configuration screen.
        /// </summary>
        public override void Terminate()
        {
        }

        /// <summary>
        ///     This is called when the configuration should be "persisted"
        /// </summary>
        public override void Commit()
        {
        }

        /// <summary>
        ///     This is called when the configuration should be "rolled back"
        /// </summary>
        public override void Rollback()
        {
        }
    }
}