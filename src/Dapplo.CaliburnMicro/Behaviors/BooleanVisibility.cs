//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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

using System.Diagnostics.Contracts;
using System.Windows;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     Change the visibility of a UIElement depending on a boolean
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    public static class BooleanVisibility
    {
        /// <summary>
        ///     The value to check
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(bool),
            typeof(BooleanVisibility),
            new PropertyMetadata(true, OnVisibilityChanged));

        /// <summary>
        ///     What needs to be done when true
        ///     Default is Visibility.Visible
        /// </summary>
        public static readonly DependencyProperty WhenTrueProperty = DependencyProperty.RegisterAttached(
            "WhenTrue",
            typeof(Visibility),
            typeof(BooleanVisibility),
            new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

        /// <summary>
        ///     What needs to be done when false
        ///     Default is Visibility.Collapsed
        /// </summary>
        public static readonly DependencyProperty WhenFalseProperty = DependencyProperty.RegisterAttached(
            "WhenFalse",
            typeof(Visibility),
            typeof(BooleanVisibility),
            new PropertyMetadata(Visibility.Collapsed, OnVisibilityChanged));

        private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(host => new BooleanVisibilityBehavior((UIElement) host));

        /// <summary>
        ///     Returns the value from the UIElement of the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>object which represents the value of the in ValueProperty specified DependencyProperty</returns>
        public static bool GetValue(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);

            return (bool) uiElement.GetValue(ValueProperty);
        }

        /// <summary>
        ///     Returns the value which is used when the boolean is false
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenFalse(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);

            return (Visibility) uiElement.GetValue(WhenFalseProperty);
        }

        /// <summary>
        ///     Returns the value which is used when the boolean is true
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenTrue(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);

            return (Visibility) uiElement.GetValue(WhenTrueProperty);
        }

        private static void OnVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            Behavior.Update(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        /// <summary>
        ///     Sets the value of the UIElement to the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="value">Value to assign to the in ValueProperty specified DependencyProperty</param>
        public static void SetValue(UIElement uiElement, bool value)
        {
            Contract.Requires(uiElement != null);

            uiElement.SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Sets the value which is used when the boolean is false
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenFalse(UIElement uiElement, Visibility visibility)
        {
            Contract.Requires(uiElement != null);

            uiElement.SetValue(WhenFalseProperty, visibility);
        }

        /// <summary>
        ///     Sets the value which is used when the boolean is true
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenTrue(UIElement uiElement, Visibility visibility)
        {
            Contract.Requires(uiElement != null);

            uiElement.SetValue(WhenTrueProperty, visibility);
        }

        /// <summary>
        ///     Implementation of the actual behavior logic of the BooleanVisibilityBehavior
        /// </summary>
        private sealed class BooleanVisibilityBehavior : Behavior<UIElement>
        {
            internal BooleanVisibilityBehavior(UIElement uiElement) : base(uiElement)
            {
                // Does not propagate external changes in visibility to binding source - that will be
                // covered in a future post
            }

            protected override void Update(UIElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                uiElement.Visibility = GetValue(uiElement) ? GetWhenTrue(uiElement) : GetWhenFalse(uiElement);
            }
        }
    }
}