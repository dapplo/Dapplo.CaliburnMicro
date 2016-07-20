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
using System.ComponentModel;

#endregion

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	///     Base interface for the IConfiguration
	/// </summary>
	public interface IConfig : INotifyPropertyChanged
	{
		/// <summary>
		///     The config screens for the config UI
		/// </summary>
		IEnumerable<IConfigScreen> ConfigScreens { get; set; }

		/// <summary>
		///     Returns the current config screen
		/// </summary>
		IConfigScreen CurrentConfigScreen { get; set; }

		/// <summary>
		///     The config screens for the config UI in the tree
		/// </summary>
		ICollection<IConfigScreen> TreeItems { get; }

		/// <summary>
		///     Can the current config be cancelled?
		/// </summary>
		bool CanCancel { get; }

		/// <summary>
		///     Test if the config can be OKed
		/// </summary>
		bool CanOk { get; }

		/// <summary>
		///     Initialize will o.a. initialize and sort (tree) the config screens
		/// </summary>
		void Initialize();

		/// <summary>
		///     Terminate the config.
		/// </summary>
		void Terminate();

		/// <summary>
		///     Cancel the config
		/// </summary>
		void Cancel();

		/// <summary>
		///     Ok the config
		/// </summary>
		void Ok();
	}

	/// <summary>
	///     This is the generic interface for a config implementation
	/// </summary>
	public interface IConfig<out TConfigScreen> : IConfig
	{
		/// <summary>
		///     The IConfigScreen items of the config
		/// </summary>
		new IEnumerable<TConfigScreen> ConfigScreens { get; }

		/// <summary>
		///     Returns the current config screen
		/// </summary>
		new TConfigScreen CurrentConfigScreen { get; }
	}
}