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

using System.ComponentModel;
using Caliburn.Micro;

#endregion

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     This defines an IMenuItem
    /// </summary>
    public interface IMenuItem : ITreeNode<IMenuItem>, INotifyPropertyChanged, IAmDisplayable, IHaveIcon, IHaveDisplayName
    {
        /// <summary>
        ///     A string which describes which hotkey the menu entry would respond to.
        ///     This does NOT implement the hotkey binding, it's only a hint
        /// </summary>
        string HotKeyHint { get; set; }

        /// <summary>
        ///     Is called when the IMenuItem it clicked
        /// </summary>
        void Click(IMenuItem clickedItem);

        /// <summary>
        ///     The initialize is called from the UI Thread before the menu-item is added to a context menu.
        ///     This allows for any initialization, like icons etc, to be made
        /// </summary>
        void Initialize();
    }
}