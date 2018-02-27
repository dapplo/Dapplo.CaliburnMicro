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

#region usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
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
    [StartupAction(StartupOrder = (int)CaliburnStartOrder.Shell)]
    [ShutdownAction(ShutdownOrder = (int)CaliburnStartOrder.Shell)]
    public class UiStartupShutdownAction : IAsyncStartupAction, IAsyncShutdownAction
    {
        private static readonly LogSource Log = new LogSource();

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [ImportMany] private IEnumerable<Lazy<IShell>> Shells = null;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [ImportMany] private IEnumerable<Lazy<IUiStartupAction, IUiStartupMetadata>> _uiStartupModules = null;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [ImportMany] private IEnumerable<Lazy<IUiShutdownAction, IShutdownMetadata>> _uiShutdownModules = null;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [Import] private IWindowManager _windowManager = null;

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Log.Debug().WriteLine("Start of all IUiStartupAction");

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

                foreach (var shell in Shells)
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
