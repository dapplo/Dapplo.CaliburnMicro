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
using System.ComponentModel;

#endregion

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	///     Base interface for the IWizard
	/// </summary>
	public interface IWizard : INotifyPropertyChanged
	{
		/// <summary>
		/// The current progress
		/// </summary>
		int Progress { get; }

		/// <summary>
		/// Test if we are at the last screen
		/// </summary>
		bool IsLast { get; }

		/// <summary>
		/// Test if we are at the first screen
		/// </summary>
		bool IsFirst { get; }

		/// <summary>
		///     The TWizardScreen items of the wizard
		/// </summary>
		IEnumerable<IWizardScreen> WizardScreens { get; set; }

		/// <summary>
		///     Returns the current wizard screen
		/// </summary>
		IWizardScreen CurrentWizardScreen { get; set; }

		/// <summary>
		///     Can the wizard go to the next wizard screen, where IsEnabled/IsVisible are true, in the list. (e.g. false if we are
		///     at the last)
		/// </summary>
		bool CanNext { get; }

		/// <summary>
		///     Can the wizard go to the previous IsEnabled/IsVisible wizard screen? (e.g. false if we are at the first)
		/// </summary>
		bool CanPrevious { get; }

		/// <summary>
		///     Can the current wizard "flow" be cancelled?
		/// </summary>
		bool CanCancel { get; }

		/// <summary>
		///     Test if the wizard can be finished
		/// </summary>
		bool CanFinish { get; }

		/// <summary>
		///     Initialize will o.a. initialize the wizard screens
		/// </summary>
		void Initialize();

		/// <summary>
		///     Cleanup the wizard.
		/// </summary>
		void Terminate();

		/// <summary>
		///     Go to the next wizard screen, where IsEnabled/IsVisible are true, in the list.
		/// </summary>
		bool Next();

		/// <summary>
		///     Go to the previous wizard screen, where IsEnabled/IsVisible are true, in the list.
		/// </summary>
		bool Previous();

		/// <summary>
		///     Cancel the wizard
		/// </summary>
		void Cancel();

		/// <summary>
		///     Finish the wizard
		/// </summary>
		void Finish();
	}

	/// <summary>
	///     This is the generic interface for a wizard implementation
	/// </summary>
	public interface IWizard<out TWizardScreen> : IWizard
	{
		/// <summary>
		///     The TWizardScreen items of the wizard
		/// </summary>
		new IEnumerable<TWizardScreen> WizardScreens { get; }

		/// <summary>
		///     Returns the current wizard screen
		/// </summary>
		new TWizardScreen CurrentWizardScreen { get; }
	}
}