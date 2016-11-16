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
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public static class EnumVisibility
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
			"Value",
			typeof(object),
			typeof(EnumVisibility),
			new PropertyMetadata(OnArgumentsChanged));

		public static readonly DependencyProperty TargetValueProperty = DependencyProperty.RegisterAttached(
			"TargetValue",
			typeof(string),
			typeof(EnumVisibility),
			new PropertyMetadata(OnArgumentsChanged));

		public static readonly DependencyProperty WhenMatchedProperty = DependencyProperty.RegisterAttached(
			"WhenMatched",
			typeof(Visibility),
			typeof(EnumVisibility),
			new FrameworkPropertyMetadata(Visibility.Visible, OnArgumentsChanged));

		public static readonly DependencyProperty WhenNotMatchedProperty = DependencyProperty.RegisterAttached(
			"WhenNotMatched",
			typeof(Visibility),
			typeof(EnumVisibility),
			new FrameworkPropertyMetadata(Visibility.Collapsed, OnArgumentsChanged));

		private static readonly AttachedBehavior Behavior =
			AttachedBehavior.Register(host => new EnumVisibilityBehavior(host));

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

		public static string GetTargetValue(UIElement uiElement)
		{
			return (string) uiElement.GetValue(TargetValueProperty);
		}

		public static void SetTargetValue(UIElement uiElement, string value)
		{
			uiElement.SetValue(TargetValueProperty, value);
		}

		public static Visibility GetWhenMatched(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenMatchedProperty);
		}

		public static void SetWhenMatched(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenMatchedProperty, visibility);
		}

		public static Visibility GetWhenNotMatched(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(WhenNotMatchedProperty);
		}

		public static void SetWhenNotMatched(UIElement uiElement, Visibility visibility)
		{
			uiElement.SetValue(WhenNotMatchedProperty, visibility);
		}

		private sealed class EnumVisibilityBehavior : Behavior<UIElement>
		{
			private readonly EnumCheck _enumCheck = new EnumCheck();

			internal EnumVisibilityBehavior(DependencyObject host) : base(host)
			{
			}

			protected override void Update(UIElement host)
			{
				_enumCheck.Update(GetValue(host), GetTargetValue(host));

				host.Visibility = _enumCheck.IsMatch ? GetWhenMatched(host) : GetWhenNotMatched(host);
			}
		}
	}
}