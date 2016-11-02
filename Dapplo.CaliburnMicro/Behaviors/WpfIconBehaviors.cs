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

using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.Log.Facade;
using Size = System.Windows.Size;

namespace Dapplo.CaliburnMicro.Behaviors
{

	/// <summary>
	/// This class contains logic to assist in setting an Icon of e.g. Window or e.g. TaskbarIcon from a FrameworkElement
	/// </summary>
	public static class WpfIconBehaviors
	{
		private static readonly LogSource Log = new LogSource();
		/// <summary>
		/// Sets the specified frameworkElement as value for the target icon
		/// </summary>
		/// <param name="dependencyObject"></param>
		/// <param name="frameworkElement"></param>
		public static void SetIcon(DependencyObject dependencyObject, FrameworkElement frameworkElement)
		{
			dependencyObject.SetValue(WpfIconProperty, frameworkElement);
		}

		/// <summary>
		/// Update the icon from the dependencyObject's value
		/// </summary>
		/// <param name="dependencyObject">DependencyObject</param>
		public static void RefreshIcon(DependencyObject dependencyObject)
		{

			var frameworkElement = dependencyObject.GetValue(WpfIconProperty) as FrameworkElement;
			SetIconInternally(dependencyObject, frameworkElement);
		}

		/// <summary>
		/// Use to set an Icon with a FrameworkElement
		/// </summary>
		public static readonly DependencyProperty WpfIconProperty = DependencyProperty.RegisterAttached("WpfIconProperty", typeof(FrameworkElement), typeof(WpfIconBehaviors), new PropertyMetadata(null, PropertyChangedCallback));

		private static void SetIconInternally(DependencyObject dependencyObject, FrameworkElement icon)
		{
			var propertyInfo = dependencyObject.GetType().GetProperty("Icon");
			if (propertyInfo == null)
			{
				return;
			}
			if (icon == null)
			{
				propertyInfo.SetValue(dependencyObject, null);
				return;
			}


			if (propertyInfo.PropertyType == typeof(ImageSource))
			{
				// Icon is of type ImageSource
				var image = propertyInfo.GetValue(dependencyObject) as ImageSource;
				// Default size for the icon
				var size = new Size(256, 256);
				if (image != null)
				{
					size = new Size(image.Width, image.Height);
				}
				propertyInfo.SetValue(dependencyObject, icon.ToBitmapSource(size));
			}
			else if (propertyInfo.PropertyType == typeof(Icon))
			{
				// Icon is of type System.Drawing.Icon
				propertyInfo.SetValue(dependencyObject, icon.ToIcon());
			}
			else
			{
				Log.Error().WriteLine("Unknown type for Icon property: {0}", propertyInfo.PropertyType);
			}
		}

		/// <summary>
		/// Handle the fact that the property changed, now really apply the value
		/// </summary>
		/// <param name="dependencyObject">DependencyObject which is the target</param>
		/// <param name="dependencyPropertyChangedEventArgs">DependencyPropertyChangedEventArgs which describes the value</param>
		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var frameworkElement = dependencyPropertyChangedEventArgs.NewValue as FrameworkElement;
			SetIconInternally(dependencyObject, frameworkElement);
		}
	}
}
