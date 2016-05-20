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
using Caliburn.Micro;
using Dapplo.CaliburnMicro.Demo.Interfaces;
using Dapplo.CaliburnMicro.Demo.Models;
using Dapplo.Config.Language;
using Dapplo.Utils;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	/// <summary>
	///     The settings view model is, well... for the settings :)
	///     It is a conductor where one item is active.
	/// </summary>
	[Export(typeof(IShell))]
	public class SettingsViewModel : Conductor<ISettingsControl>.Collection.OneActive, IShell, IPartImportsSatisfiedNotification
	{
		[Import]
		private ICoreTranslations CoreTranslations { get; set; }

		[Import]
		private IDemoConfiguration DemoConfiguration { get; set; }

		/// <summary>
		///     Make the DisplayName be translatable
		/// </summary>
		public override string DisplayName
		{
			get { return CoreTranslations.Settings; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		///     Get all settings controls, these are the items that are displayed.
		/// </summary>
		[ImportMany]
		private IEnumerable<ISettingsControl> SettingsControls { get; set; }

		[Import]
		private IWindowManager WindowsManager { get; set; }

		public void OnImportsSatisfied()
		{
			CoreTranslations.BindChanges(nameof(CoreTranslations.Settings), OnPropertyChanged, nameof(DisplayName));
		}

		/// <summary>
		///     This is called when an item from the itemssource is selected
		///     And will make sure that the selected item is made visible.
		/// </summary>
		/// <param name="view"></param>
		public void ActivateChildView(ISettingsControl view)
		{
			ActivateItem(view);
		}

		/// <summary>
		///     As soon as it's activated, the items that are imported are add to the Observable Items collection.
		/// </summary>
		protected override void OnActivate()
		{
			base.OnActivate();
			var lang = DemoConfiguration.Language;

			// Add all the imported settings controls
			// TODO: Sort them for a tree view, somehow...
			Items.AddRange(SettingsControls);

			UiContext.RunOn(async () => await LanguageLoader.Current.ChangeLanguageAsync(lang)).Wait();
		}
	}
}