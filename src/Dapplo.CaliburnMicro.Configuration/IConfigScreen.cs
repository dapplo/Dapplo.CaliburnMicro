//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

using Dapplo.CaliburnMicro.Menu;

#endregion

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
        ///     Terminate the config screen.
        ///     This is called when the parent config UI is terminated
        /// </summary>
        void Terminate();
    }
}