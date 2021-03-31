// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Overlays.ViewModels
{
    /// <summary>
    /// This is the view model which will display all IOverlay items.
    /// If you want, you can have the overlay extend IActivate to get called when it's activated.
    /// By default the overlay takes the whole screen.
    /// </summary>
    public class OverlayViewModel : Screen, IOverlay
    {
        private bool _isEnabled = true;
        private bool _isVisible = true;
        private bool _isHittestable = true;
        private double _left;
        private double _top;


        /// <inheritdoc />
        public virtual bool IsHittestable
        {
            get => _isHittestable;
            set
            {
                _isHittestable = value;
                NotifyOfPropertyChange();
            }
        }


        /// <inheritdoc />
        public virtual double Left
        {
            get => _left;
            set
            {
                _left = value;
                NotifyOfPropertyChange();
            }
        }

        /// <inheritdoc />
        public virtual double Top
        {
            get => _top;
            set
            {
                _top = value;
                NotifyOfPropertyChange();
            }
        }


        /// <inheritdoc />
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            protected set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        /// <inheritdoc />
        public virtual bool IsVisible
        {
            get => _isVisible;
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(nameof(IsVisible));
            }
        }
    }
}
