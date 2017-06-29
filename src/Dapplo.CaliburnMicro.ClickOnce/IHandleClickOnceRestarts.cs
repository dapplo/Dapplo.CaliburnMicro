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

using System.Deployment.Application;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// If you want to handle a ClickOnce restart, e.g. by asking the user, implement this and export your class as typeof(IHandleClickOnceRestarts)
    /// </summary>
    public interface IHandleClickOnceRestarts
    {
        /// <summary>
        /// This is called after an update is applied, which usually should trigger a restart
        /// </summary>
        /// <param name="updateCheckInfo"></param>
        void HandleRestart(UpdateCheckInfo updateCheckInfo);
    }
}
