//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro.Demo
// 
//  Dapplo.CaliburnMicro.Demo is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro.Demo is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro.Demo. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System.Windows.Controls.Primitives;
using Caliburn.Micro;

#endregion

namespace Dapplo.CaliburnMicro.NotifyIconWpf
{
	/// <summary>
	///     Make your ViewModel extend this, instead of Screen, so you can get to the ITrayIcon
	/// </summary>
	public class TrayIconScreen : Screen
	{
		public ITrayIcon TrayIcon
		{
			get
			{
				var popup = GetView() as Popup;
				return popup?.Child as ITrayIcon;
			}
		}
	}
}