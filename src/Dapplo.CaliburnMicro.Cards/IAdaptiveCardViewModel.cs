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

using AdaptiveCards.Rendering;
using System.Windows;

namespace Dapplo.CaliburnMicro.Cards
{
    /// <summary>
    /// Your ViewModel needs to implement this interface to be able to host an adaptive card.
    /// </summary>
    public interface IAdaptiveCardViewModel
    {
        /// <summary>
        /// The card for the View
        /// </summary>
        FrameworkElement RenderedCard { get; set; }

        /// <summary>
        /// Handle missing input
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="missingInputEventArgs">MissingInputEventArgs</param>
        void OnMissingInput(object sender, MissingInputEventArgs missingInputEventArgs);

        /// <summary>
        /// Called when an action on a card is invoked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="actionEventArgs"></param>
        void OnAction(object sender, ActionEventArgs actionEventArgs);
    }
}
