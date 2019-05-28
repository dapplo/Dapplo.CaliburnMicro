//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

#endregion

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