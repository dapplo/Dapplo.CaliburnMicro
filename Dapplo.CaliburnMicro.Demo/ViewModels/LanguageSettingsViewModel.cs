//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Interfaces;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.Config.Language;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	[Export(typeof(ISettingsControl))]
	public class LanguageSettingsViewModel : Screen, ISettingsControl, IPartImportsSatisfiedNotification
	{
		public IDictionary<string, string> AvailableLanguages => LanguageLoader.Current.AvailableLanguages;

		/// <summary>
		///     Can the login button be pressed?
		/// </summary>
		public bool CanChangeLanguage => !string.IsNullOrWhiteSpace(DemoConfiguration.Language);

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		public IDemoConfiguration DemoConfiguration { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		public void OnImportsSatisfied()
		{
			CoreTranslations.BindChanges(nameof(CoreTranslations.Language), OnPropertyChanged, nameof(DisplayName));
		}

		/// <summary>
		///     Implement the IHaveDisplayName
		/// </summary>
		public override string DisplayName
		{
			get { return CoreTranslations.Language; }
			set { throw new NotImplementedException($"Set {nameof(DisplayName)}"); }
		}

		public async Task ChangeLanguage()
		{
			EventAggregator.PublishOnUIThread($"Changing to language: {DemoConfiguration.Language}");
			await LanguageLoader.Current.ChangeLanguageAsync(DemoConfiguration.Language);
		}
	}
}