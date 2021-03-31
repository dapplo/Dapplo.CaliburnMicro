// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Deployment.Application;

namespace Dapplo.CaliburnMicro.ClickOnce
{
    /// <summary>
    /// If you want to handle a ClickOnce restart, e.g. by asking the user, implement this and export your class as typeof(IHandleClickOnceRestarts)
    /// </summary>
    public interface IHandleClickOnceRestarts
    {
        /// <summary>
        /// This is called after an update is applied, which usually should trigger a restart
        /// </summary>
        /// <param name="updateCheckInfo"></param>
        void HandleRestart(UpdateCheckInfo updateCheckInfo);
    }
}
