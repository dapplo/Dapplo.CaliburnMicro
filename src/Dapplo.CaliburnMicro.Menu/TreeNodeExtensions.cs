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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     Helper extension to build a tree
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        ///     Create a tree from the supplied IEnumerable
        /// </summary>
        /// <param name="items">IEnumerable with ITreeNodes</param>
        /// <param name="predicate">
        ///     Function which allows you to specify if the item is visible (parents with visible children are
        ///     always visible)
        /// </param>
        /// <typeparam name="TTreeNodeItem">The specific type of the ITreeNodes</typeparam>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<ITreeNode<TTreeNodeItem>> CreateTree<TTreeNodeItem>(this IEnumerable<TTreeNodeItem> items, Func<TTreeNodeItem, bool> predicate = null)
            where TTreeNodeItem : class, ITreeNode<TTreeNodeItem>
        {
            var treeNodes = items.OrderBy(x => x.Id).ToList();

            var rootItems = new List<ITreeNode<TTreeNodeItem>>();
            // Build a tree for the ConfigScreens
            foreach (var treeNode in treeNodes)
            {
                // No parentId?
                var hasParentId = !string.IsNullOrEmpty(treeNode.ParentId);

                // Find the parent
                var parentNode = treeNodes.FirstOrDefault(x => x.Id == treeNode.ParentId);

                // Check if we found a parent, if not place treeNode in the root
                if (!hasParentId || parentNode == null)
                {
                    treeNode.ParentNode = null;
                    rootItems.Add(treeNode);
                    continue;
                }

                // if the tree already had a parent, remove it from the previous parent
                treeNode.ParentNode?.ChildNodes.Remove(treeNode);
                // Assign the parent to the tree node
                treeNode.ParentNode = parentNode;
                // Add the tree node to the parent's children, if it wasn't already
                if (!parentNode.ChildNodes.Contains(treeNode))
                {
                    parentNode.ChildNodes.Add(treeNode);
                }
            }

            if (predicate == null)
            {
                return rootItems;
            }
            // Filter out the ones which don't match the predicate
            foreach (var treeNodeItem in treeNodes)
            {
                if (treeNodeItem.ChildNodes.Any())
                {
                    continue;
                }
                if (predicate(treeNodeItem))
                {
                    continue;
                }
                // should not be shown, removed it from it's parent or the root
                treeNodeItem.ParentNode?.ChildNodes.Remove(treeNodeItem);
                if (treeNodeItem.ParentNode?.ChildNodes != null && !treeNodeItem.ParentNode.ChildNodes.Any())
                {
                    rootItems.Remove(treeNodeItem.ParentNode);
                }
                rootItems.Remove(treeNodeItem);
            }
            return rootItems;
        }
    }
}