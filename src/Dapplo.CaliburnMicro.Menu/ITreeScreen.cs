// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     Implement this for elements that are visible in a tree
    ///     Some of the configuration functionality is covered in standard Caliburn.Micro interfaces
    ///     which are supplied by the interfaces which IScreen extends:
    ///     IHaveDisplayName: Covers the name and title of the config screen
    ///     INotifyPropertyChanged: Makes it possible that changes to e.g. the name are directly represented in the view
    ///     IActivate, IDeactivate: to know if the config screen is activated or deactivated
    ///     IGuardClose.CanClose: Prevents leaving the config screen
    ///     A default implementation is just to extend Screen from Caliburn.Micro
    ///     Additionally some Dapplo.CaliburnMicro Interfaces are used:
    ///     IAmDisplayable: Covers the visiblity and enabled (extends IHaveDisplayName)
    /// </summary>
    public interface ITreeScreen : IScreen, IAmDisplayable
    {
        /// <summary>
        ///     Returns if the ITreeScreen can be activated (when clicking on it)
        /// </summary>
        bool CanActivate { get; }
    }
}