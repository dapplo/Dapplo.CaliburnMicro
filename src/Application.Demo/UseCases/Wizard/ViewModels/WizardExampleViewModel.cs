//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2019 Dapplo
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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.CaliburnMicro.Wizard.ViewModels;

namespace Application.Demo.UseCases.Wizard.ViewModels
{
    /// <summary>
    /// An example of the wizard
    /// </summary>
    [SuppressMessage("Sonar Code Smell", "S110:Inheritance tree of classes should not be too deep", Justification = "MVVM Framework brings huge interitance tree.")]
    public sealed class WizardExampleViewModel : Wizard<IWizardScreen>
    {
        private bool _isStep2Enabled;
        private bool _isStep3Visible;
        private bool _isStep4Complete;

        public override bool CanCancel => WizardScreens.Last() != CurrentWizardScreen;

        public override bool CanFinish => WizardScreens.Last() == CurrentWizardScreen;

        public bool IsStep2Enabled
        {
            get => _isStep2Enabled;
            set
            {
                _isStep2Enabled = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsStep3Visible
        {
            get => _isStep3Visible;
            set
            {
                _isStep3Visible = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsStep4Complete
        {
            get => _isStep4Complete;
            set
            {
                _isStep4Complete = value;
                NotifyOfPropertyChange();
            }
        }

        public WizardProgressViewModel WizardProgress { get; }

        public IWizardTranslations WizardTranslations { get; }

        public WizardExampleViewModel(
            IEnumerable<Lazy<IWizardScreen>> wizardItems,
            IWizardTranslations wizardTranslations)
        {
            WizardTranslations = wizardTranslations;
            // automatically update the DisplayName
            WizardTranslations.CreateDisplayNameBinding(this, nameof(IWizardTranslations.Title));
            // Set the WizardScreens as TrulyObservableCollection (needed for the WizardProgressViewModel) and by ordering them
            WizardScreens = wizardItems.Select(x => x.Value).OrderBy(x => x.Order);
            WizardProgress = new WizardProgressViewModel(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            CurrentWizardScreen = WizardScreens.FirstOrDefault();
        }
    }
}