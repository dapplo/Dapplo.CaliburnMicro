// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Features.AttributeFilters;
using Dapplo.CaliburnMicro.Overlays;
using Dapplo.CaliburnMicro.Overlays.ViewModels;

namespace Application.Demo.OverlayAddon.ViewModels
{
    /// <summary>
    /// This is the view model which will display all IOverlay items.
    /// If you want, you can have the overlay extend IActivate to get called when it's activated.
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class DemoOverlayContainerViewModel : OverlayContainerViewModel
    {
        private readonly IEnumerable<Lazy<IOverlay>> _overlays;

        public DemoOverlayContainerViewModel(
            [MetadataFilter("Overlay", "demo")]
            IEnumerable<Lazy<IOverlay>> overlays
        )
        {
            _overlays = overlays;
        }
        /// <summary>
        /// Make sure all the items are assigned
        /// </summary>
        protected override void OnActivate()
        {
            Items.AddRange(_overlays.Select(lazy => lazy.Value));
            base.OnActivate();
        }
    }
}
