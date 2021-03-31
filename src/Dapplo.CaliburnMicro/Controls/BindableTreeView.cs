﻿// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;

namespace Dapplo.CaliburnMicro.Controls
{
    /// <summary>
    /// A treeview which allows you to bind 2 way to the selected item
    /// </summary>
    public class BindableTreeView : TreeView
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BindableTreeView()
        {
            SelectedItemChanged += SelectedItemChangedHandler;
        }

        private void SelectedItemChangedHandler(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        /// <summary>
        /// Gets or Sets the SelectedItem possible Value of the TreeViewItem object.
        /// </summary>
        public new object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// DependencyProperty
        /// </summary>
        public new static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableTreeView), new PropertyMetadata(SelectedItemPropertyChangedHandler));

        private static void SelectedItemPropertyChangedHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is BindableTreeView targetObject))
            {
                return;
            }

            if (targetObject.FindItemNode(targetObject.SelectedItem) is { } tvi)
            {
                tvi.IsSelected = true;
            }
        }

        /// <summary>
        /// Helper method to find the TreeViewItem belonging to the item which is selected
        /// </summary>
        /// <param name="item">object</param>
        /// <returns>TreeViewItem</returns>
        private TreeViewItem FindItemNode(object item)
        {
            TreeViewItem node = null;
            foreach (var data in Items)
            {
                node = ItemContainerGenerator.ContainerFromItem(data) as TreeViewItem;
                if (node == null)
                {
                    continue;
                }

                if (data == item)
                {
                    break;
                }
                node = FindItemNodeInChildren(node, item);
                if (node != null)
                {
                    break;
                }
            }
            return node;
        }

        /// <summary>
        /// Recursive method to assist FindItemNode
        /// </summary>
        /// <param name="parent">TreeViewItem</param>
        /// <param name="item">object with item to find</param>
        /// <returns>TreeViewItem</returns>
        private TreeViewItem FindItemNodeInChildren(TreeViewItem parent, object item)
        {
            TreeViewItem node = null;
            bool isExpanded = parent.IsExpanded;
            if (!isExpanded) //Can't find child container unless the parent node is Expanded once
            {
                parent.IsExpanded = true;
                parent.UpdateLayout();
            }
            foreach (var data in parent.Items)
            {
                node = parent.ItemContainerGenerator.ContainerFromItem(data) as TreeViewItem;
                if (data == item && node != null)
                {
                    break;
                }
                node = FindItemNodeInChildren(node, item);
                if (node != null)
                {
                    break;
                }
            }

            if (node == null && parent.IsExpanded != isExpanded)
            {
                parent.IsExpanded = isExpanded;
            }

            if (node != null)
            {
                parent.IsExpanded = true;
            }
            return node;
        }
    }
}
