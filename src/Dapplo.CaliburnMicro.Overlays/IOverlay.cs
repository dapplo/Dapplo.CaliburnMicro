// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro.Overlays
{
    /// <summary>
    /// Things marked with this interface overlay the screen
    /// </summary>
    public interface IOverlay : IAmDisplayable
    {
        /// <summary>
        /// X Location
        /// </summary>
        double Left { get; }

        /// <summary>
        /// Y Location
        /// </summary>
        double Top { get; }

        /// <summary>
        /// Specifies if the overlay is visible to hit-testing
        /// </summary>
        bool IsHittestable { get; }
    }
}
