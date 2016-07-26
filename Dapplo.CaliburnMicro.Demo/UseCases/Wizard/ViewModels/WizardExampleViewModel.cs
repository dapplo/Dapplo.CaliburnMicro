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
using System.ComponentModel.Composition;
using System.Linq;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.CaliburnMicro.Wizard.ViewModels;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Wizard.ViewModels
{
	[Export]
	public class WizardExampleViewModel : Wizard<IWizardScreen>, IPartImportsSatisfiedNotification
	{
		private bool _isStep2Enabled = false;

		[ImportMany]
		private IEnumerable<IWizardScreen> WizardItems { get; set; }

		[Import]
		public IWizardTranslations WizardTranslations { get; set; }

		public WizardProgressViewModel WizardProgress { get; set; }

		public override bool CanFinish => WizardScreens.Last() == CurrentWizardScreen;

		public override bool CanCancel => WizardScreens.Last() != CurrentWizardScreen;

		public bool IsStep2Enabled
		{
			get { return _isStep2Enabled; }
			set
			{
				_isStep2Enabled = value;
				NotifyOfPropertyChange(nameof(IsStep2Enabled));
			}
		}

		public void OnImportsSatisfied()
		{
			WizardProgress = new WizardProgressViewModel(this);
			// automatically update the DisplayName
			this.BindDisplayName(WizardTranslations, nameof(IWizardTranslations.Title));
			// Set the WizardScreens by ordering them
			WizardScreens = WizardItems.OrderBy(x => x.Order);
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			CurrentWizardScreen = WizardScreens.FirstOrDefault();
		}
	}
}