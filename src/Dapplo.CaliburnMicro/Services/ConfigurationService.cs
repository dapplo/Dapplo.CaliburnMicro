// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.Config.Ini;

namespace Dapplo.CaliburnMicro.Services
{
    /// <summary>
    /// Shows a toast when the application starts
    /// </summary>
    [Service(nameof(CaliburnServices.ConfigurationService), nameof(CaliburnServices.CaliburnMicroBootstrapper))]
    public class ConfigurationService : IStartupAsync, IShutdownAsync
    {
        private readonly IniFileContainer _iniFileContainer;

        /// <summary>
        /// IOC constructor
        /// </summary>
        /// <param name="iniFileContainer">IniFileContainer</param>
        public ConfigurationService(IniFileContainer iniFileContainer)
        {
            _iniFileContainer = iniFileContainer;
        }

        /// <inheritdoc />
        public Task StartupAsync(CancellationToken cancellationToken = default)
        {
            return _iniFileContainer.ReloadAsync(true, cancellationToken);
        }

        /// <inheritdoc />
        public Task ShutdownAsync(CancellationToken cancellationToken = default)
        {
            return _iniFileContainer.WriteAsync(cancellationToken);
        }
    }
}
