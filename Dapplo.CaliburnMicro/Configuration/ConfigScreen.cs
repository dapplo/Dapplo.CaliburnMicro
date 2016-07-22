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
using System.Collections.ObjectModel;
using System.Globalization;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	///     A basic implementation of IConfigScreen
	/// </summary>
	public class ConfigScreen : Screen, IConfigScreen
	{
		private bool _canActivate = true;
		private bool _isEnabled = true;
		private bool _isVisible = true;
		private string _id;

		/// <summary>
		/// Default constructor take the name of the type for the Id
		/// </summary>
		public ConfigScreen()
		{
			_id = GetType().Name;
		}

		/// <summary>
		/// Tests if the IConfigScreen contains the supplied text
		/// </summary>
		/// <param name="text">the text to search for</param>
		public virtual bool Contains(string text)
		{
			return string.IsNullOrEmpty(text) || CultureInfo.CurrentUICulture.CompareInfo.IndexOf(DisplayName, text, CompareOptions.IgnoreCase) >= 0;
		}

		/// <summary>
		///     Do some general initialization, if needed
		///     This is called when the config UI is initializing
		/// </summary>
		public virtual void Initialize(IConfig config)
		{
		}

		/// <summary>
		///     Terminate is called (must!) for every IConfigScreen when the parent IConfig Terminate is called.
		///     No matter if this config screen was every shown and what reason there is to leave the configuration screen.
		/// </summary>
		public virtual void Terminate()
		{
		}

		/// <summary>
		/// This is called when the configuration should be "persisted"
		/// </summary>
		public virtual void Commit()
		{
		}

		/// <summary>
		/// This is called when the configuration should be "rolled back"
		/// </summary>
		public virtual void Rollback()
		{
		}

		/// <summary>
		///     Returns if the IConfigScreen can be activated
		/// </summary>
		public virtual bool CanActivate
		{
			get { return _canActivate; }
			protected set
			{
				_canActivate = value;
				NotifyOfPropertyChange(nameof(CanActivate));
			}
		}

		/// <summary>
		///     Returns if the IConfigScreen can be selected
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
		///     Returns if the IConfigScreen is visible
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

		#region ITreeNode

		/// <summary>
		///     Used to showing this IConfigScreen inside a tree
		/// </summary>
		public virtual ITreeNode<IConfigScreen> ParentNode { get; set; }

		/// <summary>
		///     Used to showing this IConfigScreen inside a tree
		/// </summary>
		public virtual ICollection<ITreeNode<IConfigScreen>> ChildNodes { get; set; } = new ObservableCollection<ITreeNode<IConfigScreen>>();

		/// <summary>
		///     The parent under which the IConfigScreen is shown, null is root
		/// </summary>
		public virtual string ParentId { get; set; }

		/// <summary>
		///     The Id of this IConfigScreen, is also used to order children of a parent.
		///     By default the name of the type is used
		/// </summary>
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		#endregion
	}
}