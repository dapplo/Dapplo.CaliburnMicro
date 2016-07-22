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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.Config.Language;
using Dapplo.Utils.Extensions;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels
{
	[Export(typeof(IConfigScreen))]
	public sealed class LanguageConfigViewModel : ConfigScreen
	{
		private readonly Disposables _disposables = new Disposables();

		public IDictionary<string, string> AvailableLanguages => LanguageLoader.Current.AvailableLanguages;

		/// <summary>
		///     Can the login button be pressed?
		/// </summary>
		public bool CanChangeLanguage
			=> !string.IsNullOrWhiteSpace(DemoConfiguration.Language) && DemoConfiguration.Language != LanguageLoader.Current.CurrentLanguage;

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		public IDemoConfiguration DemoConfiguration { get; set; }

		[Import]
		private IEventAggregator EventAggregator { get; set; }

		public override void Initialize(IConfig config)
		{
			// Place this under the Ui parent
			ParentId = nameof(ConfigIds.Ui);

			// Make sure Commit/Rollback is called on the IDemoConfiguration
			config.Register(DemoConfiguration);

			// automatically update the DisplayName
			_disposables.Add(CoreTranslations.OnPropertyChanged(pcEvent =>
			{
				DisplayName = CoreTranslations.Language;
			}, nameof(ICoreTranslations.Language)));

			// automatically update the CanChangeLanguage state when a different language is selected
			_disposables.Add(DemoConfiguration.OnPropertyChanged(pcEvent =>
			{
				NotifyOfPropertyChange(nameof(CanChangeLanguage));
			}, nameof(IDemoConfiguration.Language)));

			base.Initialize(config);
		}

		protected override void OnDeactivate(bool close)
		{
			_disposables.Dispose();
			base.OnDeactivate(close);
		}

		public override void Commit()
		{
			// Manually commit
			DemoConfiguration.CommitTransaction();
			EventAggregator.PublishOnUIThread($"Changing to language: {DemoConfiguration.Language}");
			UiContext.RunOn(async () =>
			{
				await LanguageLoader.Current.ChangeLanguageAsync(DemoConfiguration.Language).ConfigureAwait(false);
			});
			base.Commit();
		}
	}
}