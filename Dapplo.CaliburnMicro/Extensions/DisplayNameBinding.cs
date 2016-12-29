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
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;

#endregion

namespace Dapplo.CaliburnMicro.Extensions
{
	/// <summary>
	///     Binding describing all information needed for binding names
	/// </summary>
	public class DisplayNameBinding : IDisposable
	{
		/// <summary>
		///     Create the name binding object
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="notifyPropertyChanged"></param>
		public DisplayNameBinding(IObservable<EventPattern<PropertyChangedEventArgs>> observable, INotifyPropertyChanged notifyPropertyChanged)
		{
			Observable = observable;
			NotifyPropertyChanged = notifyPropertyChanged;
			Disposables = new CompositeDisposable();
		}

		/// <summary>
		///     Observable as event provider for the name binding
		/// </summary>
		public IObservable<EventPattern<PropertyChangedEventArgs>> Observable { get; }

		/// <summary>
		///     The source of the events
		/// </summary>
		public INotifyPropertyChanged NotifyPropertyChanged { get; }

		/// <summary>
		///     All bindings are stored here
		/// </summary>
		/// tran
		public CompositeDisposable Disposables { get; }

		/// <summary>
		///     Dispose the underlying bindings which are stored in a CompositeDisposable
		/// </summary>
		public void Dispose()
		{
			Disposables?.Dispose();
		}
	}
}