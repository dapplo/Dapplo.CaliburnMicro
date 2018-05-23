//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.InterfaceImpl.Extensions;

#endregion

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