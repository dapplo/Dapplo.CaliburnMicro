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

using Caliburn.Micro;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	///     Every element in the config UI should implement this
	///     Some of the configuration functionality is covered in standard Caliburn.Micro interfaces
	///     which are supplied by the interfaces which IScreen extends:
	///     IHaveDisplayName: Covers the name and title of the config screen
	///     INotifyPropertyChanged: Makes it possible that changes to e.g. the name are directly represented in the view
	///     IActivate, IDeactivate: to know if the config screen is activated or deactivated
	///     IGuardClose.CanClose: Prevents leaving the config screen
	///     A default implementation is just to extend Screen from Caliburn.Micro
	/// </summary>
	public interface IConfigScreen : IScreen, ITreeNode<IConfigScreen>
	{
		/// <summary>
		///     Returns if the IConfigScreen can be activated (when clicking on it)
		/// </summary>
		bool CanActivate { get; }

		/// <summary>
		///     Returns if the IConfigScreen can be selected (visible but not usable)
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		///     Returns if the IConfigScreen is visible (not visible and not usable)
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Tests if the IConfigScreen contains the supplied text
		/// </summary>
		/// <param name="text">the text to search for</param>
		bool Contains(string text);

		/// <summary>
		///     Do some general initialization, if needed
		///     This is called when the config UI is initialized
		/// </summary>
		void Initialize(IConfig config);

		/// <summary>
		///     Terminate the config screen.
		///     This is called when the parent config UI is terminated
		/// </summary>
		void Terminate();

		/// <summary>
		/// This is called when the configuration should be "persisted"
		/// </summary>
		void Commit();

		/// <summary>
		/// This is called when the configuration should be "rolled back"
		/// </summary>
		void Rollback();
	}
}