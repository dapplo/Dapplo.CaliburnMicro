// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Log;
using Size = System.Windows.Size;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     This class contains a behavior to assist in setting an Icon of a e.g. Window or TaskbarIcon via a FrameworkElement
    /// </summary>
    public static class FrameworkElementIcon
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        ///     The icon to set
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(FrameworkElement),
            typeof(FrameworkElementIcon),
            new PropertyMetadata(OnArgumentsChanged));

        /// <summary>
        ///     What is the target property which should used for the icon
        /// </summary>
        public static readonly DependencyProperty TargetValueProperty = DependencyProperty.RegisterAttached(
            "TargetValue",
            typeof(string),
            typeof(FrameworkElementIcon),
            new PropertyMetadata("Icon", OnArgumentsChanged));

        /// <summary>
        ///     Use the IconBehavior
        /// </summary>
        private static AttachedBehavior Behavior { get; } = AttachedBehavior.Register(host => new IconBehavior((FrameworkElement) host));

        /// <summary>
        ///     Returns the value from the UIElement of the DependencyProperty specified in TargetValueProperty.
        /// </summary>
        /// <param name="uiElement">UIElement</param>
        /// <returns>string which represents the name of the target property</returns>
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
        public static FrameworkElement GetValue(UIElement uiElement)
        {
            Contract.Requires(uiElement != null);
            return (FrameworkElement) uiElement.GetValue(ValueProperty);
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
        public static void SetValue(UIElement uiElement, FrameworkElement value)
        {
            Contract.Requires(uiElement != null);
            uiElement.SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     Implementation of the actual behavior logic of the EnumVisibilityBehavior
        /// </summary>
        private sealed class IconBehavior : Behavior<FrameworkElement>
        {
            private readonly FrameworkElement _icon;

            internal IconBehavior(FrameworkElement uiElement) : base(uiElement)
            {
                _icon = GetValue(uiElement);
            }

            protected override void Update(FrameworkElement uiElement, DependencyPropertyChangedEventArgs? dependencyPropertyChangedEventArgs)
            {
                if (uiElement == null)
                {
                    return;
                }

                // For user controls, find the parent - parent
                while (typeof(UserControl).IsAssignableFrom(uiElement.GetType().BaseType))
                {
                    if (!(uiElement.Parent is FrameworkElement parentUiElement))
                    {
                        break;
                    }
                    uiElement = parentUiElement;
                }

                if (!uiElement.IsLoaded)
                {
                    // If the host is not loaded, wait until it is.
                    var target = uiElement.IsLoaded ? _icon : uiElement;
                    var handlers = new RoutedEventHandler[1];
                    handlers[0] = (sender, args) =>
                    {
                        Update(uiElement, dependencyPropertyChangedEventArgs);
                        target.Loaded -= handlers[0];
                    };
                    target.Loaded += handlers[0];
                    return;
                }

                var iconProperty = GetTargetValue(uiElement);
                var propertyInfo = uiElement.GetType().GetProperty(iconProperty);
                if (propertyInfo == null)
                {
                    return;
                }
                if (propertyInfo.PropertyType == typeof(ImageSource))
                {
                    // Icon is of type ImageSource
                    var image = propertyInfo.GetValue(uiElement) as ImageSource;
                    // Default size for the icon
                    var size = new Size(256, 256);
                    if (image != null)
                    {
                        size = new Size(image.Width, image.Height);
                    }
                    var iconIcon = _icon.ToBitmapSource(size);
                    propertyInfo.SetValue(uiElement, iconIcon);
                }
                else if (propertyInfo.PropertyType == typeof(Icon))
                {
                    // Icon is of type System.Drawing.Icon
                    propertyInfo.SetValue(uiElement, _icon.ToIcon());
                }
                else
                {
                    Log.Error().WriteLine("Unknown type for Icon property: {0}", propertyInfo.PropertyType);
                }
            }
        }
    }
}