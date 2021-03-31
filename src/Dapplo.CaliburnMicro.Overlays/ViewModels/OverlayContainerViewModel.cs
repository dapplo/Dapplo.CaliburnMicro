// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Caliburn.Micro;
using Dapplo.Windows.User32;

namespace Dapplo.CaliburnMicro.Overlays.ViewModels
{
    /// <summary>
    /// This is the view model which will display all IOverlay items.
    /// If you want, you can have the overlay extend IActivate to get called when it's activated.
    /// By default the overlay takes the whole screen.
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public class OverlayContainerViewModel : Conductor<IOverlay>.Collection.AllActive, IAmDisplayable
    {
        private bool _isEnabled = true;
        private bool _isVisible = true;
        private double _left;
        private double _top;
        private double _width;
        private double _height;

        /// <inheritdoc />
        protected override void OnActivate()
        {
            var bounds = DisplayInfo.ScreenBounds;
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
            base.OnActivate();
        }

        /// <summary>
        ///     Returns if the Left of the displays
        /// </summary>
        public virtual double Left
        {
            get => _left;
            set
            {
                _left = value;
                NotifyOfPropertyChange();
            }
        } 

        /// <summary>
        ///     Returns if the Top of the displays
        /// </summary>
        public virtual double Top
        {
            get => _top;
            set
            {
                _top = value;
                NotifyOfPropertyChange();
            }
        }


        /// <summary>
        ///     Returns if the Width of the displays
        /// </summary>
        public virtual double Width
        {
            get => _width;
            set
            {
                _width = value;
                NotifyOfPropertyChange();
            }
        }


        /// <summary>
        ///     Returns if the Height of the displays
        /// </summary>
        public virtual double Height
        {
            get => _height;
            set
            {
                _height = value;
                NotifyOfPropertyChange();
            }
        }


        /// <summary>
        ///     Returns if the OverlayViewModel can be selected
        /// </summary>
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            protected set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        /// <summary>
        ///     Returns if the OverlayViewModel is visible
        /// </summary>
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
