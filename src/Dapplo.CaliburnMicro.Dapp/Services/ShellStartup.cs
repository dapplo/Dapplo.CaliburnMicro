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
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;

#endregion

namespace Dapplo.CaliburnMicro.Dapp.Services
{
    /// <summary>
    /// This takes care of showing the shell(s)
    /// </summary>
    [Service(nameof(ShellStartup), nameof(CaliburnStartOrder.CaliburnMicroBootstrapper), TaskSchedulerName = "ui")]
    public class ShellStartup : IStartup
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IWindowManager _windowManager;
        private readonly IEnumerable<Lazy<IShell>> _shells;

        /// <inheritdoc />
        public ShellStartup(
            IWindowManager windowManager,
            IEnumerable<Lazy<IShell>> shells)
        {
            _windowManager = windowManager;
            _shells = shells;
        }


        /// <inheritdoc />
        public void Start()
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
