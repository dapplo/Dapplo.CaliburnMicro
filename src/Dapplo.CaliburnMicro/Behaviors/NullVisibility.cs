//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System.Windows;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     Change the visibility of a UiElement depending on a target value
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    public static class NullVisibility
    {
        /// <summary>
        ///     The value to check
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(object),
            typeof(NullVisibility),
            new PropertyMetadata(true, OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the value is null
        ///     Default is Visibility.Collapsed
        /// </summary>
        public static readonly DependencyProperty WhenNullProperty = DependencyProperty.RegisterAttached(
            "WhenNull",
            typeof(Visibility),
            typeof(NullVisibility),
            new PropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the value is not null
        ///     Default is Visibility.Visible
        /// </summary>
        public static readonly DependencyProperty WhenNotNullProperty = DependencyProperty.RegisterAttached(
            "WhenNotNull",
            typeof(Visibility),
            typeof(NullVisibility),
            new PropertyMetadata(Visibility.Visible, OnArgumentsChanged));

        private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(host => new NullVisibilityBehavior((UIElement) host));

        /// <summary>
        ///     Returns the value from the UIElement of the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>object which represents the value of the in ValueProperty specified DependencyProperty</returns>
        public static object GetValue(UIElement uiElement)
        {
            return uiElement.GetValue(ValueProperty);
        }

        /// <summary>
        ///     Returns the visibility which is used when the value is not null
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenNotNull(UIElement uiElement)
        {
            return (Visibility) uiElement.GetValue(WhenNotNullProperty);
        }

        /// <summary>
        ///     Returns the visibility which is used when the value is null
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenNull(UIElement uiElement)
        {
            return (Visibility) uiElement.GetValue(WhenNullProperty);
        }

        private static void OnArgumentsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            Behavior.Update(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        /// <summary>
        ///     Sets the value of the UIElement to the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="value">Value to assign to the in ValueProperty specified DependencyProperty</param>
        public static void SetValue(UIElement uiElement, object value)
        {
            uiElement.SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Sets the visibility which is used when the value is not null
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenNotNull(UIElement uiElement, Visibility visibility)
        {
            uiElement.SetValue(WhenNotNullProperty, visibility);
        }

        /// <summary>
        ///     Sets the visibility which is used when the value is null
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenNull(UIElement uiElement, Visibility visibility)
        {
            uiElement.SetValue(WhenNullProperty, visibility);
        }

        /// <summary>
        ///     Implementation of the NullVisibilityBehavior
        /// </summary>
        private sealed class NullVisibilityBehavior : Behavior<UIElement>
        {
            internal NullVisibilityBehavior(UIElement uiElement) : base(uiElement)
            {
            }

            protected override void Update(UIElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                uiElement.Visibility = GetValue(uiElement) == null
                    ? GetWhenNull(uiElement)
                    : GetWhenNotNull(uiElement);
            }
        }
    }
}