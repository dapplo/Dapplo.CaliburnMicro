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
using System.Reactive.Linq;
using Caliburn.Micro;
using System.Reactive;
using System.Reactive.Disposables;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	/// Extensions for IHaveDisplayName
	/// </summary>
	public static class HaveDisplayNameExtensions
	{
		/// <summary>
		/// Bind the property of a INotifyPropertyChanged implementing class to the DisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
		/// <param name="propertyName">string with the name of a property</param>
		/// <param name="disposables">optional CompositeDisposable to add the binding to</param>
		/// <returns>IDisposable</returns>
		public static IDisposable BindDisplayName(this IHaveDisplayName haveDisplayName, INotifyPropertyChanged notifyPropertyChanged, string propertyName, CompositeDisposable disposables = null)
		{
			var propertyChangedObservable = notifyPropertyChanged.OnPropertyChangedPattern();
			return haveDisplayName.BindDisplayName(propertyChangedObservable, propertyName, disposables);
		}

		/// <summary>
		/// Bind the property of a INotifyPropertyChanged implementing class to the DisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
		/// <param name="propertyName">string with the name of a property</param>
		/// <param name="disposables">optional CompositeDisposable to add the binding to</param>
		/// <returns>IEventObservable for the PropertyChanged event, dispose this to stop it</returns>
		public static IObservable<EventPattern<PropertyChangedEventArgs>> MultiBindDisplayName(this IHaveDisplayName haveDisplayName, INotifyPropertyChanged notifyPropertyChanged, string propertyName, CompositeDisposable disposables = null)
		{
			var propertyChangedObservable = notifyPropertyChanged.OnPropertyChangedPattern();
			haveDisplayName.BindDisplayName(propertyChangedObservable, propertyName, disposables);
			return propertyChangedObservable;
		}

		/// <summary>
		/// Bind the property of a INotifyPropertyChanged implementing class to the DisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="observable">IObservable for EventPattern of PropertyChangedEventArgs</param>
		/// <param name="propertyName">string with the name of a property</param>
		/// <param name="disposables">optional CompositeDisposable to add the binding to</param>
		/// <returns>IDisposable for the event registration, dispose this to stop it</returns>
		public static IDisposable BindDisplayName(this IHaveDisplayName haveDisplayName, IObservable<EventPattern<PropertyChangedEventArgs>> observable, string propertyName, CompositeDisposable disposables = null)
		{
			var binding = observable.Where(args => args.EventArgs.PropertyName == propertyName)
					.Subscribe(pattern => haveDisplayName.DisplayName = pattern.Sender.GetType().GetProperty(pattern.EventArgs.PropertyName).GetValue(pattern.Sender) as string);
			disposables?.Add(binding);

			return binding;
		}
	}
}