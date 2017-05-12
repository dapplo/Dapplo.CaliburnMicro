## Dapplo.CaliburnMicro.Cards

This project makes it possible to show adaptive cards, as described here: http://adaptivecards.io, in form of a toast near the system tray.

You need to extend AdaptiveCardTrayIconViewModel, instead of TrayIconViewModel.

Use the IEventAggregator to send an AdaptiveCard, this than will be automatically shown.

