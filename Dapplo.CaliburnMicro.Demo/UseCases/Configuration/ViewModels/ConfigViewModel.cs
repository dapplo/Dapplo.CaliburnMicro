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
using Dapplo.CaliburnMicro.Demo.ViewModels;
using Dapplo.Config.Language;
using Dapplo.Log.Facade;
using Dapplo.Utils.Extensions;
using MahApps.Metro.Controls.Dialogs;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels
{
	/// <summary>
	///     The settings view model is, well... for the settings :)
	///     It is a conductor where one item is active.
	/// </summary>
	[Export]
	public class ConfigViewModel : Config<IConfigScreen>, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		public IConfigTranslations ConfigTranslations { get; set; }

		[Import]
		private CredentialsViewModel CredentialsVm { get; set; }

		[Import]
		private IDemoConfiguration DemoConfiguration { get; set; }

		/// <summary>
		/// Used to send events
		/// </summary>
		[Import]
		private IEventAggregator EventAggregator { get; set; }

		/// <summary>
		///     Get all settings controls, these are the items that are displayed.
		/// </summary>
		[ImportMany]
		public override IEnumerable<IConfigScreen> ConfigScreens { get; set; }

		/// <summary>
		/// Used to show a "normal" dialog
		/// </summary>
		[Import]
		private IWindowManager WindowsManager { get; set; }

		/// <summary>
		/// Used to make it possible to show a MahApps dialog
		/// </summary>
		[Import]
		private IDialogCoordinator Dialogcoordinator { get; set; }

		/// <summary>
		///     This is called when an item from the itemssource is selected
		///     And will make sure that the selected item is made visible.
		/// </summary>
		/// <param name="view"></param>
		public void ActivateChildView(IConfigScreen view)
		{
			ActivateItem(view);
		}

		/// <summary>
		/// Show the credentials for the login
		/// </summary>
		public void Login()
		{
			var result = WindowsManager.ShowDialog(CredentialsVm);
			Log.Info().WriteLine($"Girl you know it's {result}");
		}

		/// <summary>
		/// Show a MahApps dialog from the MVVM
		/// </summary>
		/// <returns></returns>
		public async Task Dialog()
		{
			await Dialogcoordinator.ShowMessageAsync(this, "Message from VM", "MVVM based dialogs!");
		}

		public void OnImportsSatisfied()
		{
			// automatically update the DisplayName
			CoreTranslations.OnPropertyChanged(pcEvent =>
			{
				DisplayName = CoreTranslations.Settings;
			}, nameof(ICoreTranslations.Settings));

			// Set the current language (this should update all registered OnPropertyChanged anyway, so it can run in the background
			var lang = DemoConfiguration.Language;
			Task.Run(async () => await LanguageLoader.Current.ChangeLanguageAsync(lang).ConfigureAwait(false));
		}
	}
}