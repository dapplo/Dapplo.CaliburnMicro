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
	/// Binding describing all information needed for binding names
	/// </summary>
	public class NameBinding : IDisposable
	{
		/// <summary>
		/// Create the name binding object
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="notifyPropertyChanged"></param>
		public NameBinding(IObservable<EventPattern<PropertyChangedEventArgs>> observable, INotifyPropertyChanged notifyPropertyChanged)
		{
			Observable = observable;
			NotifyPropertyChanged = notifyPropertyChanged;
			Disposables = new CompositeDisposable();
		}
		/// <summary>
		/// Observable as event provider for the name binding
		/// </summary>
		public IObservable<EventPattern<PropertyChangedEventArgs>> Observable { get; }

		/// <summary>
		/// The source of the events
		/// </summary>
		public INotifyPropertyChanged NotifyPropertyChanged { get; }

		/// <summary>
		/// All bindings are stored here
		/// </summary>tran
		public CompositeDisposable Disposables { get; }

		/// <summary>
		/// Dispose the underlying bindings which are stored in a CompositeDisposable
		/// </summary>
		public void Dispose()
		{
			Disposables?.Dispose();
		}
	}

	/// <summary>
	/// Extensions for IHaveDisplayName
	/// </summary>
	public static class HaveDisplayNameExtensions
	{
		/// <summary>
		/// Copy the value specified by the property name from the source to the haveDisplayName
		/// </summary>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="eventPattern"></param>
		private static void CopyValue(this IHaveDisplayName haveDisplayName, EventPattern<PropertyChangedEventArgs> eventPattern)
		{
			var source = eventPattern.Sender;
			var propertyName = eventPattern.EventArgs.PropertyName;
			var value = source.GetType().GetProperty(propertyName).GetValue(source) as string;
			haveDisplayName.DisplayName = value;
		}

		/// <summary>
		/// Create a binding between the INotifyPropertyChanged and optional IHaveDisplayName objects.
		/// </summary>
		/// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
		/// <param name="haveDisplayName">optional IHaveDisplayName for the first binding</param>
		/// <param name="propertyName">optional property name for the first binding</param>
		/// <returns>NameBinding</returns>
		public static NameBinding CreateBinding(this INotifyPropertyChanged notifyPropertyChanged, IHaveDisplayName haveDisplayName = null, string propertyName = null)
		{
			var propertyChangedObservable = notifyPropertyChanged.OnPropertyChangedPattern();
			var nameBinding =  new NameBinding(propertyChangedObservable, notifyPropertyChanged);
			if (haveDisplayName != null)
			{
				nameBinding.AddDisplayNameBinding(haveDisplayName, propertyName);
			}
			return nameBinding;
		}

		/// <summary>
		/// Add a displayname binding to the NameBinding
		/// </summary>
		/// <param name="nameBinding">NameBinding to bind to</param>
		/// <param name="haveDisplayName">IHaveDisplayName</param>
		/// <param name="propertyName">Name of the property in the original INotifyPropertyChanged object</param>
		/// <returns>binding</returns>
		public static NameBinding AddDisplayNameBinding(this NameBinding nameBinding, IHaveDisplayName haveDisplayName, string propertyName)
		{
			if (haveDisplayName == null)
			{
				throw new ArgumentNullException(nameof(haveDisplayName));
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException(nameof(propertyName));
			}
			var disposable = nameBinding.Observable.Where(args => args.EventArgs.PropertyName == propertyName).Subscribe(haveDisplayName.CopyValue);
			// Update the display name right away
			haveDisplayName.DisplayName = nameBinding.NotifyPropertyChanged.GetType().GetProperty(propertyName).GetValue(nameBinding.NotifyPropertyChanged) as string;
			// If the disposables is passed, add the disposable
			nameBinding.Disposables?.Add(disposable);
			return nameBinding;
		}
	}
}