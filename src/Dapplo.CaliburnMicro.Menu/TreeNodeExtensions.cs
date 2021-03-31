// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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