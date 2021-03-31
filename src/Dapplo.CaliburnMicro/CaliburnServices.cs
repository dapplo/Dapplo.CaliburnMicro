// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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