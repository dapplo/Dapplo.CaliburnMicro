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

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     Helps to structure the order of starting Dappo StartupActions
    /// </summary>
    public enum CaliburnStartOrder
    {
        /// <summary>
        ///     This is the order which the CaliburnMicroBootstrapper uses, if you depend on this take a higher order!
        /// </summary>
        CaliburnMicroBootstrapper,

        /// <summary>
        ///     This is the id of the TrayIconManager, IF Dapplo.CaliburnMicro.NotifyIconWpf is used
        /// </summary>
        TrayIconManager,

        /// <summary>
        /// If you want to show toasts, set this as your prerequisite
        /// </summary>
        ToastConductor
    }
}