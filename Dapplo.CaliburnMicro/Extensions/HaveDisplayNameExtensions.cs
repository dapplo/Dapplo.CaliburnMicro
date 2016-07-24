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
using Caliburn.Micro;
using Dapplo.Log.Facade;
using Dapplo.Utils.Events;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	/// Extensions for IHaveDisplayName
	/// </summary>
	public static class HaveDisplayNameExtensions
	{
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		/// Bind the property of a INotifyPropertyChanged implementing class to the DisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
		/// <param name="propertyName">string with the name of a property</param>
		/// <returns>IEventObservable for the PropertyChanged event, dispose this to stop it</returns>
		public static IEventObservable<PropertyChangedEventArgs> BindDisplayName(this IHaveDisplayName haveDisplayName, INotifyPropertyChanged notifyPropertyChanged, string propertyName)
		{
			var propertyChangedObservable = notifyPropertyChanged.ToObservable();
			haveDisplayName.BindDisplayName(propertyChangedObservable, propertyName);
			return propertyChangedObservable;
		}

		/// <summary>
		/// Bind the property of a INotifyPropertyChanged implementing class to the DisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="eventObservable">IEventObservable for PropertyChangedEventArgs</param>
		/// <param name="propertyName">string with the name of a property</param>
		/// <returns>IDisposable for the event registration, dispose this to stop it</returns>
		public static IDisposable BindDisplayName(this IHaveDisplayName haveDisplayName, IEventObservable<PropertyChangedEventArgs> eventObservable, string propertyName)
		{
			var propertyInfo = eventObservable.Source.GetType().GetProperty(propertyName);

			haveDisplayName.DisplayName = propertyInfo.GetValue(eventObservable.Source) as string;
			return eventObservable.OnPropertyChanged(s =>
			{
				haveDisplayName.DisplayName = propertyInfo.GetValue(eventObservable.Source) as string;
			}, propertyName);
		}
	}
}