// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using Application.Demo.Languages;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Cards.ViewModels;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using Dapplo.Log;
using MahApps.Metro.IconPacks;

namespace Application.Demo.UseCases.Toast
{
    /// <summary>
    ///     This provides the IMenuItem to open the WindowWithMenuViewModel
    /// </summary>
    [Menu("contextmenu")]
    public sealed class AdaptiveCardMenuItem : AuthenticatedMenuItem<IMenuItem, Visibility>
    {
        private static readonly LogSource Log = new LogSource();

        public AdaptiveCardMenuItem(AdaptiveHostConfig adaptiveHostConfig,
            IEventAggregator eventAggregator,
            IContextMenuTranslations contextMenuTranslations)
        {
            // automatically update the DisplayName
            contextMenuTranslations.CreateDisplayNameBinding(this, nameof(IContextMenuTranslations.ActiveCard));

            Icon = new PackIconMaterial
            {
                Kind = PackIconMaterialKind.Cards
            };


            ClickAction = clickedItem =>
            {
                Log.Debug().WriteLine("ActiveCard");

                var card = new AdaptiveCard("1.1")
                {
                    Body = new List<AdaptiveElement>
                    {
                        new AdaptiveImage
                        {
                            HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                            Size = AdaptiveImageSize.Large,
                            Url = new Uri("http://static.nichtlustig.de/comics/full/150422.jpg")
                        },
                        new AdaptiveImage
                        {
                            HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                            Size = AdaptiveImageSize.Large,
                            Url = new Uri("http://static.nichtlustig.de/comics/full/150421.jpg")
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

                eventAggregator.PublishOnCurrentThread(new AdaptiveCardViewModel(adaptiveHostConfig, card, eventAggregator));
            };

            this.VisibleOnPermissions("Admin");
        }
    }
}