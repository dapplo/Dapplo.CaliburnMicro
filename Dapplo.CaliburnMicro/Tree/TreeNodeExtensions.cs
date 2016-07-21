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
using System.Linq;

#endregion

namespace Dapplo.CaliburnMicro.Tree
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
		/// <typeparam name="TTreeNodeItem">The specific type of the ITreeNodes</typeparam>
		/// <returns>IEnumerable</returns>
		public static IEnumerable<TTreeNodeItem> CreateTree<TTreeNodeItem>(this IEnumerable<TTreeNodeItem> items)
			where TTreeNodeItem : class, ITreeNode<TTreeNodeItem>
		{
			var treeNodes = items.OrderBy(x => x.Id).ToList();

			// Build a tree for the ConfigScreens
			foreach (var treeNode in treeNodes)
			{
				if (treeNode.ParentId == null)
				{
					yield return treeNode;
					continue;
				}
				var parentNode = treeNodes.FirstOrDefault(x => x.Id == treeNode.ParentId);

				// Check if we can find a parent
				if (parentNode == null)
				{
					yield return treeNode;
				}
				else
				{
					// Assign the parent to the tree node
					treeNode.ParentNode = parentNode;
					// Add the tree node to the parent's children
					parentNode.ChildNodes.Add(treeNode);
				}
			}
		}
	}
}