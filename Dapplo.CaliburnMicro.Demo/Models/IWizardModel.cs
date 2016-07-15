using System.ComponentModel;

namespace Dapplo.CaliburnMicro.Demo.Models
{
	/// <summary>
	/// This is the model which is used by the wizard steps
	/// </summary>
	public interface IWizardModel : INotifyPropertyChanged
	{
		string Name { get; set; }
		string Age { get; set; }
	}
}
