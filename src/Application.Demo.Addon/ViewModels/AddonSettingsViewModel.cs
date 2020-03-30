//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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