//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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
using Caliburn.Micro;

#endregion

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

        #region IHaveID

        /// <summary>
        ///     The Id of this ITreeScreen, is also used to order children of a parent.
        ///     By default the name of the type is used
        /// </summary>
        public virtual string Id
        {
            get => _id;
            set => _id = value;
        }

        #endregion

        #region IAmDisplayable

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

        #endregion

        #region ITreeNode

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

        #endregion
    }
}