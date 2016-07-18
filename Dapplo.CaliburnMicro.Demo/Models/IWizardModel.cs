using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapplo.CaliburnMicro.Demo.Languages;

namespace Dapplo.CaliburnMicro.Demo.Models
{
	/// <summary>
	/// This is the model which is used by the wizard steps
	/// </summary>
	public interface IWizardModel : INotifyPropertyChanged
	{
		[RegularExpression("[A-Z][a-z ]*", ErrorMessage = "Name ", ErrorMessageResourceName = nameof(IValidationErrors.Name), ErrorMessageResourceType = typeof(IValidationErrors))]
		string Name { get; set; }
		string Age { get; set; }
	}
}
