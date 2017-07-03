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
using System.ComponentModel.Composition;
using System.Deployment.Application;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.ClickOnce.Configuration;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// This StartupAction takes care of managing ClickOnce applications
    /// </summary>
    [StartupAction(StartupOrder = (int)CaliburnStartOrder.Bootstrapper + 1, AwaitStart = true)]
    [Export(typeof(IClickOnceService))]
    [Export(typeof(IVersionProvider))]
    public class ClickOnceService : PropertyChangedBase, IStartupAction, IClickOnceService, IHandleClickOnceUpdates, IApplyClickOnceUpdates, IHandleClickOnceRestarts
    {
        private static readonly LogSource Log = new LogSource();
        private bool _isInCheck;
        private Version _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private Version _latestVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private bool _isUpdateAvailable;
        private DateTimeOffset _lastCheckedOn;

        [Import]
        private IStartupShutdownBootstrapper StartupShutdownBootstrapper { get; set; }

        [Import]
        private IClickOnceConfiguration ClickOnceConfiguration { get; set; }

        [Import("ui")]
        private SynchronizationContext UiSynchronizationContext { get; set; }

        [Import(AllowDefault = true)]
        private IHandleClickOnceUpdates HandleClickOnceUpdates { get; set; }

        [Import(AllowDefault = true)]
        private IHandleClickOnceRestarts HandleClickOnceRestarts { get; set; }

        [Import(AllowDefault = true)]
        private IApplyClickOnceUpdates ApplyClickOnceUpdates { get; set; }

        /// <inheritdoc />
        public void Start()
        {
            if (!IsClickOnce)
            {
                Log.Info().WriteLine("Application is not deployed via ClickOnce, there will be no checks for updates.");
                if (MessageBox.Show("Testing UI stop during start?", "Update", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }
                // Make sure the startup of the bootstrapper is not continued
                StartupShutdownBootstrapper.CancelStartup();
                Execute.BeginOnUIThread(Restart);
                return;
            }
            Log.Info().WriteLine("Configuring ClickOnce update handling.");

            CurrentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            IObservable <long> updateObservable = null;

            var checkInBackground = ClickOnceConfiguration.EnableBackgroundUpdateCheck && ClickOnceConfiguration.CheckInterval > 0;
            if (checkInBackground)
            {
                // Check in background
                updateObservable = Observable.Interval(TimeSpan.FromMinutes(ClickOnceConfiguration.CheckInterval));
            }

            if (ClickOnceConfiguration.CheckOnStart)
            {
                Log.Info().WriteLine("Starting application update check.");
                var updateCheckInfo = CheckForUpdate();
                HandleUpdateCheck(updateCheckInfo);
            }
            // Register the check, if there is an update observable
            updateObservable?.ObserveOn(UiSynchronizationContext).Select(l => CheckForUpdate())
                .Where(updateCheckInfo => updateCheckInfo != null)
                .Subscribe(HandleUpdateCheck);
        }

        /// <inheritdoc />
        public DateTimeOffset LastCheckedOn
        {
            get
            {
                return _lastCheckedOn;
            }
            private set
            {
                _lastCheckedOn = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public bool IsUpdateAvailable
        {
            get
            {
                return _isUpdateAvailable;
            }
            set
            {
                _isUpdateAvailable = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public bool IsClickOnce { get; } = ApplicationDeployment.IsNetworkDeployed;

        /// <inheritdoc />
        public Version CurrentVersion
        {
            get
            {
                return _currentVersion;
            }
            private set
            {
                _currentVersion = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public Version LatestVersion
        {
            get { return _latestVersion; }
            private set {
                _latestVersion = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public void HandleUpdateCheck(UpdateCheckInfo updateCheckInfo)
        {
            LatestVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            IsUpdateAvailable = updateCheckInfo.UpdateAvailable;
            if (!IsUpdateAvailable)
            {
                Log.Debug().WriteLine("No update available.");
                return;
            }
            LatestVersion = updateCheckInfo.AvailableVersion;
            Log.Info().WriteLine("Update is available. Version information: Current: {0}, updated {1}, new: {2}", ApplicationDeployment.CurrentDeployment.CurrentVersion, ApplicationDeployment.CurrentDeployment.UpdatedVersion, updateCheckInfo.AvailableVersion);

            if (ApplicationDeployment.CurrentDeployment.UpdatedVersion >= updateCheckInfo.AvailableVersion)
            {
                Log.Info().WriteLine("For version {0} there is an update to version {1} available, it was already applied and will be used at next start.", ApplicationDeployment.CurrentDeployment.CurrentVersion, updateCheckInfo.AvailableVersion);
                return;
            }
            Log.Info().WriteLine("For version {0} there is an update to version {1} available, starting the update.", ApplicationDeployment.CurrentDeployment.CurrentVersion, updateCheckInfo.AvailableVersion);

            if (HandleClickOnceUpdates != null)
            {
                // Have the application handle the check
                HandleClickOnceUpdates.HandleUpdateCheck(updateCheckInfo);
            }
            else
            {
                if (ApplyClickOnceUpdates == null && (updateCheckInfo.IsUpdateRequired || ClickOnceConfiguration.AutoUpdate))
                {
                    // "Force" update
                    ApplyUpdate(updateCheckInfo);
                }
                else
                {
                    // Have the application handle the update
                    ApplyClickOnceUpdates?.ApplyUpdate(updateCheckInfo);
                }
            }
        }

        /// <inheritdoc />
        public void ApplyUpdate(UpdateCheckInfo updateCheckInfo)
        {
            Log.Info().WriteLine("Applying update.");
            var updated = ApplicationDeployment.CurrentDeployment.Update();

            if (updated)
            {
                Log.Info().WriteLine("Application succesfully updated.");
                if (HandleClickOnceRestarts != null)
                {
                    // Have the application the need for a restart
                    HandleClickOnceRestarts?.HandleRestart(updateCheckInfo);
                    return;
                }
                HandleRestart(updateCheckInfo);
            }
            else
            {
                Log.Warn().WriteLine("Application could not be updated.");
            }
        }

        /// <summary>
        /// Start the update check
        /// </summary>
        private UpdateCheckInfo CheckForUpdate()
        {
            if (!IsClickOnce)
            {
                return null;
            }
            // Make sure we don't check twice
            if (_isInCheck)
            {
                return null;
            }
            _isInCheck = true;
            try
            {
                Log.Debug().WriteLine("Checking for ClickOnce updates from {0}", ApplicationDeployment.CurrentDeployment.UpdateLocation);
                LastCheckedOn = DateTimeOffset.Now;
                return ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate(false);
            }
            catch (DeploymentDownloadException dde)
            {
                Log.Warn().WriteLine(dde, "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: ");
            }
            catch (InvalidDeploymentException ide)
            {
                // TODO: Handle this with an event or something
                Log.Error().WriteLine(ide, "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: ");
            }
            catch (InvalidOperationException ioe)
            {
                Log.Warn().WriteLine(ioe, "This application cannot be updated. It is likely not a ClickOnce application. Error: ");
            }
            catch (Exception ex)
            {
                Log.Error().WriteLine(ex, "Problem checking for ClickOnce updates: ");
            }
            finally
            {
                _isInCheck = false;
            }
            return null;
        }

        /// <inheritdoc />
        public void Restart()
        {
            Log.Info().WriteLine("Restarting application.");

            // TODO: This should be replaced by a better, non System.Windows.Forms.dll, implementation?
            // Note: CorLaunchApplication is deprecated, haven't been able to find a replacement yet.
            StartupShutdownBootstrapper.CancelStartup();
            StartupShutdownBootstrapper.RegisterForDisposal(Disposable.Create(System.Windows.Forms.Application.Restart));
            Application.Current.Shutdown();
        }

        /// <inheritdoc />
        public void HandleRestart(UpdateCheckInfo updateCheckInfo)
        {
            if (!ClickOnceConfiguration.AutoRestart)
            {
                return;
            }
            Restart();
        }
    }
}
