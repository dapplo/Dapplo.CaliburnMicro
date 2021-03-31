// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;

namespace Application.Demo.UseCases.Wizard.ViewModels
{
    public sealed class WizardStep2ViewModel : WizardScreen<WizardExampleViewModel>
    {
        private IDisposable _displayNameUpdater;

        private readonly IWizardTranslations _wizardTranslations;

        public WizardStep2ViewModel(IWizardTranslations wizardTranslations)
        {
            _wizardTranslations = wizardTranslations;
            Order = 2;
            IsEnabled = false;
        }

        public override void Initialize()
        {
            // automatically update the DisplayName
            _displayNameUpdater = _wizardTranslations.CreateDisplayNameBinding(this, nameof(IWizardTranslations.TitleStep2));
            ParentWizard.OnPropertyChanged(nameof(WizardExampleViewModel.IsStep2Enabled)).Subscribe(s => IsEnabled = ParentWizard.IsStep2Enabled);
        }

        public override void Terminate()
        {
            _displayNameUpdater?.Dispose();
        }
    }
}