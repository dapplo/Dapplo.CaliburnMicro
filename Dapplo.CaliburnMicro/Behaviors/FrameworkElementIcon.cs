#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dapplo.Log;
using Size = System.Windows.Size;
using Dapplo.CaliburnMicro.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	///     This class contains a behavior to assist in setting an Icon of a e.g. Window or TaskbarIcon via a FrameworkElement
	/// </summary>
	public static class FrameworkElementIcon
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		/// The icon to set
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
			"Value",
			typeof(FrameworkElement),
			typeof(FrameworkElementIcon),
			new PropertyMetadata(OnArgumentsChanged));

		/// <summary>
		/// What is the target property which should used for the icon
		/// </summary>
		public static readonly DependencyProperty TargetValueProperty = DependencyProperty.RegisterAttached(
			"TargetValue",
			typeof(string),
			typeof(FrameworkElementIcon),
			new PropertyMetadata("Icon", OnArgumentsChanged));

		/// <summary>
		/// Use the IconBehavior
		/// </summary>
		private static AttachedBehavior Behavior { get; } = AttachedBehavior.Register(host => new IconBehavior((FrameworkElement)host));

		/// <summary>
		/// When the arguments change, the Behavior.Update is called
		/// </summary>
		/// <param name="dependencyObject">DependencyObject</param>
		/// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs</param>
		private static void OnArgumentsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			Behavior.Update(dependencyObject);
		}

		/// <summary>
		/// Returns the value from the UIElement of the DependencyProperty specified in ValueProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <returns>object which represents the value of the in ValueProperty specified DependencyProperty</returns>
		public static FrameworkElement GetValue(UIElement uiElement)
		{
			Contract.Requires(uiElement != null);
			return (FrameworkElement)uiElement.GetValue(ValueProperty);
		}

		/// <summary>
		/// Sets the value of the UIElement to the DependencyProperty specified in ValueProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <param name="value">Value to assign to the in ValueProperty specified DependencyProperty</param>
		public static void SetValue(UIElement uiElement, FrameworkElement value)
		{
			Contract.Requires(uiElement != null);
			uiElement.SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Returns the value from the UIElement of the DependencyProperty specified in TargetValueProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <returns>string which represents the name of the target property</returns>
		public static string GetTargetValue(UIElement uiElement)
		{
			Contract.Requires(uiElement != null);
			return (string)uiElement.GetValue(TargetValueProperty);
		}

		/// <summary>
		/// Sets the value of the UIElement to the DependencyProperty specified in TargetValueProperty.
		/// </summary>
		/// <param name="uiElement">UIElement</param>
		/// <param name="value">Value to assign to the in TargetValueProperty specified DependencyProperty</param>
		public static void SetTargetValue(UIElement uiElement, string value)
		{
			Contract.Requires(uiElement != null);
			uiElement.SetValue(TargetValueProperty, value);
		}

		/// <summary>
		/// Implementation of the actual behavior logic of the EnumVisibilityBehavior
		/// </summary>
		private sealed class IconBehavior : Behavior<FrameworkElement>
		{
			internal IconBehavior(FrameworkElement host) : base(host)
			{
			}

			protected override void Update(FrameworkElement host)
			{
				var icon = GetValue(host);
				if (icon == null)
				{
					return;
				}
				if (!host.IsLoaded)
				{
					// If the host is not loaded, wait until it is.
					FrameworkElement target = host.IsLoaded ? icon : host;
					var handlers = new RoutedEventHandler[1];
					handlers[0] = (sender, args) =>
					{
						Update(host);
						target.Loaded -= handlers[0];
					};
					target.Loaded += handlers[0];
					return;
				}

				var iconProperty = GetTargetValue(host);
				var propertyInfo = host.GetType().GetProperty(iconProperty);
				if (propertyInfo == null)
				{
					return;
				}
				if (propertyInfo.PropertyType == typeof(ImageSource))
				{
					// Icon is of type ImageSource
					var image = propertyInfo.GetValue(host) as ImageSource;
					// Default size for the icon
					var size = new Size(256, 256);
					if (image != null)
					{
						size = new Size(image.Width, image.Height);
					}
					var iconIcon = icon.ToBitmapSource(size);
					using (var fileStream = new FileStream(@"C:\LocalData\icon.png", FileMode.Create))
					{
						BitmapEncoder encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(iconIcon));
						encoder.Save(fileStream);
					}
					propertyInfo.SetValue(host, iconIcon);
				}
				else if (propertyInfo.PropertyType == typeof(Icon))
				{
					// Icon is of type System.Drawing.Icon
					propertyInfo.SetValue(host, icon.ToIcon());
				}
				else
				{
					Log.Error().WriteLine("Unknown type for Icon property: {0}", propertyInfo.PropertyType);
				}
			}
		}
	}
}