using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapplo.CaliburnMicro.Tree
{
	/// <summary>
	/// Helper extension to build a tree
	/// </summary>
	public static class TreeNodeExtensions
	{
		/// <summary>
		/// Create a tree from the supplied IEnumerable
		/// </summary>
		/// <param name="items">IEnumerable with ITreeNodes</param>
		/// <typeparam name="TTreeNodeItem">The specific type of the ITreeNodes</typeparam>
		/// <returns>IEnumerable</returns>
		public static IEnumerable<TTreeNodeItem> CreateTree<TTreeNodeItem>(this IEnumerable<TTreeNodeItem> items) where TTreeNodeItem : ITreeNode<TTreeNodeItem>
		{
			var treeNodes = items.ToList();

			// Build a tree for the ConfigScreens
			foreach (var treeNode in treeNodes)
			{
				if (treeNode.ParentId == 0)
				{
					yield return treeNode;
					continue;
				}
				var parentNode = treeNodes.FirstOrDefault(x => x.Id == treeNode.ParentId);

				if (parentNode == null)
				{
					yield return treeNode;
				}
				else
				{
					parentNode.ChildNodes.Add(treeNode);
				}
			}
		}
	}
}
