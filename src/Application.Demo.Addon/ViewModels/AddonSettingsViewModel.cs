//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
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

#region using

using System.ComponentModel.Composition;
using Application.Demo.Addon.Languages;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Security;
using Application.Demo.Shared;

#endregion

namespace Application.Demo.Addon.ViewModels
{
    [Export(typeof(IConfigScreen))]
    public sealed class AddonSettingsViewModel : ConfigScreen
    {
        public AddonSettingsViewModel()
        {
            ParentId = nameof(ConfigIds.Addons);
        }

        public IAddonTranslations AddonTranslations { get; }

        private IAuthenticationProvider AuthenticationProvider { get; }

        private IEventAggregator EventAggregator { get; }

        [ImportingConstructor]
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