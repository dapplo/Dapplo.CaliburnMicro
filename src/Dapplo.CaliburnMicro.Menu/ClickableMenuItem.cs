// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    /// Default type for the ClickableMenuItem
    /// </summary>
    public class ClickableMenuItem : ClickableMenuItem<IMenuItem>
    {
        
    }

    /// <summary>
    ///     Extend this to make your IMenuItem
    /// </summary>
    public class ClickableMenuItem<TClickArgument> : MenuItem, ICanBeClicked<TClickArgument>
    {
        /// <summary>
        ///     This action is called when Click is invoked
        /// </summary>
        public Action<TClickArgument> ClickAction { get; set; }

 
        /// <summary>
        ///     Is called when the IMenuItem is clicked
        /// </summary>
        public virtual void Click(TClickArgument clickedItem)
        {
            ClickAction?.Invoke(clickedItem);
        }
    }
}