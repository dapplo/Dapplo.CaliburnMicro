//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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

using System;

#if DEBUG
using System.Collections.Generic;
using System.ComponentModel;
#endif

using System.Diagnostics;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Wpf;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Toasts.ViewModels;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro.Cards.ViewModels
{
    /// <summary>
    /// This can display an Active-Card, 
    /// See the active-card <a href="http://adaptivecards.io/documentation/#display-libraries-wpf">WPF library</a>
    /// </summary>
    public class AdaptiveCardViewModel : ToastBaseViewModel
    {
        private static readonly LogSource Log = new LogSource();
        private FrameworkElement _card;
        private AdaptiveCard _adaptiveCard;
        private readonly AdaptiveHostConfig _adaptiveHostConfig;
        private readonly IEventAggregator _eventAggregator;

#if DEBUG
        /// <summary>
        /// A constructor which helps the designer.
        /// </summary>
        public AdaptiveCardViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            // Some design-time example
            Card = new AdaptiveCard("1.1")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveImage
                    {
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                        Size = AdaptiveImageSize.Small,
                        Url = new Uri("http://static.nichtlustig.de/comics/full/150422.jpg")
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveShowCardAction
                    {
                        Title = "Do you like this?",
                        Card = new AdaptiveCard("1.1")
                        {
                            Body = new List<AdaptiveElement>
                            {
                                new AdaptiveTextInput
                                {
                                    Id = "rating"
                                }
                            }
                        }
                    }
                }
            };
        }
#endif

        /// <summary>
        /// Constructor with AdaptiveCard
        /// </summary>
        /// <param name="adaptiveHostConfig">AdaptiveHostConfig</param>
        /// <param name="adaptiveCard"></param>
        /// <param name="eventAggregator">Optional IEventAggregator for when the AdaptiveCard has a ShowCardAction</param>
        public AdaptiveCardViewModel(AdaptiveHostConfig adaptiveHostConfig, AdaptiveCard adaptiveCard, IEventAggregator eventAggregator = null)
        {
            _adaptiveHostConfig = adaptiveHostConfig;
            _eventAggregator = eventAggregator;
            Card = adaptiveCard;
        }

        /// <summary>
        /// Property for the AdaptiveCard
        /// </summary>
        public AdaptiveCard Card
        {
            get => _adaptiveCard;
            set
            {
                _adaptiveCard = value;
                var renderer = new AdaptiveCardRenderer(_adaptiveHostConfig);
                var renderedAdaptiveCard = renderer.RenderCard(_adaptiveCard);
                foreach (var warning in renderedAdaptiveCard.Warnings)
                {
                    Log.Warn().WriteLine(warning.Message);
                }
                RenderedCard = renderedAdaptiveCard.FrameworkElement;
            }
        }

        /// <summary>
        /// The actual card
        /// </summary>
        public virtual FrameworkElement RenderedCard
        {
            get => _card;
            set
            {
                _card = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Called when the AdaptiveCard is missing input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public virtual void OnMissingInput(object sender, MissingInputEventArgs args)
        {
            MessageBox.Show($"Required input {args.AdaptiveInput.Id} is missing.");
        }

        /// <summary>
        /// Called when an action on a card is invoked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ActionEventArgs</param>
        /// <exception cref="NotSupportedException">Thrown when an action is not supported</exception>
        public virtual void OnAction(object sender, AdaptiveActionEventArgs e)
        {
            switch (e.Action)
            {
                case null:
                    return;
                case AdaptiveOpenUrlAction openUrlAction:
                    Process.Start(openUrlAction.Url.AbsoluteUri);
                    return;
                case AdaptiveShowCardAction showCardAction:
                    _eventAggregator.BeginPublishOnUIThread(new AdaptiveCardViewModel(_adaptiveHostConfig, showCardAction.Card, _eventAggregator));
                    return;
                case AdaptiveSubmitAction submitAction:
                    // TODO: submit how / where?
                    throw new NotSupportedException(submitAction.Title);
            }
        }

    }
}
