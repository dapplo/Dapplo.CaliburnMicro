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
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Wizard.ViewModels
{
	[Export]
	public class WizardExampleViewModel : Wizard<IWizardScreen>, IPartImportsSatisfiedNotification
	{
		[ImportMany]
		private IEnumerable<IWizardScreen> WizardElements { get; set; }

		[Import]
		public IWizardTranslations WizardTranslations { get; set; }

		public override bool CanFinish
		{
			get { return WizardElements.Last() == CurrentWizardScreen; }
		}

		public override bool CanCancel
		{
			get { return WizardElements.Last() != CurrentWizardScreen; }
		}

		public void OnImportsSatisfied()
		{
			WizardTranslations.OnPropertyChanged(propertyName => DisplayName = WizardTranslations.Title);
		}


		/// <summary>
		///     OnActivate is overriden to add the items to the list.
		/// </summary>
		protected override void OnActivate()
		{
			// Make sure the order is step1, step2 by ordering on the name
			Items.AddRange(WizardElements.OrderBy(x => x.GetType().Name));

			base.OnActivate();
		}
	}
}