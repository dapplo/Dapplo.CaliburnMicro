using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	/// This implements a wizard Caliburn-Micro style
	/// </summary>
	public abstract class Wizard<TWizardScreen> : Conductor<TWizardScreen>.Collection.OneActive, IWizard<TWizardScreen> where TWizardScreen : class, IWizardScreen
	{
		/// <summary>
		/// The TWizardScreen items of the wizard
		/// </summary>
		public IEnumerable<TWizardScreen> WizardScreens => Items;

		/// <summary>
		/// This is returns ActiveItem which is typed.
		/// IHaveActiveItem from Caliburn.Micro, which could be used instead, doesn't have a type.
		/// </summary>
		public TWizardScreen CurrentWizardScreen => ActiveItem;

		/// <summary>
		/// Changes the ActiveItem of the conductor to the next IWizardScreen
		/// </summary>
		/// <returns></returns>
		public virtual bool Next()
		{
			// Skip as long as there is a CurrentWizardScreen, and the item is not the current, skip 1 (the current) and skip as long as the item can not be shown.
			// Take the first available.
			var nextWizardScreen = WizardScreens.SkipWhile(w => w != CurrentWizardScreen).Skip(1).SkipWhile(w => !w.CanShown).FirstOrDefault();
			if (nextWizardScreen != null)
			{
				ActivateItem(nextWizardScreen);
				return true;
			}
			return false;
		}

		/// <summary>
		/// This returns true if there is a IWizardScreen after the current
		/// </summary>
		/// <returns>true if next can be called</returns>
		public virtual bool HasNext
		{
			get
			{
				// Skip as long as there is a CurrentWizardScreen, and the item is not the current, skip 1 (the current) and skip as long as the item can not be shown.
				// Return if there is anything left 
				return WizardScreens.SkipWhile(w => CurrentWizardScreen != null && w != CurrentWizardScreen).Skip(1).SkipWhile(w => !w.CanShown).Any();
			}
		}

		/// <summary>
		/// Goto the previous
		/// </summary>
		public virtual bool Previous()
		{

			// Take until 
			var previousWizardScreen = CurrentWizardScreen != null ? WizardScreens.TakeWhile(w => w != CurrentWizardScreen && w.CanShown).LastOrDefault() : null;
			if (previousWizardScreen != null)
			{
				ActivateItem(previousWizardScreen);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Is there a previous WizardScreen?
		/// </summary>
		/// <returns></returns>
		public virtual bool HasPrevious
		{
			get
			{
				return CurrentWizardScreen != null && WizardScreens.TakeWhile(w => w != CurrentWizardScreen && w.CanShown).Any();
			}
		}

		/// <summary>
		/// This will call TryClose with false if all IWizardScreen items are okay with closing
		/// </summary>
		public virtual void CancelWizard()
		{
			if (CanCancelWizard)
			{
				TryClose(false);
			}
		}

		/// <summary>
		/// check every IWizardScreen if it can close
		/// </summary>
		public virtual bool CanCancelWizard
		{
			get
			{
				bool result = true;
				foreach (var wizardScreen in Items)
				{
					wizardScreen.CanClose(canClose => result &= canClose);
				}
				return result;
			}
		}

		/// <summary>
		/// This will call TryClose with true if all IWizardScreen items are okay with closing
		/// </summary>
		public virtual void FinishWizard()
		{
			if (CanFinishWizard)
			{
				TryClose(true);
			}
		}

		/// <summary>
		/// check every IWizardScreen if it can close
		/// </summary>
		public virtual bool CanFinishWizard
		{
			get { return CanCancelWizard; }
		}
	}
}
