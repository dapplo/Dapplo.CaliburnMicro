//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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
using System.Windows;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Overlays.ViewModels;
using Dapplo.Windows.User32.Structs;
using Application.Demo.OverlayAddon.Views;
using Dapplo.CaliburnMicro.Overlays;
using Dapplo.Windows.Input.Mouse;

namespace Application.Demo.OverlayAddon.ViewModels
{
    [Overlay("demo")]
    public sealed class RulerOverlayViewModel : OverlayViewModel
    {
        private IDisposable _subscription;
        private RulerOverlayView _rulerView;
        private readonly Rect _screenbounds = DisplayInfo.GetAllScreenBounds();
        private bool _hasChange;
        protected override void OnActivate()
        {
            IsHittestable = false;
            IsEnabled = false;
            _subscription = MouseHook.MouseEvents.Subscribe(p =>
            {
                Y = p.Point.Y;
                X = p.Point.X;
                _hasChange = true;
            });

            CompositionTarget.Rendering += CompositionTargetOnRendering;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _rulerView = view as RulerOverlayView;
        }

        private void CompositionTargetOnRendering(object sender, EventArgs args)
        {
            if (_rulerView == null || !_hasChange)
            {
                return;
            }
            _hasChange = false;
            _rulerView.RulerLeft.Y1 = Y;
            _rulerView.RulerLeft.Y2 = Y;
            _rulerView.RulerLeft.X1 = _screenbounds.Left;
            _rulerView.RulerLeft.X2 = X - 20;

            _rulerView.RulerRight.Y1 = Y;
            _rulerView.RulerRight.Y2 = Y;
            _rulerView.RulerRight.X1 = X + 20;
            _rulerView.RulerRight.X2 = _screenbounds.Right;
        }

        protected override void OnDeactivate(bool close)
        {
            CompositionTarget.Rendering -= CompositionTargetOnRendering;
            _subscription?.Dispose();
            _subscription = null;
            base.OnDeactivate(close);
        }

        /// <summary>
        /// X Coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y Coordinate
        /// </summary>
        public int Y { get; private set; }
    }
}
