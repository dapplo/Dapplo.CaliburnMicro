// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     Interface for tree nodes
    /// </summary>
    /// <typeparam name="TTreeItem"></typeparam>
    public interface ITreeNode<TTreeItem> : IHaveId, IHaveDisplayName
    {
        /// <summary>
        ///     The children for this ITreeNode, the collections MUST be initialized!!
        /// </summary>
        ICollection<ITreeNode<TTreeItem>> ChildNodes { get; set; }

        /// <summary>
        ///     This defines the Location in the tree, by specifying the Id of the parent, where the config screen is shown.
        ///     if the value is null, or the parent can't be found, this item is placed into the root
        /// </summary>
        string ParentId { get; }

        /// <summary>
        ///     The parent for this ITreeNode
        /// </summary>
        ITreeNode<TTreeItem> ParentNode { get; set; }
    }
}