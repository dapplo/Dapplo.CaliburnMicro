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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

#endregion

namespace Dapplo.CaliburnMicro.Collections
{
	/// <summary>
	/// A which comes from <a href="http://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop">here</a>
	/// </summary>
	/// <typeparam name="T">Type for the collection items</typeparam>
	public sealed class TrulyObservableCollection<T> : ObservableCollection<T>
		where T : INotifyPropertyChanged
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TrulyObservableCollection()
		{
			CollectionChanged += FullObservableCollectionCollectionChanged;
		}

		/// <summary>
		/// Constructor which a IEnumerable
		/// </summary>
		/// <param name="pItems"></param>
		public TrulyObservableCollection(IEnumerable<T> pItems) : this()
		{
			foreach (var item in pItems)
			{
				Add(item);
			}
		}

		private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var item in e.NewItems)
				{
					((INotifyPropertyChanged) item).PropertyChanged += ItemPropertyChanged;
				}
			}
			if (e.OldItems != null)
			{
				foreach (var item in e.OldItems)
				{
					((INotifyPropertyChanged) item).PropertyChanged -= ItemPropertyChanged;
				}
			}
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T) sender));
			OnCollectionChanged(args);
		}
	}
}