// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;

namespace Application.Demo.UseCases.Wizard.ViewModels
{
    public sealed class WizardStep3ViewModel : WizardScreen<WizardExampleViewModel>
    {
        private IDisposable _displayNameUpdater;

        private readonly IWizardTranslations _wizardTranslations;

        public WizardStep3ViewModel(IWizardTranslations wizardTranslations)
        {
            _wizardTranslations = wizardTranslations;
            Order = 3;
        }

        public override void Initialize()
        {
            // automatically update the DisplayName
            _displayNameUpdater = _wizardTranslations.CreateDisplayNameBinding(this, nameof(IWizardTranslations.TitleStep3));
            IsVisible = false;
            ParentWizard.OnPropertyChanged(nameof(WizardExampleViewModel.IsStep3Visible)).Subscribe(s => IsVisible = ParentWizard.IsStep3Visible);
        }

        public override void Terminate()
        {
            _displayNameUpdater?.Dispose();
        }
    }
}