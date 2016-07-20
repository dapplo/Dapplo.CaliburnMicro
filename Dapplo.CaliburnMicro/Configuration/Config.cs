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
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Dapplo.Log.Facade;
using System.Collections.ObjectModel;
using Dapplo.CaliburnMicro.Tree;

#endregion

namespace Dapplo.CaliburnMicro.Configuration
{
	/// <summary>
	///     This implements a Caliburn-Micro Config UI
	/// </summary>
	public abstract class Config<TConfigScreen> : Conductor<TConfigScreen>.Collection.OneActive, IConfig<TConfigScreen>
		where TConfigScreen : class, IConfigScreen, ITreeNode<TConfigScreen>
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		///     This is called when the config needs to initialize stuff, it will call Initialize on every screen
		/// </summary>
		public virtual void Initialize()
		{
			Log.Verbose().WriteLine("Initializing config");

			foreach (var configScreen in ConfigScreens)
			{
				configScreen.Initialize(this);
			}
		}

		/// <summary>
		///     This is called when the config needs to cleanup things, it will call Terminate on every screen
		/// </summary>
		public virtual void Terminate()
		{
			Log.Verbose().WriteLine("Terminating config");
			foreach (var configScreen in ConfigScreens)
			{
				configScreen.Terminate();
			}
		}

		/// <summary>
		///     The TConfigScreen items of the config
		/// </summary>
		public virtual IEnumerable<TConfigScreen> ConfigScreens { get; set; }

		/// <summary>
		///     This implements IConfig.ConfigScreens via ConfigScreens
		/// </summary>
		IEnumerable<IConfigScreen> IConfig.ConfigScreens
		{
			get { return ConfigScreens; }
			set { ConfigScreens = value as ICollection<TConfigScreen>; }
		}

		/// <summary>
		///     This implements IConfig.TreeItems via ConfigScreens
		/// </summary>
		public virtual ICollection<IConfigScreen> TreeItems { get; } = new ObservableCollection<IConfigScreen>();

		/// <summary>
		///     This return or sets the current config screen
		/// </summary>
		public virtual TConfigScreen CurrentConfigScreen
		{
			get { return ActiveItem; }
			set { ActivateItem(value); }
		}

		/// <summary>
		///     This implements IConfig.CurrentConfigScreen via CurrentConfigScreen
		/// </summary>
		IConfigScreen IConfig.CurrentConfigScreen
		{
			get { return CurrentConfigScreen; }
			set { CurrentConfigScreen = value as TConfigScreen; }
		}

		/// <summary>
		///     This will call TryClose with false if all IConfigScreen items are okay with closing
		/// </summary>
		public virtual void Cancel()
		{
			if (CanCancel)
			{
				TryClose(false);
			}
		}

		/// <summary>
		///     check every IConfigScreen if it can close
		/// </summary>
		public virtual bool CanCancel
		{
			get
			{
				var result = true;
				CanClose(b => result = b);
				return result;
			}
		}

		/// <summary>
		///     This will call TryClose with true if all IConfigScreen items are okay with closing
		/// </summary>
		public virtual void Ok()
		{
			if (CanOk)
			{
				TryClose(true);
			}
		}

		/// <summary>
		///     check every IConfigScreen if it can close
		/// </summary>
		public virtual bool CanOk
		{
			get
			{
				var result = true;
				CanClose(b => result = b);
				return result;
			}
		}

		/// <summary>
		/// Called to check whether or not this instance can close.
		/// </summary>
		/// <param name="callback">The implementor calls this action with the result of the close check.</param>
		public override void CanClose(Action<bool> callback)
		{
			var result = true;
			foreach (var configScreen in ConfigScreens)
			{
				configScreen.CanClose(canClose => result &= canClose);
			}
			callback(result);
		}

		/// <summary>
		///     Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to
		///     close.
		///     Also provides an opportunity to pass a dialog result to it's corresponding view.
		/// </summary>
		/// <param name="dialogResult">The dialog result.</param>
		public override void TryClose(bool? dialogResult = null)
		{
			// Terminate needs to be called before TryClose, otherwise our items are gone.
			Terminate();
			base.TryClose(dialogResult);
		}

		/// <summary>Called when activating.</summary>
		protected override void OnActivate()
		{
			Items.AddRange(ConfigScreens);
			
			// Build a tree for the ConfigScreens
			foreach (var configScreen in ConfigScreens.CreateTree())
			{
				TreeItems.Add(configScreen);
			}

			Initialize();

			base.OnActivate();
		}

		/// <summary>
		///     Activates the specified config screen, and sends notify property changed events.
		/// </summary>
		/// <param name="configScreen">The TConfigScreen to activate.</param>
		public override void ActivateItem(TConfigScreen configScreen)
		{
			if (configScreen.CanActivate)
			{
				base.ActivateItem(configScreen);
				NotifyOfPropertyChange(nameof(CurrentConfigScreen));
				NotifyOfPropertyChange(nameof(CanCancel));
				NotifyOfPropertyChange(nameof(CanOk));
			}
		}
	}
}