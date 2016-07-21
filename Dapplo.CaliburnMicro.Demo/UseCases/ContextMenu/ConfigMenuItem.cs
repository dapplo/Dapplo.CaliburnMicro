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
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Demo.UseCases.Configuration.ViewModels;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Log.Facade;
using Dapplo.Utils;
using MahApps.Metro.Controls;

#endregion

namespace Dapplo.CaliburnMicro.Demo.UseCases.ContextMenu
{
	/// <summary>
	/// This will add an extry for the configuration to the context menu
	/// </summary>
	[Export(typeof(IMenuItem))]
	public class ConfigMenuItem : MenuItem, IPartImportsSatisfiedNotification
	{
		private static readonly LogSource Log = new LogSource();

		[Import]
		public IWindowManager WindowManager { get; set; }

		[Import]
		public ConfigViewModel ConfigViewModel { get; set; }

		[Import]
		private IContextMenuTranslations ContextMenuTranslations { get; set; }

		public void OnImportsSatisfied()
		{
			using (new NoSynchronizationContextScope())
			{
				UiContext.RunOn(() =>
				{
					DisplayName = ContextMenuTranslations.Configure;
					Icon = new PackIconModern
					{
						Kind = PackIconModernKind.Settings
					};
				}).Wait();
			}
		}

		public override void Click(IMenuItem clickedItem)
		{
			Log.Debug().WriteLine("Configure");
			// TODO: Closing the ConfigViewModel also closes other windows, check / fix
			if (WindowManager.ShowDialog(ConfigViewModel) == false)
			{
				Log.Warn().WriteLine("You cancelled the configuration");
			}
		}
	}
}