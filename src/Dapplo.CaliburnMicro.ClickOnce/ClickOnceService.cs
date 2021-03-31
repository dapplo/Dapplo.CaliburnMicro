﻿// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Deployment.Application;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Autofac.Features.AttributeFilters;
using Caliburn.Micro;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.ClickOnce.Configuration;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// This StartupAction takes care of managing ClickOnce applications
    /// </summary>
    [Service(nameof(ClickOnceService), nameof(CaliburnServices.ConfigurationService), TaskSchedulerName = "ui")]
    public class ClickOnceService : PropertyChangedBase, IStartup, IClickOnceService, IHandleClickOnceUpdates, IApplyClickOnceUpdates, IHandleClickOnceRestarts
    {
        private static readonly LogSource Log = new LogSource();
        private bool _isInCheck;
        private Version _currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private Version _latestVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private bool _isUpdateAvailable;
        private DateTimeOffset _lastCheckedOn;

        private readonly IClickOnceConfiguration _clickOnceConfiguration;

        private readonly SynchronizationContext _uiSynchronizationContext;

        private readonly IHandleClickOnceUpdates _handleClickOnceUpdates;

        private readonly IHandleClickOnceRestarts _handleClickOnceRestarts;

        private readonly IApplyClickOnceUpdates _applyClickOnceUpdates;

        /// <inheritdoc />
        public ClickOnceService(
            IClickOnceConfiguration clickOnceConfiguration,
            [KeyFilter("ui")]SynchronizationContext synchronizationContext,
            IHandleClickOnceUpdates handleClickOnceUpdates = null,
            IHandleClickOnceRestarts handleClickOnceRestarts = null,
            IApplyClickOnceUpdates applyClickOnceUpdates = null
            )
        {
            _uiSynchronizationContext = synchronizationContext;
            _handleClickOnceUpdates = handleClickOnceUpdates;
            _handleClickOnceRestarts = handleClickOnceRestarts;
            _applyClickOnceUpdates = applyClickOnceUpdates;
            _clickOnceConfiguration = clickOnceConfiguration;
        }

        /// <inheritdoc />
        public void Startup()
        {
            if (!IsClickOnce)
            {
                Log.Info().WriteLine("Application is not deployed via ClickOnce, there will be no checks for updates.");
                return;
            }
            Log.Info().WriteLine("Configuring ClickOnce update handling.");

            CurrentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;

            IObservable <long> updateObservable = null;

            var checkInBackground = _clickOnceConfiguration.EnableBackgroundUpdateCheck && _clickOnceConfiguration.CheckInterval > 0;
            if (checkInBackground)
            {
                // Check in background
                updateObservable = Observable.Interval(TimeSpan.FromMinutes(_clickOnceConfiguration.CheckInterval));
            }

            if (_clickOnceConfiguration.CheckOnStart)
            {
                Log.Info().WriteLine("Starting application update check.");
                var updateCheckInfo = CheckForUpdate();
                HandleUpdateCheck(updateCheckInfo);
            }
            // Register the check, if there is an update observable
            updateObservable?.ObserveOn(_uiSynchronizationContext).Select(l => CheckForUpdate())
                .Where(updateCheckInfo => updateCheckInfo != null)
                .Subscribe(HandleUpdateCheck);
        }

        /// <inheritdoc />
        public DateTimeOffset LastCheckedOn
        {
            get => _lastCheckedOn;
            private set
            {
                _lastCheckedOn = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public bool IsUpdateAvailable
        {
            get => _isUpdateAvailable;
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
            get => _currentVersion;
            private set
            {
                _currentVersion = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public Version LatestVersion
        {
            get => _latestVersion;
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

            if (_handleClickOnceUpdates != null)
            {
                // Have the application handle the check
                _handleClickOnceUpdates.HandleUpdateCheck(updateCheckInfo);
            }
            else
            {
                if (_applyClickOnceUpdates == null && (updateCheckInfo.IsUpdateRequired || _clickOnceConfiguration.AutoUpdate))
                {
                    // "Force" update
                    ApplyUpdate(updateCheckInfo);
                }
                else
                {
                    // Have the application handle the update
                    _applyClickOnceUpdates?.ApplyUpdate(updateCheckInfo);
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
                if (_handleClickOnceRestarts != null)
                {
                    // Have the application the need for a restart
                    _handleClickOnceRestarts?.HandleRestart(updateCheckInfo);
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
            // TODO: Test this
            // TODO: This should be replaced by a better, non System.Windows.Forms.dll, implementation?
            // Note: CorLaunchApplication is deprecated, haven't been able to find a replacement yet.
            Application.Current.Exit += (sender, args) => { System.Windows.Forms.Application.Restart(); };
            // The shutdown needs to be on the UI thread, otherwise we can't release resources etc.
            Execute.BeginOnUIThread(Application.Current.Shutdown);
        }

        /// <inheritdoc />
        public void HandleRestart(UpdateCheckInfo updateCheckInfo)
        {
            if (!_clickOnceConfiguration.AutoRestart)
            {
                return;
            }
            Restart();
        }
    }
}
