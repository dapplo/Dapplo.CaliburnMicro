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
    ///     Change the visibility of a UIElement depending on an enum value
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    public static class EnumVisibility
    {
        /// <summary>
        ///     The value to check
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(object),
            typeof(EnumVisibility),
            new PropertyMetadata(OnArgumentsChanged));

        /// <summary>
        ///     Possible value(s) which the target should have to match
        /// </summary>
        public static readonly DependencyProperty TargetValueProperty = DependencyProperty.RegisterAttached(
            "TargetValue",
            typeof(string),
            typeof(EnumVisibility),
            new PropertyMetadata(OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the value matches
        ///     Default is Visibility.Visible
        /// </summary>
        public static readonly DependencyProperty WhenMatchedProperty = DependencyProperty.RegisterAttached(
            "WhenMatched",
            typeof(Visibility),
            typeof(EnumVisibility),
            new FrameworkPropertyMetadata(Visibility.Visible, OnArgumentsChanged));

        /// <summary>
        ///     Visibility to use when the value doesn't match
        ///     Default is Visibility.Collapsed
        /// </summary>
        public static readonly DependencyProperty WhenNotMatchedProperty = DependencyProperty.RegisterAttached(
            "WhenNotMatched",
            typeof(Visibility),
            typeof(EnumVisibility),
            new FrameworkPropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

        private static readonly AttachedBehavior Behavior = AttachedBehavior.Register(host => new EnumVisibilityBehavior((UIElement) host));

        /// <summary>
        ///     Returns the value from the UIElement of the DependencyProperty specified in TargetValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>object which represents the value of the in TargetValueProperty specified DependencyProperty</returns>
        public static string GetTargetValue(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);
            return (string) uiElement.GetValue(TargetValueProperty);
        }

        /// <summary>
        ///     Returns the value from the UIElement of the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>object which represents the value of the in ValueProperty specified DependencyProperty</returns>
        public static object GetValue(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);
            return uiElement.GetValue(ValueProperty);
        }

        /// <summary>
        ///     Returns the value which is used when the enum value matches
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenMatched(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);
            return (Visibility) uiElement.GetValue(WhenMatchedProperty);
        }

        /// <summary>
        ///     Returns the value which is used when the enum value doesn't match
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>Visibility</returns>
        public static Visibility GetWhenNotMatched(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);
            return (Visibility) uiElement.GetValue(WhenNotMatchedProperty);
        }

        /// <summary>
        ///     When the arguments change, the Behavior.Update is called
        /// </summary>
        /// <param name="dependencyObject">DependencyObject</param>
        /// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
        private static void OnArgumentsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            Behavior.Update(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        /// <summary>
        ///     Sets the value of the UIElement to the DependencyProperty specified in TargetValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="value">Value to assign to the in TargetValueProperty specified DependencyProperty</param>
        public static void SetTargetValue(UIElement uiElement, string value)
        {
            Contract.Requires(uiElement != null);
            uiElement.SetValue(TargetValueProperty, value);
        }

        /// <summary>
        ///     Sets the value of the UIElement to the DependencyProperty specified in ValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="value">Value to assign to the in ValueProperty specified DependencyProperty</param>
        public static void SetValue(UIElement uiElement, object value)
        {
            Contract.Requires(uiElement != null);
            uiElement.SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Sets the value which is used when the enum value matches
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenMatched(UIElement uiElement, Visibility visibility)
        {
            Contract.Requires(uiElement != null);
            uiElement.SetValue(WhenMatchedProperty, visibility);
        }

        /// <summary>
        ///     Sets the value which is used when the enum value doesn't match
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <param name="visibility">Visibility to use when matched</param>
        public static void SetWhenNotMatched(UIElement uiElement, Visibility visibility)
        {
            Contract.Requires(uiElement != null);
            uiElement.SetValue(WhenNotMatchedProperty, visibility);
        }


        /// <summary>
        ///     Implementation of the actual behavior logic of the EnumVisibilityBehavior
        /// </summary>
        private sealed class EnumVisibilityBehavior : Behavior<UIElement>
        {
            private readonly ValueChecker<object> _enumCheck = new ValueChecker<object>();

            internal EnumVisibilityBehavior(UIElement uiElement) : base(uiElement)
            {
            }

            protected override void Update(UIElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                _enumCheck.Update(GetValue(uiElement), GetTargetValue(uiElement));

                uiElement.Visibility = _enumCheck.IsMatch ? GetWhenMatched(uiElement) : GetWhenNotMatched(uiElement);
            }
        }
    }
}