// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Application.Demo.Languages;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Wizard;

namespace Application.Demo.UseCases.Wizard.ViewModels
{
    public sealed class WizardStep1ViewModel : WizardScreen<WizardExampleViewModel>
    {
        private IDisposable _displayNameUpdater;

        public WizardStep1ViewModel(IWizardTranslations wizardTranslations)
        {
            WizardTranslations = wizardTranslations;
            Order = 1;
        }

        public IWizardTranslations WizardTranslations { get; }

        public override void Initialize()
        {
            // automatically update the DisplayName
            _displayNameUpdater = WizardTranslations.CreateDisplayNameBinding(this, nameof(IWizardTranslations.TitleStep1));
        }

        public override void Terminate()
        {
            _displayNameUpdater?.Dispose();
        }
    }
}