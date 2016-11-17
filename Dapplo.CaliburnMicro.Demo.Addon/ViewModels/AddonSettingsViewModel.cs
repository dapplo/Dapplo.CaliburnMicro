#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Behaviors.Security;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Demo.Addon.Languages;
using Dapplo.CaliburnMicro.Demo.UseCases.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Security;

#endregion

namespace Dapplo.CaliburnMicro.Demo.Addon.ViewModels
{
	[Export(typeof(IConfigScreen))]
	public sealed class AddonSettingsViewModel : ConfigScreen, IPartImportsSatisfiedNotification
	{
		[Import]
		public IAddonTranslations AddonTranslations { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }


		[Import]
		private IAuthenticationProvider AuthenticationProvider { get; set; }

		public AddonSettingsViewModel()
		{
			ParentId = nameof(ConfigIds.Addons);
		}
		public void OnImportsSatisfied()
		{
			// automatically update the DisplayName
			AddonTranslations.CreateBinding(this, nameof(IAddonTranslations.Addon));
		}

		public void DoSomething()
		{
			EventAggregator.PublishOnUIThread("Addon button clicked");
		}

		public void AddAdmin()
		{
			var authenticationProvider = AuthenticationProvider as SimpleAuthenticationProvider;
			authenticationProvider?.AddPermission("Admin");

		}
		public void RemoveAdmin()
		{
			var authenticationProvider = AuthenticationProvider as SimpleAuthenticationProvider;
			authenticationProvider?.RemovePermission("Admin");

		}
	}
}