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

namespace Dapplo.CaliburnMicro
{
	/// <summary>
	///     Helps to structure the order of starting Dappo StartupActions
	/// </summary>
	public enum CaliburnStartOrder
	{
		/// <summary>
		///     This is the order which the CaliburnMicroBootstrapper uses, if you depend on this take a higher order!
		/// </summary>
		Bootstrapper = 100,

		/// <summary>
		///     This is the order for opening the TrayIcons, IF Dapplo.CaliburnMicro.NotifyIconWpf is used
		/// </summary>
		TrayIcons = 200
	}
}