// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Overlays.ViewModels;
using Application.Demo.OverlayAddon.Views;
using Dapplo.CaliburnMicro.Overlays;
using Dapplo.Windows.Input.Mouse;
using Dapplo.Windows.User32;

namespace Application.Demo.OverlayAddon.ViewModels
{
    [Overlay("demo")]
    public sealed class RulerOverlayViewModel : OverlayViewModel
    {
        private IDisposable _subscription;
        private RulerOverlayView _rulerView;
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

            var screenBounds = DisplayInfo.ScreenBounds;
            _hasChange = false;
            _rulerView.RulerLeft.Y1 = Y;
            _rulerView.RulerLeft.Y2 = Y;
            _rulerView.RulerLeft.X1 = screenBounds.Left;
            _rulerView.RulerLeft.X2 = X - 20;

            _rulerView.RulerRight.Y1 = Y;
            _rulerView.RulerRight.Y2 = Y;
            _rulerView.RulerRight.X1 = X + 20;
            _rulerView.RulerRight.X2 = screenBounds.Right;
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
