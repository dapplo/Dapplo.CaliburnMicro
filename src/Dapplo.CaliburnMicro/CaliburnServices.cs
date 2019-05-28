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

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     Helps to structure the order of starting Dappo StartupActions
    /// </summary>
    public enum CaliburnServices
    {
        /// <summary>
        ///     If you want to make sure that the IEventAggregator, IWindowManager etc are available, set this as your prerequisite
        /// </summary>
        CaliburnMicroBootstrapper,

        /// <summary>
        ///     If you want to interact with the systemtray set this as your prerequisite
        /// </summary>
        TrayIconManager,

        /// <summary>
        /// If you want to show toasts, set this as your prerequisite
        /// </summary>
        ToastConductor,

        /// <summary>
        /// Reference this when you want to make sure the configuration is loaded
        /// </summary>
        ConfigurationService,

        /// <summary>
        /// Reference this when you want to make sure the language is loaded
        /// </summary>
        LanguageService
    }
}