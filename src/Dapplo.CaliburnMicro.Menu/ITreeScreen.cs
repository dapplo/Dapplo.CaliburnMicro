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

#region using

using Caliburn.Micro;

#endregion

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