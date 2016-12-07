using System.ComponentModel;
using Dapplo.Language;

namespace Dapplo.CaliburnMicro.Demo.MetroAddon.Configurations
{
	[Language("Ui")]
	public interface IUiTranslations : ILanguage, INotifyPropertyChanged
	{
		string Theme { get; }
		string Hotkey { get; }
	}
}
