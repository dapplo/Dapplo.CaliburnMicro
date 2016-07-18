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
	///     A very simple implementation of IWizardScreen
	/// </summary>
	public abstract class WizardScreen : Screen, IWizardScreen
	{
		private bool _isEnabled = true;
		private bool _isVisible = true;
		private int _order = 1;

		/// <summary>
		///     The order in which the screens are shown
		/// </summary>
		public virtual int Order
		{
			get { return _order; }
			protected set
			{
				_order = value;
				NotifyOfPropertyChange(nameof(Order));
			}
		}

		/// <summary>
		///     Do some general initialization, if needed
		///     This is called, no matter if the IWizardScreen is shown.
		/// </summary>
		public virtual void Initialize(IWizard parent)
		{
		}

		/// <summary>
		///     Cleanup the wizard screen
		/// </summary>
		public virtual void Terminate()
		{
		}

		/// <summary>
		///     Returns if the wizard screen can be selected
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
		///     Returns if the wizard screen is visible
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
	}
}