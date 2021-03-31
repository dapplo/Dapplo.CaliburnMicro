// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Menu
{
    /// <summary>
    ///     Extend this to make your IMenuItem
    /// </summary>
    public class MenuItem : PropertyChangedBase, IMenuItem
    {
        private string _displayName;
        private Control _icon;

        private bool _isEnabled = true;
        private bool _isVisible = true;
        private MenuItemStyles _style = MenuItemStyles.Default;

        /// <summary>
        ///     Default constructor take the name of the type for the Id
        /// </summary>
        public MenuItem()
        {
            Id = GetType().Name;
        }

        /// <summary>
        ///     This property defines the style the item uses.
        /// </summary>
        public MenuItemStyles Style
        {
            get => _style;
            set
            {
                _style = value;
                NotifyOfPropertyChange(nameof(Style));
            }
        }

        /// <summary>
        ///     The initialize is called from the UI Thread before the menu-item is added to a context menu.
        ///     This allows for any UI initialization, like icons etc, to be made
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <inheritdoc />
        public string HotKeyHint { get; set; }

        /// <summary>
        ///     Returns the icon of the IMenuItem
        /// </summary>
        public virtual Control Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                NotifyOfPropertyChange(nameof(Icon));
            }
        }

        /// <summary>
        ///     Returns the DisplayName of the IMenuItem
        /// </summary>
        public virtual string DisplayName
        {
            get => _displayName ?? GetType().Name;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(nameof(DisplayName));
            }
        }

        /// <summary>
        ///     The Id of this IMenuItem, is also used to order children of a parent
        ///     Default the Id is the name of the Type
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Returns if the IMenuItem is enabled
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
        ///     Returns if the IMenuItem is visible
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
        ///     Used to showing this inside a tree
        /// </summary>
        public virtual ITreeNode<IMenuItem> ParentNode { get; set; }

        /// <summary>
        ///     Used to showing this inside a tree
        /// </summary>
        public virtual ICollection<ITreeNode<IMenuItem>> ChildNodes { get; set; } = new ObservableCollection<ITreeNode<IMenuItem>>();

        /// <summary>
        ///     The parent under which the IMenuItem is shown, null is root
        /// </summary>
        public virtual string ParentId { get; set; }
    }
}