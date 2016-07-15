using System.Collections.Generic;
using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.Demo.Languages;
using Dapplo.CaliburnMicro.Wizard;
using Dapplo.Utils.Extensions;
using System.Linq;

namespace Dapplo.CaliburnMicro.Demo.ViewModels
{
	[Export]
	public class WizardExampleViewModel : Wizard<IWizardScreen>, IPartImportsSatisfiedNotification
	{
		[ImportMany]
		private IEnumerable<IWizardScreen> WizardElements { get; set; }

		[Import]
		private IWizardTranslations WizardTranslations { get; set; }

		public void OnImportsSatisfied()
		{
			WizardTranslations.OnPropertyChanged(propertyName => DisplayName = WizardTranslations.Title);
			// Make sure the order is step1, step2 by ordering on the name
			Items.AddRange(WizardElements.OrderBy(x => x.GetType().Name));
		}
	}
}
