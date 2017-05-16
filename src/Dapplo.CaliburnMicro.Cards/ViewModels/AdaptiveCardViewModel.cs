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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Config;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Cards.Entities;
using Dapplo.CaliburnMicro.Toasts.ViewModels;
using HorizontalAlignment = AdaptiveCards.HorizontalAlignment;
using Dapplo.CaliburnMicro.Dapp;
using Dapplo.HttpExtensions;
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
            // Some designtime example
            Card = new AdaptiveCard
            {
                Body = new List<CardElement>
                {
                    new Image
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Size = ImageSize.Small,
                        Url = "http://static.nichtlustig.de/comics/full/150422.jpg"
                    }
                },
                Actions = new List<ActionBase>
                {
                    new ShowCardAction
                    {
                        Title = "Do you like this?",
                        Card = new AdaptiveCard
                        {
                            Body = new List<CardElement>
                            {
                                new TextInput
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
        /// <param name="adaptiveCard"></param>
        public AdaptiveCardViewModel(AdaptiveCard adaptiveCard)
        {
            Card = adaptiveCard;
        }

        /// <summary>
        /// Property for the AdaptiveCard
        /// </summary>
        public AdaptiveCard Card
        {
            get
            {
                return _adaptiveCard;
            }
            set
            {
                _adaptiveCard = value;
                var hostConfig = new HostConfig
                {
                    FontSizes = {
                        Small = 15,
                        Normal =20,
                        Medium = 25,
                        Large = 30,
                        ExtraLarge= 40
                    },
                    ImageSizes =
                    {
                        Large = 100,
                        Medium = 70,
                        Small = 40
                    }

                };

                var renderer = new XamlRenderer(hostConfig, new ResourceDictionary(), OnAction, OnMissingInput);
                RenderedCard = renderer.RenderAdaptiveCard(_adaptiveCard);
            }
        }

        /// <summary>
        /// The actual card
        /// </summary>
        public virtual FrameworkElement RenderedCard
        {
            get { return _card; }
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
            MessageBox.Show($"Required input {args.Input.Id} is missing.");
        }

        /// <summary>
        /// Called when an action on a card is invoked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ActionEventArgs</param>
        /// <exception cref="NotSupportedException">Thrown when an action is not supported</exception>
        public virtual async void OnAction(object sender, ActionEventArgs e)
        {
            if (e.Action == null)
            {
                return;
            }
            var openUrlAction = e.Action as OpenUrlAction;
            if (openUrlAction != null)
            {
                Process.Start(openUrlAction.Url);
                return;
            }

            var showCardAction = e.Action as ShowCardAction;
            if (showCardAction != null)
            {
                // TODO: Currently kinda a hack
                var eventAggregator = Dapplication.Current.Bootstrapper.GetExport<IEventAggregator>().Value;
                eventAggregator.BeginPublishOnUIThread(new AdaptiveCardViewModel(showCardAction.Card));
                return;
            }

            var submitAction = e.Action as SubmitAction;
            if (submitAction != null)
            {
                // TODO: submit how / where?
                throw new NotSupportedException(submitAction.Title);
            }

            var httpAction = e.Action as HttpAction;
            if (httpAction != null)
            {
                var httpActionRequest = new HttpActionRequest
                {
                    Body = httpAction.Body
                    // TODO: How to unpack the headers?
                    // httpAction.Headers has headers for HTTP operation
                };
                var uri = new Uri(httpAction.Url);
                switch (httpAction.Method)
                {
                    case "POST":
                        var postResponse = await uri.PostAsync<HttpResponse<string>>(httpActionRequest);
                        Log.Debug().WriteLine("Response: {0}", postResponse.Response);
                        break;
                    case "GET":
                        var getResponse = await uri.GetAsAsync<HttpResponse<string>>();
                        Log.Debug().WriteLine("Response: {0}", getResponse.Response);
                        break;
                    default:
                        throw new NotSupportedException($"{httpAction.Title} - {httpAction.Method}");
                }
            }
        }

    }
}
