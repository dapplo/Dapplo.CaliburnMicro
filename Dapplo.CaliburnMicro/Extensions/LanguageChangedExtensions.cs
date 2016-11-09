#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Config
// 
// Dapplo.Config is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Config is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Config. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Reactive.Linq;
using Dapplo.Config.Language;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	///     Supply an extension to simplify the usage of ILanguage.LanguageChanged
	/// </summary>
	public static class LanguageChangedExtensions
	{
		/// <summary>
		///     Automatically call the update action when the LanguageChanged fires
		///     If the is called on a DI object, make sure it's available.
		///     When using MEF, it would be best to call this from IPartImportsSatisfiedNotification.OnImportsSatisfied
		/// </summary>
		/// <param name="language">ILanguage</param>
		/// <param name="updateAction">Action to call on active and update, the argument is the property name</param>
		/// <param name="run">default the action is run when defining, specify false if this is not wanted</param>
		/// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
		public static IDisposable OnLanguageChanged(this ILanguage language, Action<ILanguage> updateAction, bool run = true)
		{
			if (language == null)
			{
				throw new ArgumentNullException(nameof(language));
			}
			if (updateAction == null)
			{
				throw new ArgumentNullException(nameof(updateAction));
			}
			if (run)
			{
				updateAction(language);
			}
			var observable = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(
							   h => language.LanguageChanged += h,
							   h => language.LanguageChanged -= h);

			return observable.Subscribe(pce => updateAction(pce.Sender as ILanguage));
		}
	}
}