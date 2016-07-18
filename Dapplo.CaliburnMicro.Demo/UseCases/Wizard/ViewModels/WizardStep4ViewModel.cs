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

using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Wizard.ViewModels
{
	[Export(typeof(IWizardScreen))]
	public class WizardStep4ViewModel : WizardScreen
	{
		private IDisposable _displayNameUpdater;

		[Import]
		private IWizardTranslations WizardTranslations { get; set; }

		public WizardStep4ViewModel()
		{
			// ReSharper disable once VirtualMemberCallInConstructor
			Order = 4;
		}

		public override void Initialize(IWizard parent)
		{
			_displayNameUpdater = WizardTranslations.OnPropertyChanged(propertyName => DisplayName = WizardTranslations.TitleStep4);
		}

		public override void Terminate()
		{
			_displayNameUpdater?.Dispose();
		}
	}
}