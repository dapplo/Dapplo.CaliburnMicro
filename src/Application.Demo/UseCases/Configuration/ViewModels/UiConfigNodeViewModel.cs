// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Languages;
using Application.Demo.Shared;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;

namespace Application.Demo.UseCases.Configuration.ViewModels
{
    /// <summary>
    /// This represents a node in the config
    /// </summary>
    public sealed class UiConfigNodeViewModel : ConfigNode
    {
        public IDemoConfigTranslations ConfigTranslations { get; }

        public UiConfigNodeViewModel(IDemoConfigTranslations configTranslations)
        {
            ConfigTranslations = configTranslations;

            // automatically update the DisplayName
            ConfigTranslations.CreateDisplayNameBinding(this, nameof(IDemoConfigTranslations.Ui));

            // automatically update the DisplayName
            CanActivate = false;
            Id = nameof(ConfigIds.Ui);
        }
    }
}