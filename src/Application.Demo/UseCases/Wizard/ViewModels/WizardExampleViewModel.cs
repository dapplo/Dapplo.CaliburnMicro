// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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