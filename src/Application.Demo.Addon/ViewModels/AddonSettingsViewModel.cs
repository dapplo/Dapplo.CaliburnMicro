// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Application.Demo.Addon.Languages;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Security;
using Application.Demo.Shared;

namespace Application.Demo.Addon.ViewModels
{
    /// <summary>
    /// The ViewModel for the add-on settings
    /// </summary>
    public sealed class AddonSettingsViewModel : SimpleConfigScreen
    {
        public AddonSettingsViewModel()
        {
            ParentId = nameof(ConfigIds.Addons);
        }

        public IAddonTranslations AddonTranslations { get; }

        private IAuthenticationProvider AuthenticationProvider { get; }

        private IEventAggregator EventAggregator { get; }

        public AddonSettingsViewModel(
            IAddonTranslations addonTranslations,
            IAuthenticationProvider authenticationProvider,
            IEventAggregator eventAggregator)
        {
            AddonTranslations = addonTranslations;
            EventAggregator = eventAggregator;
            AuthenticationProvider = authenticationProvider;
            // automatically update the DisplayName
            AddonTranslations.CreateDisplayNameBinding(this, nameof(IAddonTranslations.Addon));
        }

        // ReSharper disable once UnusedMember.Global
        public void AddAdmin()
        {
            var authenticationProvider = AuthenticationProvider as SimpleAuthenticationProvider;
            authenticationProvider?.AddPermission("Admin");
        }

        // ReSharper disable once UnusedMember.Global
        public void DoSomething()
        {
            EventAggregator.PublishOnUIThread("Addon button clicked");
        }

        // ReSharper disable once UnusedMember.Global
        public void RemoveAdmin()
        {
            var authenticationProvider = AuthenticationProvider as SimpleAuthenticationProvider;
            authenticationProvider?.RemovePermission("Admin");
        }
    }
}