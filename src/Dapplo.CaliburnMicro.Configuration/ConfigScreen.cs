// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Dapplo.CaliburnMicro.Menu;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     A screen for the config must implement this
    /// </summary>
    public abstract class ConfigScreen : TreeScreen<IConfigScreen>, IConfigScreen
    {
        /// <summary>
        /// The IConfig parent for this IConfigScreen
        /// </summary>
        public virtual IConfig ConfigParent { get; private set; }

        /// <inheritdoc />
        public virtual void Initialize(IConfig config)
        {
            ConfigParent = config;
        }

        /// <summary>
        ///     Tests if the ITreeScreen contains the supplied text
        /// </summary>
        /// <param name="text">the text to search for</param>
        public virtual bool Contains(string text)
        {
            return CultureInfo.CurrentUICulture.CompareInfo.IndexOf(DisplayName, text, CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        ///     Terminate is called (must!) for every ITreeScreen when the parent IConfig Terminate is called.
        ///     No matter if this config screen was every shown and what reason there is to leave the configuration screen.
        /// </summary>
        public abstract void Terminate();

        /// <summary>
        ///     This is called when the configuration should be "persisted"
        /// </summary>
        public abstract void Commit();

        /// <summary>
        ///     This is called when the configuration should be "rolled back"
        /// </summary>
        public abstract void Rollback();
    }
}