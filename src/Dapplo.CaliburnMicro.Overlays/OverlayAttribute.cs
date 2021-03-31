// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Composition;

namespace Dapplo.CaliburnMicro.Overlays
{
    /// <summary>
    ///     This attribute can be used to specify the overlay to use
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class OverlayAttribute : Attribute
    {
        /// <inheritdoc />
        public OverlayAttribute()
        {
        }

        /// <inheritdoc />
        public OverlayAttribute(string overlay)
        {
            Overlay = overlay;
        }

        /// <summary>
        ///     The overlay to use
        /// </summary>
        public string Overlay { get; set; }
    }
}