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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Disposables;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.Utils.Extensions;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels
{
	[Export(typeof(IConfigScreen))]
	public sealed class ThemeConfigViewModel : ConfigScreen
	{
		/// <summary>
		/// Here all disposables are registered, so we can clean the up
		/// </summary>
		private CompositeDisposable _disposables;

		/// <summary>
		/// The avaible themes
		/// </summary>
		public ObservableCollection<Tuple<Themes, string>> AvailableThemes { get; set; } = new ObservableCollection<Tuple<Themes, string>>();

		/// <summary>
		/// The avaible theme accents
		/// </summary>
		public ObservableCollection<Tuple<ThemeAccents, string>> AvailableThemeAccents { get; set; } = new ObservableCollection<Tuple<ThemeAccents, string>>();

		[Import]
		public IConfigTranslations ConfigTranslations { get; set; }

		[Import]
		public IDemoConfiguration DemoConfiguration { get; set; }

		[Import(typeof(IWindowManager))]
		private MetroWindowManager MetroWindowManager { get; set; }

		public override void Initialize(IConfig config)
		{
			// Prepare disposables
			_disposables?.Dispose();
			_disposables = new CompositeDisposable();

			AvailableThemeAccents.Clear();
			foreach (var themeAccent in Enum.GetValues(typeof(ThemeAccents)).Cast<ThemeAccents>())
			{
				var translation = themeAccent.EnumValueOf();
				AvailableThemeAccents.Add(new Tuple<ThemeAccents, string>(themeAccent, translation));
			}

			AvailableThemes.Clear();
			foreach (var theme in Enum.GetValues(typeof(Themes)).Cast<Themes>())
			{
				var translation = theme.EnumValueOf();
				AvailableThemes.Add(new Tuple<Themes, string>(theme, translation));
			}

			// Place this under the Ui parent
			ParentId = nameof(ConfigIds.Ui);

			// Make sure Commit/Rollback is called on the IDemoConfiguration
			config.Register(DemoConfiguration);

			// automatically update the DisplayName
			_disposables.Add(this.BindDisplayName(ConfigTranslations, nameof(IConfigTranslations.Theme)));

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
			MetroWindowManager.ChangeTheme(DemoConfiguration.Theme);
			MetroWindowManager.ChangeThemeAccent(DemoConfiguration.ThemeAccent);
			base.Commit();
		}
	}
}