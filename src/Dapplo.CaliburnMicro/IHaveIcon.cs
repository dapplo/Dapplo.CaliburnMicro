// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Controls;

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     Implement this interface to have an icon visible
    /// </summary>
    public interface IHaveIcon
    {
        /// <summary>
        ///     The icon, which can be used when the element is displayed.
        /// </summary>
        Control Icon { get; }
    }
}