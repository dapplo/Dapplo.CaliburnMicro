//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Caliburn.Micro.Demo
// 
//  Caliburn.Micro.Demo is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Caliburn.Micro.Demo is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Caliburn.Micro.Demo. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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