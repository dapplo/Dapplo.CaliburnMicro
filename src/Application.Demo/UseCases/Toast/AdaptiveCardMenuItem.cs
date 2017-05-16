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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using AdaptiveCards;
using Application.Demo.Languages;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Cards.ViewModels;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.CaliburnMicro.Security;
using Dapplo.Log;
using MahApps.Metro.IconPacks;
using HorizontalAlignment = AdaptiveCards.HorizontalAlignment;

#endregion

namespace Application.Demo.UseCases.Toast
{
    /// <summary>
    ///     This provides the IMenuItem to open the WindowWithMenuViewModel
    /// </summary>
    [Export("contextmenu", typeof(IMenuItem))]
    public sealed class AdaptiveCardMenuItem : AuthenticatedMenuItem<Visibility>
    {
        private static readonly LogSource Log = new LogSource();

        [ImportingConstructor]
        public AdaptiveCardMenuItem(
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

                var card = new AdaptiveCard
                {
                    Body = new List<CardElement>
                    {
                        new Image
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Size = ImageSize.Large,
                            Url = "http://static.nichtlustig.de/comics/full/150422.jpg"
                        },
                        new Image
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Size = ImageSize.Large,
                            Url = "http://static.nichtlustig.de/comics/full/150421.jpg"
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
                        },
                        new HttpAction
                        {
                            Body = "Testing 1 2 3",
                            Url = "https://httpbin.org/post",
                            Title = "Call HTTP Bin"
                        }
                    }
                };

                eventAggregator.PublishOnCurrentThread(new AdaptiveCardViewModel(card));
            };

            this.VisibleOnPermissions("Admin");

        }
    }
}