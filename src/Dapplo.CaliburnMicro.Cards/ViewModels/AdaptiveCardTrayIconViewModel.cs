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
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls.Primitives;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Config;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.NotifyIconWpf.ViewModels;

namespace Dapplo.CaliburnMicro.Cards.ViewModels
{
    /// <summary>
    /// This service takes care of showing the adaptive cards as a "trayicon" balloon.
    /// </summary>
    public class AdaptiveCardTrayIconViewModel : TrayIconViewModel, IHandle<AdaptiveCard>
    {
        [Import]
        private IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Handle the fact that the ViewModel is activated.
        /// Subscribe to the AdaptiveCard event
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            EventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Handle the fact that the ViewModel is deactivated
        /// And unsubscribe the AdaptiveCard event
        /// </summary>
        /// <param name="close"></param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            EventAggregator.Unsubscribe(this);
        }

        /// <summary>
        /// Handle an AdaptiveCard message
        /// </summary>
        /// <param name="card">AdaptiveCard</param>
        public void Handle(AdaptiveCard card)
        {
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

            var adaptiveCardViewModel = new AdaptiveCardViewModel();

            var renderer = new XamlRenderer(hostConfig, new ResourceDictionary(), adaptiveCardViewModel.OnAction, adaptiveCardViewModel.OnMissingInput);
            adaptiveCardViewModel.Card = renderer.RenderAdaptiveCard(card);

            TrayIcon.ShowBalloonTip(adaptiveCardViewModel, PopupAnimation.Scroll, TimeSpan.FromSeconds(8));
        }
    }
}
