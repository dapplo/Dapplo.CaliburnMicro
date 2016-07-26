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

#endregion

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	///     Every screen in a wizard should implement this
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
		/// The parent wizard where this IWizardScreen is used
		/// </summary>
		IWizard ParentWizard { get; set; }

		/// <summary>
		///     The order in which the IWizardScreen ist shown
		/// </summary>
		int Order { get; }

		/// <summary>
		///     Returns if the wizard screen can be selected (visible but not usable)
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		///     Returns if the wizard screen is visible (not visible and not usable)
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		///     Do some general initialization, if needed
		///     This is called when the parent wizard is initializing.
		/// ParentWizard will be set before this is called
		/// </summary>
		void Initialize();

		/// <summary>
		///     Terminate the wizard screen.
		///     This is called when the parent wizard is terminated
		/// </summary>
		void Terminate();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TParentWizard"></typeparam>
	public interface IWizardScreen<TParentWizard> : IWizardScreen where TParentWizard : IWizard
	{
		/// <summary>
		/// The parent wizard where this IWizardScreen is used
		/// </summary>
		new TParentWizard ParentWizard { get; set; }
	}
}