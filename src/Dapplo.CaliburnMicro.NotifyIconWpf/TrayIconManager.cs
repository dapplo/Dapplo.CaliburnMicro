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

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.Log;

#endregion

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
    /// <summary>
    ///     This takes care of starting and managing your tray icons
    /// </summary>
    [Service(nameof(CaliburnServices.TrayIconManager), nameof(CaliburnServices.ConfigurationService), TaskSchedulerName = "ui")]
    public class TrayIconManager : IStartupAsync, IShutdownAsync, ITrayIconManager
    {
        private static readonly LogSource Log = new LogSource();
        private readonly IEnumerable<Lazy<ITrayIconViewModel>> _trayIconViewModels;
        private readonly ResourceManager _resourceManager;
        private readonly IWindowManager _windowsManager;

        /// <summary>
        ///     Cache for the created tray icons
        /// </summary>
        private readonly IDictionary<WeakReference<ITrayIconViewModel>, WeakReference<ITrayIcon>> _trayIcons =
            new Dictionary<WeakReference<ITrayIconViewModel>, WeakReference<ITrayIcon>>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="resourceManager">ResourceManager</param>
        /// <param name="trayIconViewModels">IEnumerable with laze tray icon ViewModels</param>
        /// <param name="windowsManager">IWindowManager</param>
        public TrayIconManager(ResourceManager resourceManager,
            IEnumerable<Lazy<ITrayIconViewModel>> trayIconViewModels,
            IWindowManager windowsManager
            )
        {
            _resourceManager = resourceManager;
            _windowsManager = windowsManager;
            _trayIconViewModels = trayIconViewModels;
        }

        /// <summary>
        ///     Hide all tray icons to prevent them hanging useless in the system tray
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ShutdownAsync(CancellationToken cancellationToken = default)
        {
            var trayIcons = TrayIcons.ToList();
            if (trayIcons.Any())
            {
                Log.Debug().WriteLine("Hiding all created tray-icons");

                await Execute.OnUIThreadAsync(() =>
                {
                    Log.Debug().WriteLine("Hiding {0} tray-icons", trayIcons.Count);
                    foreach (var trayIcon in trayIcons)
                    {
                        trayIcon.Hide();
                    }
                }).ConfigureAwait(false);
            }
            else
            {
                Log.Debug().WriteLine("No tray-icons to hide");
            }
        }

        /// <summary>
        ///     Find all tray icons and initialize them.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task StartupAsync(CancellationToken cancellationToken = default)
        {
            await Execute.OnUIThreadAsync(InitializeTrayIconViewModels).ConfigureAwait(false);
        }

        /// <summary>
        /// Do the initialization
        /// </summary>
        private void InitializeTrayIconViewModels()
        {
            // Load the TrayIconResourceDirectory.xaml for the look & feel
            var trayIconResourceDirectory = new Uri("pack://application:,,,/Dapplo.CaliburnMicro.NotifyIconWpf;component/TrayIconResourceDirectory.xaml", UriKind.RelativeOrAbsolute);
            _resourceManager.AddResourceDictionary(trayIconResourceDirectory, 1);

            foreach (var trayIconViewModel in _trayIconViewModels.Select(x => x.Value))
            {
                // Get the view, to store it as ITrayIcon
                trayIconViewModel.ViewAttached += (sender, viewAttachedEventArgs) =>
                {
                    var trayIcon = viewAttachedEventArgs.View as ITrayIcon;
                    _trayIcons.Add(new WeakReference<ITrayIconViewModel>(trayIconViewModel), new WeakReference<ITrayIcon>(trayIcon));
                };
                _windowsManager.ShowPopup(trayIconViewModel);
            }
        }

        /// <summary>
        /// Provide all available tray icons
        /// </summary>
        public IEnumerable<ITrayIcon> TrayIcons => _trayIcons.Values.Select(x =>
            {
                x.TryGetTarget(out var trayIcon);
                return trayIcon;
            })
            .Where(x => x != null);

        /// <summary>
        ///     Get the ITrayIcon belonging to the specified ITrayIconViewModel instance
        /// </summary>
        /// <param name="trayIconViewModel">ViewModel instance to get the ITrayIcon for</param>
        /// <returns>ITrayIcon</returns>
        public ITrayIcon GetTrayIconFor(ITrayIconViewModel trayIconViewModel)
        {
            var result = _trayIcons.Where(x =>
                {
                    // Try to get the key to compare
                    x.Key.TryGetTarget(out var currentTrayIconViewModel);
                    // Try to get the value to check if it's available
                    x.Value.TryGetTarget(out var trayIcon);
                    return trayIconViewModel == currentTrayIconViewModel && trayIcon != null;
                })
                .Select(x =>
                {
                    x.Value.TryGetTarget(out var trayIcon);
                    return trayIcon;
                })
                .FirstOrDefault();
            return result;
        }
    }
}