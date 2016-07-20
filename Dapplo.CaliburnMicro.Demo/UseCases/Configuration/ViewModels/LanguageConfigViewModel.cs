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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.Config.Language;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels
{
	[Export(typeof(IConfigScreen))]
	public sealed class LanguageConfigViewModel : ConfigScreen, IPartImportsSatisfiedNotification
	{
		public IDictionary<string, string> AvailableLanguages => LanguageLoader.Current.AvailableLanguages;

		public void OnImportsSatisfied()
		{
			// automatically update the DisplayName
			CoreTranslations.OnPropertyChanged(pcEvent =>
			{
				DisplayName = CoreTranslations.Language;
			}, nameof(ICoreTranslations.Language));

			// automatically update the CanChangeLanguage state when a different language is selected
			DemoConfiguration.OnPropertyChanged(pcEvent =>
			{
				NotifyOfPropertyChange(nameof(CanChangeLanguage));
			}, nameof(IDemoConfiguration.Language));
		}

		/// <summary>
		///     Can the login button be pressed?
		/// </summary>
		public bool CanChangeLanguage => !string.IsNullOrWhiteSpace(DemoConfiguration.Language) && DemoConfiguration.Language != LanguageLoader.Current.CurrentLanguage;

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		public IDemoConfiguration DemoConfiguration { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		public async Task ChangeLanguage()
		{
			EventAggregator.PublishOnUIThread($"Changing to language: {DemoConfiguration.Language}");
			await LanguageLoader.Current.ChangeLanguageAsync(DemoConfiguration.Language).ConfigureAwait(false);
		}

		public override int ParentId { get; } = (int)ConfigIds.Ui;

		public override int Id { get; } = (int) ConfigIds.Ui + 1;
	}
}