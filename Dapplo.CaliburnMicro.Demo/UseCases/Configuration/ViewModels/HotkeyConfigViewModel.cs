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
using System.Reactive.Disposables;
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.CaliburnMicro.Extensions;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels
{
	[Export(typeof(IConfigScreen))]
	public sealed class HotkeyConfigViewModel : ConfigScreen
	{
		/// <summary>
		/// Here all disposables are registered, so we can clean the up
		/// </summary>
		private CompositeDisposable _disposables;

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

			// Place this under the Ui parent
			ParentId = nameof(ConfigIds.Ui);

			// Make sure Commit/Rollback is called on the IDemoConfiguration
			config.Register(DemoConfiguration);

			// automatically update the DisplayName
			this.BindDisplayName(ConfigTranslations, nameof(IConfigTranslations.Hotkey), _disposables);

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