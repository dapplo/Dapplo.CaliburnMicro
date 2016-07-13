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

using System.ComponentModel.Composition;
using Caliburn.Micro;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	/// <summary>
	///     This is the ViewModel for the Notification popup, it's currently important to specify a special PartCreationPolicy
	///     to prevent exceptions as every time a popup is created a new instance is needed. Later I might add an attibute covering this.
	/// </summary>
	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class NotificationExampleViewModel : Screen
	{
	}
}