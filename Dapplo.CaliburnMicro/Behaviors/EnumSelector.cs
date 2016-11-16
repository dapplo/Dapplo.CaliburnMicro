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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public static class EnumSelector
	{
		public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.RegisterAttached(
			"SelectedValue",
			typeof(object),
			typeof(EnumSelector),
			new PropertyMetadata(OnSelectedValueChanged));

		public static readonly DependencyProperty ItemValueProperty = DependencyProperty.RegisterAttached(
			"ItemValue",
			typeof(string),
			typeof(EnumSelector),
			new PropertyMetadata(OnItemValueChanged));

		private static readonly AttachedBehavior Behavior =
			AttachedBehavior.Register(host => new EnumSelectorBehavior(host));

		private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Behavior.Update(d);
		}

		private static void OnItemValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var item = d as FrameworkElement;

			if (item != null)
			{
				var selector = item.Parent as Selector;

				if (selector != null)
				{
					Behavior.Update(selector);
				}
			}
		}

		public static object GetSelectedValue(Selector selector)
		{
			return selector.GetValue(SelectedValueProperty);
		}

		public static void SetSelectedValue(Selector selector, object value)
		{
			selector.SetValue(SelectedValueProperty, value);
		}

		public static string GetItemValue(DependencyObject item)
		{
			return (string) item.GetValue(ItemValueProperty);
		}

		public static void SetItemValue(DependencyObject item, string value)
		{
			item.SetValue(ItemValueProperty, value);
		}

		private sealed class EnumSelectorBehavior : Behavior<Selector>
		{
			private readonly IDictionary<object, EnumCheck> _itemEnumChecks = new Dictionary<object, EnumCheck>();

			internal EnumSelectorBehavior(DependencyObject host) : base(host)
			{
			}

			protected override void Attach(Selector host)
			{
				host.SelectionChanged += OnSelectionChanged;
			}

			protected override void Detach(Selector host)
			{
				host.SelectionChanged -= OnSelectionChanged;

				_itemEnumChecks.Clear();
			}

			protected override void Update(Selector host)
			{
				var selectedValue = GetSelectedValue(host);

				for (var index = 0; index < host.Items.Count; index++)
				{
					var item = host.Items[index];

					var itemEnumCheck = GetItemEnumCheck(item);

					itemEnumCheck.Update(selectedValue, GetItemValue(item));

					if (itemEnumCheck.IsMatch)
					{
						host.SelectedIndex = index;

						break;
					}
				}
			}

			private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
			{
				TryUpdate(UpdateSelectedValue);
			}

			private void UpdateSelectedValue(Selector host)
			{
				var selectedValue = GetSelectedValue(host);

				var itemEnumCheck = GetItemEnumCheck(host.SelectedItem);

				itemEnumCheck.Update(selectedValue, GetItemValue(host.SelectedItem));

				var parsedTargetValue = itemEnumCheck.ParsedTargetValue;

				if ((selectedValue != null) && !selectedValue.Equals(parsedTargetValue))
				{
					SetSelectedValue(host, parsedTargetValue);
				}
			}

			private EnumCheck GetItemEnumCheck(object item)
			{
				EnumCheck itemEnumCheck;

				if (!_itemEnumChecks.TryGetValue(item, out itemEnumCheck))
				{
					itemEnumCheck = new EnumCheck();

					_itemEnumChecks[item] = itemEnumCheck;
				}

				return itemEnumCheck;
			}

			private static string GetItemValue(object item)
			{
				var dependencyObjectItem = item as DependencyObject;

				return dependencyObjectItem == null ? null : EnumSelector.GetItemValue(dependencyObjectItem);
			}
		}
	}
}