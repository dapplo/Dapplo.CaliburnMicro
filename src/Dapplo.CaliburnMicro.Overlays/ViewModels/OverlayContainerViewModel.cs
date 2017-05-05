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

using System.Diagnostics.CodeAnalysis;
using Caliburn.Micro;
using Dapplo.Windows.User32.Structs;

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

        protected override void OnActivate()
        {
            var bounds = DisplayInfo.GetAllScreenBounds();
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
            base.OnActivate();
        }

        #region Location

        /// <summary>
        ///     Returns if the Left of the displays
        /// </summary>
        public virtual double Left
        {
            get
            {
                return _left;
            }
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
            get
            {
                return _top;
            }
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
            get
            {
                return _width;
            }
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
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion


        #region IAmDisplayable

        /// <summary>
        ///     Returns if the OverlayViewModel can be selected
        /// </summary>
        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
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
            get { return _isVisible; }
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(nameof(IsVisible));
            }
        }

        #endregion
    }
}
