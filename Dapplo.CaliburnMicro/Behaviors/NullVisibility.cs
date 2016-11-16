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

using System.Windows;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	/// Hide a UIElement when the target is null
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public static class NullVisibility
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
			"Value",
			typeof(object),
			typeof(NullVisibility),
			new PropertyMetadata(true, OnArgumentsChanged));

		public static readonly DependencyProperty WhenNullProperty = DependencyProperty.RegisterAttached(
			"WhenNull",
			typeof(Visibility),
			typeof(NullVisibility),
			new PropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

		public static readonly DependencyProperty WhenNotNullProperty = DependencyProperty.RegisterAttached(
			"WhenNotNull",
			typeof(Visibility),
			typeof(NullVisibility),
			new PropertyMetadata(Visibility.Visible, OnArgumentsChanged));

		private static readonly AttachedBehavior Behavior =
			AttachedBehavior.Register(host => new NullVisibilityBehavior(host));

		private static void OnArgumentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Behavior.Update(d);
		}

		public static object GetValue(UIElement uiElement)
		{
			return uiElement.GetValue(ValueProperty);
		}

		public static void SetValue(UIElement uiElement, object value)
		{
			uiElement.SetValue(ValueProperty, value);
		}

		public static Visibility GetWhenNull(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenNullProperty);
		}

		public static void SetWhenNull(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenNullProperty, visibility);
		}

		public static Visibility GetWhenNotNull(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenNotNullProperty);
		}

		public static void SetWhenNotNull(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenNotNullProperty, visibility);
		}

		private sealed class NullVisibilityBehavior : Behavior<UIElement>
		{
			internal NullVisibilityBehavior(DependencyObject host) : base(host)
			{
			}

			protected override void Update(UIElement host)
			{
				host.Visibility = GetValue(host) == null
					? GetWhenNull(host)
					: GetWhenNotNull(host);
			}
		}
	}
}