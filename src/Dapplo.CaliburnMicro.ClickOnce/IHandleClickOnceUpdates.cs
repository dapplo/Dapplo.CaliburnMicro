// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Deployment.Application;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// If you want to handle ClickOnce updates yourself, implement this and export your class as typeof(IHandleClickOnceUpdates)
    /// </summary>
    public interface IHandleClickOnceUpdates
    {
        /// <summary>
        /// Handle the update, this is usually handled by the ClickOnceService itself
        /// </summary>
        /// <param name="updateCheckInfo">UpdateCheckInfo</param>
        void HandleUpdateCheck(UpdateCheckInfo updateCheckInfo);
    }
}
