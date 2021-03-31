// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    ///     Extensions to simplify the usage of IHaveIcon
    /// </summary>
    public static class HaveIconExtensions
    {
        /// <summary>
        ///     Apply the specified Brush as Foreground for the Icons in the IEnumerable with IHaveIcon
        /// </summary>
        /// <param name="hasIcons">IEnumerable with IHaveIcon</param>
        /// <param name="foregroundBrush">Brush for the Foreground</param>
        public static void ApplyIconForegroundColor(this IEnumerable<IHaveIcon> hasIcons, Brush foregroundBrush)
        {
            foreach (var haveIcon in hasIcons)
            {
                haveIcon.ApplyIconForegroundColor(foregroundBrush);
            }
        }

        /// <summary>
        ///     Apply the specified Brush as Foreground for the icon of the IHaveIcon
        /// </summary>
        /// <param name="haveIcon">IHaveIcon</param>
        /// <param name="foregroundBrush">Brush for the Foreground</param>
        public static void ApplyIconForegroundColor(this IHaveIcon haveIcon, Brush foregroundBrush)
        {
            if (haveIcon == null)
            {
                throw new ArgumentNullException(nameof(haveIcon));
            }
            if (haveIcon.Icon != null)
            {
                haveIcon.Icon.Foreground = foregroundBrush;
            }
        }

        /// <summary>
        ///     Apply the specified Thickness as margin to the Icons in the IEnumerable with IHaveIcon
        /// </summary>
        /// <param name="hasIcons">IEnumerable with IHaveIcon</param>
        /// <param name="thickness">Thickness for the marging</param>
        public static void ApplyIconMargin(this IEnumerable<IHaveIcon> hasIcons, Thickness thickness)
        {
            foreach (var menuItem in hasIcons)
            {
                menuItem.ApplyIconMargin(thickness);
            }
        }

        /// <summary>
        ///     Apply the specified Thickness as margin to the Icon in the IHaveIcon
        /// </summary>
        /// <param name="haveIcon">IHaveIcon</param>
        /// <param name="thickness">Thickness for the marging</param>
        public static void ApplyIconMargin(this IHaveIcon haveIcon, Thickness thickness)
        {
            if (haveIcon == null)
            {
                throw new ArgumentNullException(nameof(haveIcon));
            }
            if (haveIcon.Icon != null)
            {
                haveIcon.Icon.Margin = thickness;
            }
        }
    }
}