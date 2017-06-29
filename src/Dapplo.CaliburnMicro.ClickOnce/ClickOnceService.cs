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
using System.Reflection;
using System.Windows;
using Dapplo.Addons;
using Dapplo.CaliburnMicro.ClickOnce.Configuration;
using Dapplo.CaliburnMicro.Diagnostics;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// This StartupAction takes care of managing ClickOnce applications
    /// </summary>
    [StartupAction]
    [Export(typeof(IClickOnceInformation))]
    [Export(typeof(IVersionProvider))]
    public class ClickOnceService : IStartupAction, IClickOnceInformation, IHandleClickOnceUpdates, IApplyClickOnceUpdates, IHandleClickOnceRestarts
    {
        private static readonly LogSource Log = new LogSource();
        private bool _isInCheck;

        [Import]
        private IClickOnceConfiguration ClickOnceConfiguration { get; set; }

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
            updateObservable?.Select(l => CheckForUpdate())
                .Where(updateCheckInfo => updateCheckInfo != null)
                .Subscribe(HandleUpdateCheck);
        }

        /// <inheritdoc />
        public DateTimeOffset LastCheckedOn { get; private set; }

        /// <inheritdoc />
        public bool IsClickOnce { get; } = ApplicationDeployment.IsNetworkDeployed;

        /// <inheritdoc />
        public Version CurrentVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        /// <inheritdoc />
        public Version LatestVersion { get; private set; } = Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Process the update check
        /// </summary>
        /// <param name="updateCheckInfo">UpdateCheckInfo</param>
        public void HandleUpdateCheck(UpdateCheckInfo updateCheckInfo)
        {
            if (!updateCheckInfo.UpdateAvailable)
            {
                Log.Debug().WriteLine("No update available.");
                return;
            }
            Log.Info().WriteLine("An updated version of the application is available: {0}", updateCheckInfo.AvailableVersion);
            LatestVersion = updateCheckInfo.AvailableVersion;

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

        /// <summary>
        /// Apply an update
        /// </summary>
        /// <param name="updateCheckInfo">UpdateCheckInfo</param>
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
                Log.Debug().WriteLine("Checking for ClickOnce updates.");
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

        /// <summary>
        /// Restart the current application
        /// </summary>
        public void HandleRestart(UpdateCheckInfo updateCheckInfo)
        {
            if (!ClickOnceConfiguration.AutoRestart)
            {
                return;
            }
            Log.Info().WriteLine("Automatically restarting.");
            // TODO: This should be replaced by a better, non System.Windows.Forms.dll, implementation?
            // Note: CorLaunchApplication is deprecated, haven't been able to find a replacement yet.
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }
    }
}
