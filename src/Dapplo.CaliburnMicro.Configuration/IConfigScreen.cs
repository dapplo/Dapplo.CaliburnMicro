// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.CaliburnMicro.Menu;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     Specialized interface for config screen
    /// </summary>
    public interface IConfigScreen : ITreeScreenNode<IConfigScreen>
    {
        /// <summary>
        ///     Do some general initialization, if needed
        ///     This is called when the config UI is initialized
        /// </summary>
        void Initialize(IConfig config);

        /// <summary>
        ///     This is called when the configuration should be "persisted"
        /// </summary>
        void Commit();

        /// <summary>
        ///     Tests if the ITreeScreen contains the supplied text
        /// </summary>
        /// <param name="text">the text to search for</param>
        bool Contains(string text);

        /// <summary>
        ///     This is called when the configuration should be "rolled back"
        /// </summary>
        void Rollback();

        /// <summary>
        ///     Terminate is called (must!) for every ITreeScreen when the parent IConfig Terminate is called.
        ///     No matter if this config screen was every shown and what reason there is to leave the configuration screen.
        /// </summary>
        void Terminate();
    }
}