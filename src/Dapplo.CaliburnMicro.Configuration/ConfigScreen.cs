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

using System.Globalization;
using Dapplo.CaliburnMicro.Menu;

#endregion

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