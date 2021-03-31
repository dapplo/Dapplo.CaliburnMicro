// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Addon.Languages;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Application.Demo.Shared;

namespace Application.Demo.Addon.ViewModels
{
    /// <summary>
    /// A ViewModel for the configuration which cannot be selected
    /// </summary>
    public sealed class NotSelectableConfigViewModel : SimpleConfigScreen
    {
        public IAddonTranslations AddonTranslations { get; }

        public NotSelectableConfigViewModel(IAddonTranslations addonTranslations)
        {
            AddonTranslations = addonTranslations;
            ParentId = nameof(ConfigIds.Addons);
            IsEnabled = false;
            // automatically update the DisplayName
            AddonTranslations.CreateDisplayNameBinding(this, nameof(IAddonTranslations.NotSelectableAddon));
        }
    }
}