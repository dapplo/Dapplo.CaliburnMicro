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
using System.Linq;
using Caliburn.Micro;
using Dapplo.Log.Facade;

#endregion

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	///     This implements a wizard Caliburn-Micro style
	/// </summary>
	public abstract class Wizard<TWizardScreen> : Conductor<TWizardScreen>.Collection.OneActive, IWizard<TWizardScreen> where TWizardScreen : class, IWizardScreen
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly LogSource Log = new LogSource();

		/// <summary>
		/// This is called when the wizard needs to initialize stuff
		/// </summary>
		public virtual void Initialize()
		{
			Log.Verbose().WriteLine("Initializing wizard");
			foreach (var wizardScreen in WizardScreens)
			{
				wizardScreen.Initialize(this);
			}
		}

		/// <summary>
		/// This is called when the wizard needs to cleanup things
		/// </summary>
		public virtual void Terminate()
		{
			Log.Verbose().WriteLine("Terminating wizard");
			foreach (var wizardScreen in WizardScreens)
			{
				wizardScreen.Terminate();
			}
		}

		/// <summary>
		/// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
		/// Also provides an opportunity to pass a dialog result to it's corresponding view.
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
			Initialize();
			base.OnActivate();
		}

		/// <summary>
		///     Activates the specified item, and sends notify property changed events.
		/// </summary>
		/// <param name="item">The TWizardScreen to activate.</param>
		public override void ActivateItem(TWizardScreen item)
		{
			base.ActivateItem(item);
			NotifyOfPropertyChange(nameof(CurrentWizardScreen));
			NotifyOfPropertyChange(nameof(CanNext));
			NotifyOfPropertyChange(nameof(CanPrevious));
			NotifyOfPropertyChange(nameof(CanCancel));
			NotifyOfPropertyChange(nameof(CanFinish));
		}

		/// <summary>
		///     The TWizardScreen items of the wizard
		/// </summary>
		public IEnumerable<TWizardScreen> WizardScreens => Items;

		/// <summary>
		///     The TWizardScreen items of the wizard
		/// </summary>
		IEnumerable<IWizardScreen> IWizard.WizardScreens => Items;

		/// <summary>
		///     This is returns ActiveItem which is typed.
		///     IHaveActiveItem from Caliburn.Micro, which could be used instead, doesn't have a type.
		/// </summary>
		public TWizardScreen CurrentWizardScreen => ActiveItem;

		/// <summary>
		///     This is returns ActiveItem which is typed.
		///     IHaveActiveItem from Caliburn.Micro, which could be used instead, doesn't have a type.
		/// </summary>
		IWizardScreen IWizard.CurrentWizardScreen => ActiveItem;

		/// <summary>
		///     Changes the ActiveItem of the conductor to the next IWizardScreen
		/// </summary>
		/// <returns>bool if next was possible</returns>
		public virtual bool Next()
		{
			// Skip as long as there is a CurrentWizardScreen, and the item is not the current, skip 1 (the current) and skip as long as the item can not be shown.
			// Take the first available.
			var nextWizardScreen = WizardScreens.SkipWhile(w => w != CurrentWizardScreen).Skip(1).SkipWhile(w => !w.IsEnabled || !w.IsVisible).FirstOrDefault();
			if (nextWizardScreen != null)
			{
				ActivateItem(nextWizardScreen);
				return true;
			}
			return false;
		}

		/// <summary>
		///     This returns true if there is a IWizardScreen after the current
		/// </summary>
		/// <returns>true if next can be called</returns>
		public virtual bool CanNext
		{
			get
			{
				// Skip as long as there is a CurrentWizardScreen, and the item is not the current, skip 1 (the current) and skip as long as the item can not be shown.
				// Return if there is anything left 
				return
					WizardScreens.SkipWhile(w => CurrentWizardScreen != null && w != CurrentWizardScreen).Skip(1).SkipWhile(w => !w.IsEnabled || !w.IsVisible).Any();
			}
		}

		/// <summary>
		///     Goto the previous
		/// </summary>
		/// <returns>bool if previous was possible</returns>
		public virtual bool Previous()
		{
			// Take until 
			var previousWizardScreen = CurrentWizardScreen != null
				? WizardScreens.TakeWhile(w => w != CurrentWizardScreen).LastOrDefault(w => w.IsEnabled && w.IsVisible)
				: null;
			if (previousWizardScreen != null)
			{
				ActivateItem(previousWizardScreen);
				return true;
			}
			return false;
		}

		/// <summary>
		///     Is there a previous WizardScreen?
		/// </summary>
		/// <returns></returns>
		public virtual bool CanPrevious
		{
			get { return CurrentWizardScreen != null && WizardScreens.TakeWhile(w => w != CurrentWizardScreen).Any(w => w.IsEnabled && w.IsVisible); }
		}

		/// <summary>
		///     This will call TryClose with false if all IWizardScreen items are okay with closing
		/// </summary>
		public virtual void Cancel()
		{
			if (CanCancel)
			{
				TryClose();
			}
		}

		/// <summary>
		///     check every IWizardScreen if it can close
		/// </summary>
		public virtual bool CanCancel
		{
			get
			{
				var result = true;
				foreach (var wizardScreen in WizardScreens)
				{
					wizardScreen.CanClose(canClose => result &= canClose);
				}
				return result;
			}
		}

		/// <summary>
		///     This will call TryClose with true if all IWizardScreen items are okay with closing
		/// </summary>
		public virtual void Finish()
		{
			if (CanFinish)
			{
				TryClose();
			}
		}

		/// <summary>
		///     check every IWizardScreen if it can close
		/// </summary>
		public virtual bool CanFinish => CanCancel;
	}
}