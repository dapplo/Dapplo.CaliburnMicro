#region Dapplo 2016-2018 - GNU Lesser General Public License
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
#endregion 

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    /// Marker interface for UI Services, every class exported with this interface is automatically instanciated right after CaliburnMicro is.
    /// Except for the constructor, nothing is called. All dependencies are injected.
    /// </summary>
    public interface IUiStartup
    {
        /// <summary>
        ///     Perform a start of whatever needs to be started.
        ///     Make sure this can be called multiple times, e.g. do nothing when it was already started.
        ///     throw a StartupException if something went terribly wrong and the application should NOT continue
        /// </summary>
        void Start();
    }
}
