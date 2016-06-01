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
using Dapplo.CaliburnMicro.Demo.Models;

#endregion

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	/// <summary>
	///     A view model for credentials (username / password)
	/// </summary>
	[Export]
	public class CredentialsViewModel : Screen
	{
		private string _password;
		private string _username;

		/// <summary>
		///     Can the login button be pressed?
		/// </summary>
		public bool CanLogin => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

		[Import]
		public ICredentialsTranslations CredentialsTranslations { get; set; }

		[Import]
		public ICoreTranslations CoreTranslations { get; set; }

		[Import]
		public DummyViewModel DummyVm { get; set; }

		/// <summary>
		///     Password for a login
		/// </summary>
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		/// <summary>
		///     Username for a login
		/// </summary>
		public string Username
		{
			get { return _username; }
			set
			{
				_username = value;
				NotifyOfPropertyChange(() => Username);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		public void Login()
		{
			TryClose(true);
		}

		public void Cancel()
		{
			TryClose(false);
		}
	}
}