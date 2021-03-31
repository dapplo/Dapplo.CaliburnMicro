// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Config.Interfaces;

namespace Dapplo.CaliburnMicro.Configuration
{
    /// <summary>
    ///     Base interface for the IConfiguration
    /// </summary>
    public interface IConfig : INotifyPropertyChanged
    {
        /// <summary>
        ///     Can the current config be cancelled?
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        ///     Test if the config can be OKed
        /// </summary>
        bool CanOk { get; }

        /// <summary>
        ///     The config screens for the config UI
        /// </summary>
        IEnumerable<Lazy<IConfigScreen>> ConfigScreens { get; set; }

        /// <summary>
        ///     Returns the current config screen
        /// </summary>
        IConfigScreen CurrentConfigScreen { get; set; }

        /// <summary>
        ///     The property for the filter text
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        ///     The config screens for the config UI in the tree
        /// </summary>
        ICollection<ITreeNode<IConfigScreen>> TreeItems { get; }

        /// <summary>
        ///     If CanCancel is true, this will call Rollback on all IConfigScreens and TryClose afterwards
        /// </summary>
        void Cancel();

        /// <summary>
        ///     Initialize will o.a. initialize and sort (tree) the config screens
        /// </summary>
        void Initialize();

        /// <summary>
        ///     If CanOk is true, this will call Commit on all IConfigScreens and TryClose afterwards
        /// </summary>
        void Ok();

        /// <summary>
        ///     Register an instanceof ITransactionalProperties to be included in the transaction (rollback or commit will be
        ///     called for you)
        /// </summary>
        /// <param name="transactionalProperties"></param>
        void Register(ITransactionalProperties transactionalProperties);

        /// <summary>
        ///     Terminate the config.
        /// </summary>
        void Terminate();

        /// <summary>
        /// This updates the state of the configuration window, like the Ok and Cancel buttons.
        /// e.g. this can be called from a IConfigScreen to notify of changes to the CanClose
        /// </summary>
        void RefreshState();
    }

    /// <summary>
    ///     This is the generic interface for a config implementation
    /// </summary>
    public interface IConfig<TConfigScreen> : IConfig
    {
        /// <summary>
        ///     The IConfigScreens items of the config
        /// </summary>
        new IEnumerable<Lazy<TConfigScreen>> ConfigScreens { get; }

        /// <summary>
        ///     Returns the current config screen
        /// </summary>
        new TConfigScreen CurrentConfigScreen { get; }

        /// <summary>
        ///     The config screens for the config UI in the tree
        /// </summary>
        new ICollection<ITreeNode<TConfigScreen>> TreeItems { get; }
    }
}