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
using System.Reactive.Linq;
using System.Windows;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.ClickOnce.Configuration;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// This StartupAction takes care of managing ClickOnce applications
    /// </summary>
    [StartupAction]
    [Export(typeof(IClickOnceInformation))]
    public class ClickOnceService : IStartupAction, IClickOnceInformation
    {
        private static readonly LogSource Log = new LogSource();
        private Version _clickOnceVersion;
        private bool _isInCheck;

        [Import]
        private IClickOnceConfiguration ClickOnceConfiguration { get; set; }

        /// <inheritdoc />
        public void Start()
        {
            if (!IsClickOnce)
            {
                Log.Info().WriteLine("Application is not deployed via ClickOnce, there will be no checks for updates.");
                return;
            }
            IObservable<long> updateObservable = null;

            var checkInBackground = ClickOnceConfiguration.EnableBackgroundUpdateCheck && ClickOnceConfiguration.CheckInterval > 0;
            if (!ClickOnceConfiguration.CheckOnStart && checkInBackground)
            {
                // Only in background
                updateObservable = Observable.Timer(TimeSpan.FromMinutes(ClickOnceConfiguration.CheckInterval));
            }
            else if  (ClickOnceConfiguration.CheckOnStart && ClickOnceConfiguration.CheckOnStart)
            {
                // On start and background
                updateObservable = Observable.Interval(TimeSpan.FromMinutes(ClickOnceConfiguration.CheckInterval));
            }
            else if (ClickOnceConfiguration.CheckOnStart)
            {
                // On start 
                updateObservable = Observable.Timer(TimeSpan.Zero);
            }
            // Register the check, if there is an update observable
            updateObservable?.Select(l => CheckForUpdate()).Where(updateCheckInfo => updateCheckInfo != null).Subscribe();
        }

        /// <inheritdoc />
        public bool IsClickOnce { get; } = ApplicationDeployment.IsNetworkDeployed;

        /// <inheritdoc />
        public Version CurrentVersion => _clickOnceVersion ?? (_clickOnceVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion);

        /// <inheritdoc />
        public Version LatestVersion { get; private set; }

        /// <summary>
        /// Process the update check
        /// </summary>
        /// <param name="updateCheckInfo">UpdateCheckInfo</param>
        private void HandleUpdateCheck(UpdateCheckInfo updateCheckInfo)
        {
            if (!updateCheckInfo.UpdateAvailable)
            {
                Log.Debug().WriteLine("No update available.");
                return;
            }
            LatestVersion = updateCheckInfo.AvailableVersion;

            if (updateCheckInfo.IsUpdateRequired || ClickOnceConfiguration.AutoUpdate)
            {
                // "Force" update
                Update();
            }
            // TODO: Have something handle the update request, if this is not automatically done.
        }

        /// <summary>
        /// Apply an update
        /// </summary>
        public void Update()
        {
            Log.Info().WriteLine("Applying update.");
            var updated = ApplicationDeployment.CurrentDeployment.Update();

            if (updated)
            {
                Log.Info().WriteLine("Application succesfully updated.");
                if (ClickOnceConfiguration.AutoRestart)
                {
                    Log.Info().WriteLine("Automatically restarting.");
                    RestartApplication();
                }
                // TODO: When to restart when auto-restart is not set?
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
                Log.Debug().WriteLine("Checking for ClickOnce updates.");
                return ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate(false);
            }
            catch (DeploymentDownloadException dde)
            {
                Log.Warn().WriteLine(dde, "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: ");
            }
            catch (InvalidDeploymentException ide)
            {
                // TODO: Handle this
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

        /// <summary>
        /// Restart the current application
        /// </summary>
        private static void RestartApplication()
        {
            // TODO: This should be replaced by a better, non System.Windows.Forms.dll, implementation?
            // Note: CorLaunchApplication is deprecated, haven't been able to find a replacement yet.
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }
    }
}
