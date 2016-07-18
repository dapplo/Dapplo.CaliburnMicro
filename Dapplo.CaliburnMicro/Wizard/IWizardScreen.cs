﻿#region Dapplo 2016 - GNU Lesser General Public License

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

#endregion

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	///     Every Step in a wizard should implement this
	///     Some of the wizard functionality is covered in standard Caliburn interfaces, which are supplied by the interfaces
	///     which IScreen extends:
	///     IHaveDisplayName: Covers the name and title of the Wizard screen
	///     INotifyPropertyChanged: Makes it possible that changes to e.g. the name are directly represented in the view
	///     IActivate, IDeactivate: to know if the wizard screen is activated or deactivated
	///     IGuardClose.CanClose: Prevents leaving the wizard screen
	///     A default implementation is to extend Screen
	/// </summary>
	public interface IWizardScreen : IScreen
	{
		/// <summary>
		/// The order in which the screens are shown
		/// </summary>
		int Order { get; }

		/// <summary>
		/// Do some general initialization, if needed
		/// This is called, no matter if the IWizardScreen is shown.
		/// </summary>
		void Initialize(IWizard parent);

		/// <summary>
		/// Cleanup the wizard screen
		/// </summary>
		void Terminate();

		/// <summary>
		///     Returns if the wizard screen can be selected
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		///     Returns if the wizard screen is visible
		/// </summary>
		bool IsVisible { get; }
	}
}