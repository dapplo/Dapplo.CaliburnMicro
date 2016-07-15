namespace Dapplo.CaliburnMicro.Wizard
{
	/// <summary>
	/// Have your IWizardScreen implement this interface to know of the wizard it belongs to
	/// </summary>
	public interface IWizardAware<TWizardScreen>
	{
		/// <summary>
		/// Get the wizard the current IWizardScreen belongs to
		/// </summary>
		IWizard<TWizardScreen> Wizard { get; set; }
	}
}
