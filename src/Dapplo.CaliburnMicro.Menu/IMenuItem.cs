// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     This defines an IMenuItem
    /// </summary>
    public interface IMenuItem : ITreeNode<IMenuItem>, INotifyPropertyChanged, IAmDisplayable, IHaveIcon
    {
        /// <summary>
        ///     A string which describes which hotkey the menu entry would respond to.
        ///     This does NOT implement the hotkey binding, it's only a hint
        /// </summary>
        string HotKeyHint { get; set; }

        /// <summary>
        ///     The initialize is called from the UI Thread before the menu-item is added to a context menu.
        ///     This allows for any initialization, like icons etc, to be made
        /// </summary>
        void Initialize();
    }
}