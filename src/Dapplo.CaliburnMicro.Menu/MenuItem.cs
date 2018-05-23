//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
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

#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Caliburn.Micro;

#endregion

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
            get { return _style; }
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

        #region IHaveIcon

        /// <summary>
        ///     Returns the icon of the IMenuItem
        /// </summary>
        public virtual Control Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                NotifyOfPropertyChange(nameof(Icon));
            }
        }

        #endregion

        #region IHaveDisplayName

        /// <summary>
        ///     Returns the DisplayName of the IMenuItem
        /// </summary>
        public virtual string DisplayName
        {
            get { return _displayName ?? GetType().Name; }
            set
            {
                _displayName = value;
                NotifyOfPropertyChange(nameof(DisplayName));
            }
        }

        #endregion

        #region IHaveId

        /// <summary>
        ///     The Id of this IMenuItem, is also used to order children of a parent
        ///     Default the Id is the name of the Type
        /// </summary>
        public string Id { get; set; }

        #endregion

        #region IAmDisplayable

        /// <summary>
        ///     Returns if the IMenuItem is enabled
        /// </summary>
        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
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
            get { return _isVisible; }
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(nameof(IsVisible));
            }
        }

        #endregion

        #region ITreeNode

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

        #endregion
    }
}