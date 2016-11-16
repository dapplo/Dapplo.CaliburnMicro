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
using System.Windows;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public static class BooleanVisibility
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
			"Value",
			typeof(bool),
			typeof(BooleanVisibility),
			new PropertyMetadata(true, OnVisibilityChanged));

		public static readonly DependencyProperty WhenTrueProperty = DependencyProperty.RegisterAttached(
			"WhenTrue",
			typeof(Visibility),
			typeof(BooleanVisibility),
			new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

		public static readonly DependencyProperty WhenFalseProperty = DependencyProperty.RegisterAttached(
			"WhenFalse",
			typeof(Visibility),
			typeof(BooleanVisibility),
			new PropertyMetadata(Visibility.Collapsed, OnVisibilityChanged));

		private static readonly AttachedBehavior Behavior =
			AttachedBehavior.Register(host => new BooleanVisibilityBehavior(host));

		private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Behavior.Update(d);
		}

		public static void SetValue(UIElement uiElement, bool value)
		{
			Contract.Requires(uiElement != null);

			uiElement.SetValue(ValueProperty, value);
		}

		public static bool GetValue(UIElement uiElement)
		{
			Contract.Requires(uiElement != null);

			return (bool) uiElement.GetValue(ValueProperty);
		}

		public static void SetWhenTrue(UIElement uiElement, Visibility visibility)
		{
			Contract.Requires(uiElement != null);

			uiElement.SetValue(WhenTrueProperty, visibility);
		}

		public static Visibility GetWhenTrue(UIElement uiElement)
		{
			Contract.Requires(uiElement != null);

			return (Visibility) uiElement.GetValue(WhenTrueProperty);
		}

		public static void SetWhenFalse(UIElement uiElement, Visibility visibility)
		{
			Contract.Requires(uiElement != null);

			uiElement.SetValue(WhenFalseProperty, visibility);
		}

		public static Visibility GetWhenFalse(UIElement uiElement)
		{
			Contract.Requires(uiElement != null);

			return (Visibility) uiElement.GetValue(WhenFalseProperty);
		}

		private sealed class BooleanVisibilityBehavior : Behavior<UIElement>
		{
			internal BooleanVisibilityBehavior(DependencyObject host) : base(host)
			{
				// Does not propagate external changes in visibility to binding source - that will be
				// covered in a future post
			}

			protected override void Update(UIElement host)
			{
				host.Visibility = GetValue(host) ? GetWhenTrue(host) : GetWhenFalse(host);
			}
		}
	}
}