using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Config;
using Caliburn.Micro;
using HorizontalAlignment = AdaptiveCards.HorizontalAlignment;

namespace Dapplo.CaliburnMicro.Cards.ViewModels
{
    /// <summary>
    /// This can display an Active-Card, 
    /// See the active-card <a href="http://adaptivecards.io/documentation/#display-libraries-wpf">WPF library</a>
    /// </summary>
    [Export]
    public sealed class AdaptiveCardViewModel : Screen, IHaveSettings, IHandle<AdaptiveCard>
    {
        private FrameworkElement _card;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        public AdaptiveCardViewModel()
        {
#if DEBUG
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            // Some designtime example
            var card = new AdaptiveCard
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
            Handle(card);
#endif
        }

        [ImportingConstructor]
        public AdaptiveCardViewModel(
            IWindowManager windowManager,
            IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
            DisplayName = "Card";
        }

        /// <summary>
        /// The actual card
        /// </summary>
        public FrameworkElement Card
        {
            get { return _card; }
            set
            {
                _card = value;
                NotifyOfPropertyChange();
            }
        }

        private void OnMissingInput(object sender, MissingInputEventArgs args)
        {
            MessageBox.Show($"Required input {args.Input.Id} is missing.");
        }

        private void OnAction(object sender, ActionEventArgs e)
        {
            if (e.Action != null && e.Action is OpenUrlAction)
            {
                var openUrlAction = (OpenUrlAction)e.Action;
                Process.Start(openUrlAction.Url);
            }
            else if (e.Action != null && e.Action is ShowCardAction)
            {
                var showCardAction = (ShowCardAction)e.Action;
                // Call "ourselves"
                _eventAggregator.PublishOnUIThread(showCardAction.Card);
            }
            else if (e.Action != null && e.Action is SubmitAction)
            {
                var submitAction = (SubmitAction)e.Action;
                throw new NotSupportedException(submitAction.Title);
            }
            else if (e.Action != null && e.Action is HttpAction)
            {
                var httpAction = (HttpAction)e.Action;
                // action.Headers  has headers for HTTP operation
                // action.Body has content body
                // action.Method has method to use
                // action.Url has url to post to
                // TODO: use Dapplo.HttpExtensions
                throw new NotSupportedException(httpAction.Title);
            }
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
                }
            };

            var renderer = new XamlRenderer(hostConfig, new ResourceDictionary(), OnAction, OnMissingInput);
            Card = renderer.RenderAdaptiveCard(card);

            // Make sure it's active
            if (!IsActive)
            {
                _windowManager?.ShowWindow(this);
            }
        }

        public IDictionary<string, object> Settings { get; } = new Dictionary<string, object>
        {
            {"SizeToContent", SizeToContent.WidthAndHeight}
        };
    }
}
