// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     A basic implementation of ITreeScreen, this is a screen which is visible in an IConfig
    /// </summary>
    public class TreeScreen<TTreeScreen> : Screen, ITreeScreenNode<TTreeScreen>
    {
        private bool _canActivate = true;
        private string _id;
        private bool _isEnabled = true;
        private bool _isVisible = true;

        /// <summary>
        ///     Default constructor take the name of the type for the Id
        /// </summary>
        public TreeScreen()
        {
            _id = GetType().Name;
        }

        /// <summary>
        ///     Returns if the ITreeScreen can be activated
        /// </summary>
        public virtual bool CanActivate
        {
            get => _canActivate;
            protected set
            {
                _canActivate = value;
                NotifyOfPropertyChange(nameof(CanActivate));
            }
        }

        /// <summary>
        ///     The Id of this ITreeScreen, is also used to order children of a parent.
        ///     By default the name of the type is used
        /// </summary>
        public virtual string Id
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>
        ///     Returns if the ITreeScreen can be selected
        /// </summary>
        public virtual bool IsEnabled
        {
            get => _isEnabled;
            protected set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(nameof(IsEnabled));
            }
        }

        /// <summary>
        ///     Returns if the ITreeScreen is visible
        /// </summary>
        public virtual bool IsVisible
        {
            get => _isVisible;
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(nameof(IsVisible));
            }
        }

        /// <summary>
        ///     Used to showing this ITreeScreen inside a tree
        /// </summary>
        public virtual ITreeNode<TTreeScreen> ParentNode { get; set; }

        /// <summary>
        ///     Used to showing this ITreeScreen inside a tree
        /// </summary>
        public virtual ICollection<ITreeNode<TTreeScreen>> ChildNodes { get; set; } = new ObservableCollection<ITreeNode<TTreeScreen>>();

        /// <summary>
        ///     The parent under which the ITreeScreen is shown, null is root
        /// </summary>
        public virtual string ParentId { get; set; }
    }
}