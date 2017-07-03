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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Dapp.Services
{
    /// <summary>
    /// This takes care of showing the shell(s)
    /// </summary>
    [StartupAction(StartupOrder = (int)CaliburnStartOrder.Shell)]
    public class ShellStartupAction : IAsyncStartupAction
    {
        private static readonly LogSource Log = new LogSource();

        [ImportMany]
        private IEnumerable<Lazy<IShell>> Shells { get; set; }

        [ImportMany]
        private IEnumerable<Lazy<IUiService>> UiServices { get; set; }

        [Import]
        private IWindowManager WindowManager { get; set; }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken token = new CancellationToken())
        {
            await Execute.OnUIThreadAsync(() =>
            {
                // Activate all "UI" Services
                foreach (var lazyUiService in UiServices)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    if (lazyUiService.IsValueCreated)
                    {
                        continue;
                    }
                    var uiService = lazyUiService.Value;
                    Debug.Assert(uiService != null);
                }

                foreach (var shell in Shells)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    var viewModel = shell.Value;
                    WindowManager.ShowWindow(viewModel);
                }

            }).ConfigureAwait(false);
        }
    }
}
