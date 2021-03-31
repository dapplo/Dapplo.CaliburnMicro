// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Dapp.Services
{
    /// <summary>
    /// This takes care of showing the shell(s)
    /// </summary>
    [Service(nameof(ShellStartup), nameof(CaliburnServices.CaliburnMicroBootstrapper), TaskSchedulerName = "ui")]
    public class ShellStartup : IStartup
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IWindowManager _windowManager;
        private readonly IEnumerable<Lazy<IShell>> _shells;

        /// <summary>
        /// IOC constructor
        /// </summary>
        /// <param name="windowManager">IWindowManager</param>
        /// <param name="shells">IEnumerable with all IShell items, but lazy</param>
        public ShellStartup(
            IWindowManager windowManager,
            IEnumerable<Lazy<IShell>> shells)
        {
            _windowManager = windowManager;
            _shells = shells;
        }


        /// <inheritdoc />
        public void Startup()
        {
            Log.Debug().WriteLine("Start of all IShell");

            foreach (var shell in _shells)
            {
                var viewModel = shell.Value;
                _windowManager.ShowWindow(viewModel);
            }
        }

    }
}
