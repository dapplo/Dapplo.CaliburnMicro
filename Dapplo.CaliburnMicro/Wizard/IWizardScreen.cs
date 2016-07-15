using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	/// Every Step in a wizard should implement this
	/// Some of the wizard functionality is covered in standard Caliburn interfaces, which are supplied by the interfaces which IScreen extends:
	/// IHaveDisplayName: Covers the name and title of the Wizard screen
	/// INotifyPropertyChanged: Makes it possible that changes to e.g. the name are directly represented in the view
	/// IActivate, IDeactivate: to know if the wizard screen is activated or deactivated
	/// IGuardClose.CanClose: Prevents leaving the wizard screen
	/// 
	/// A default implementation is to extend Screen
	/// </summary>
	public interface IWizardScreen : IScreen
	{
		/// <summary>
		/// Returns if the wizard screen can be shown
		/// </summary>
		bool CanShown { get; }
	}
}
