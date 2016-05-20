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
using System.ComponentModel;

#endregion

namespace Dapplo.CaliburnMicro
{
	/// <summary>
	///     Make a PropertyChange event with a certain property generate a PropertyChanged event with a different name.
	///     This can be used to bind language changes to the DisplayName
	/// </summary>
	public static class NotifyPropertyChangedExtensions
	{
		/// <summary>
		///     Bind PropertyChanged events from a source property, e.g. via ILanguage to a destination property, e.g. DisplayName
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <param name="notifier">INotifyPropertyChanged</param>
		/// <param name="notifierProperty">Nameof the source property</param>
		/// <param name="notifies">Action with PropertyChangedEventArgs</param>
		/// <param name="notifiesProperty">Nameof the target property</param>
		public static void BindChanges<T1>(this T1 notifier, string notifierProperty, Action<PropertyChangedEventArgs> notifies, string notifiesProperty)
			where T1 : INotifyPropertyChanged
		{
			notifier.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == notifierProperty)
				{
					notifies(new PropertyChangedEventArgs(notifiesProperty));
				}
			};
		}
	}
}