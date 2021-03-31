// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Deployment.Application;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// If you want to be apply the ClickOnce update yourself, implement this and export your class as typeof(IApplyClickOnceUpdates)
    /// </summary>
    public interface IApplyClickOnceUpdates
    {
        /// <summary>
        /// Apply the update
        /// </summary>
        /// <param name="updateCheckInfo"></param>
        void ApplyUpdate(UpdateCheckInfo updateCheckInfo);
    }
}
