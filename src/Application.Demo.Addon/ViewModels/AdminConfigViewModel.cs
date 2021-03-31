// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using Application.Demo.Addon.Languages;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Security;
using Application.Demo.Shared;

namespace Application.Demo.Addon.ViewModels
{
    /// <summary>
    /// The ViewModel for the admin config
    /// </summary>
    public sealed class AdminConfigViewModel : AuthenticatedConfigNode<Visibility>
    {
        public IAddonTranslations AddonTranslations { get; }

        public AdminConfigViewModel(IAddonTranslations addonTranslations)
        {
            AddonTranslations = addonTranslations;

            ParentId = nameof(ConfigIds.Addons);
            this.VisibleOnPermissions("Admin");

            // automatically update the DisplayName
            AddonTranslations.CreateDisplayNameBinding(this, nameof(IAddonTranslations.Admin));
        }

    }
}