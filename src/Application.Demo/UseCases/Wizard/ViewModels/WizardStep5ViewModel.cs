// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;

namespace Application.Demo.UseCases.Wizard.ViewModels
{
    public sealed class WizardStep5ViewModel : WizardScreen<WizardExampleViewModel>
    {
        private IDisposable _displayNameUpdater;
        private readonly IWizardTranslations _wizardTranslations;

        public WizardStep5ViewModel(IWizardTranslations wizardTranslations)
        {
            _wizardTranslations = wizardTranslations;
            Order = 5;
        }
 
        public override void Initialize()
        {
            // automatically update the DisplayName
            _displayNameUpdater = _wizardTranslations.CreateDisplayNameBinding(this, nameof(IWizardTranslations.TitleStep5));
        }

        public override void Terminate()
        {
            _displayNameUpdater?.Dispose();
        }
    }
}