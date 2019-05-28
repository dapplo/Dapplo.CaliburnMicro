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

        #region SelectedItem

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

            if (targetObject.FindItemNode(targetObject.SelectedItem) is TreeViewItem tvi)
            {
                tvi.IsSelected = true;
            }
        }
        #endregion SelectedItem   

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
