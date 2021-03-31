// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using AdaptiveCards.Rendering.Wpf;

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
        void OnAction(object sender, AdaptiveActionEventArgs actionEventArgs);
    }
}
