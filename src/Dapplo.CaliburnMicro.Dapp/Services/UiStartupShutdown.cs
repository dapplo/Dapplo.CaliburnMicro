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

#region usings
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;
using System.Linq;
#endregion

namespace Dapplo.CaliburnMicro.Dapp.Services
{
    /// <summary>
    /// This takes care of showing the shell(s)
    /// </summary>
    [ServiceOrder(CaliburnStartOrder.Shell)]
    public class UiStartupShutdown : IStartupAsync, IShutdownAsync
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IWindowManager _windowManager;
        private readonly IEnumerable<Lazy<IShell>> _shells;
        private readonly IEnumerable<Lazy<IUiStartup, ServiceOrderAttribute>> _uiStartupModules;
        private readonly IEnumerable<Lazy<IUiShutdown, ServiceOrderAttribute>> _uiShutdownModules;

        /// <inheritdoc />
        public UiStartupShutdown(
            IWindowManager windowManager,
            IEnumerable<Lazy<IShell>> shells,
            IEnumerable<Lazy<IUiStartup, ServiceOrderAttribute>> uiStartupModules,
            IEnumerable<Lazy<IUiShutdown, ServiceOrderAttribute>> uiShutdownModules
            )
        {
            _windowManager = windowManager;
            _shells = shells;
            _uiStartupModules = uiStartupModules;
            _uiShutdownModules = uiShutdownModules;
        }


        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Log.Debug().WriteLine("Start of all IUIStartup");

            await Execute.OnUIThreadAsync(() =>
            {
                var orderedStartupModules = from export in _uiStartupModules orderby export.Metadata.StartupOrder select export;

                // Activate all "UI" Services
                foreach (var startupModule in orderedStartupModules)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    startupModule.Value.Start();
                }

                foreach (var shell in _shells)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    var viewModel = shell.Value;
                    _windowManager.ShowWindow(viewModel);
                }

            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ShutdownAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Log.Debug().WriteLine("Shutdown of all IUiShutdownAction");

            await Execute.OnUIThreadAsync(() =>
            {
                var orderedShutdownModules = from export in _uiShutdownModules orderby export.Metadata.ShutdownOrder select export;

                // Activate all "UI" Services
                foreach (var shutdownModule in orderedShutdownModules)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    shutdownModule.Value.Shutdown();
                }
            }).ConfigureAwait(false);
        }
    }
}
