using System.Collections.Generic;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	/// This is the interface for a wizard implementation
	/// </summary>
	public interface IWizard<out TWizardScreen> : IHaveActiveItem
	{
		/// <summary>
		/// The TWizardScreen items of the wizard
		/// </summary>
		IEnumerable<TWizardScreen> WizardScreens { get; }

			/// <summary>
		/// Returns the current wizard screen
		/// </summary>
		TWizardScreen CurrentWizardScreen { get; }

		/// <summary>
		/// Go to the next wizard screen, where CanShown is true, in the list.
		/// </summary>
		bool Next();

		/// <summary>
		/// Can the wizard go to the next wizard screen, where CanShown is true, in the list. (e.g. false if we are at the last)
		/// </summary>
		bool HasNext { get; }

		/// <summary>
		/// Go to the previous wizard screen, where CanShown is true, in the list.
		/// </summary>
		bool Previous();

		/// <summary>
		/// Can the wizard go to the previous CanShown wizard screen? (e.g. false if we are at the first)
		/// </summary>
		bool HasPrevious { get; }

		/// <summary>
		/// Cancel the wizard
		/// </summary>
		void CancelWizard();

		/// <summary>
		/// Can the current wizard "flow" be cancelled?
		/// </summary>
		bool CanCancelWizard { get; }

		/// <summary>
		/// Finish the wizard
		/// </summary>
		void FinishWizard();

		/// <summary>
		/// Test if the wizard can be finished
		/// </summary>
		bool CanFinishWizard { get; }
	}
}
