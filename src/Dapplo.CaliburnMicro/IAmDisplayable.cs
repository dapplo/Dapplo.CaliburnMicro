// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     This interface is implemented by elements that can be displayed
    /// </summary>
    public interface IAmDisplayable
    {
        /// <summary>
        ///     Returns if the element can be selected (visible but not usable)
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        ///     Returns if the element is visible (not visible and not usable)
        /// </summary>
        bool IsVisible { get; }
    }
}