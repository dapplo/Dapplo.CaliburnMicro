## Dapplo.CaliburnMicro.Cards

This project makes it possible to show adaptive cards, as described here: http://adaptivecards.io, in form of a toast near the system tray.

You will need to:
1. instanciate an AdaptiveCard
2. get a reference to the ToastConductor
3. Call toastConductor.ActivateItem(new AdaptiveCardViewModel(card));

It's possible to bring your own style, by extending the AdaptiveCardViewModel & AdaptiveCardView with your own versions