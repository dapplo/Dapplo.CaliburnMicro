using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.Utils.Extensions;
using Caliburn.Micro;

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	[Export(typeof(IWizardScreen))]
	public class WizardStep2ViewModel : Screen, IWizardScreen, IPartImportsSatisfiedNotification
	{
		[Import]
		private IWizardTranslations WizardTranslations { get; set; }

		public void OnImportsSatisfied()
		{
			WizardTranslations.OnPropertyChanged(propertyName => DisplayName = WizardTranslations.TitleStep2);
		}

		public bool CanShown
		{
			get { return true; }
		}
	}
}
