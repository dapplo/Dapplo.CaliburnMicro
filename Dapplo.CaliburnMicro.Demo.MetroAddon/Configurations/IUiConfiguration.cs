using System.ComponentModel;
using Dapplo.CaliburnMicro.Metro;
using Dapplo.Ini;
using Dapplo.InterfaceImpl.Extensions;
using MahApps.Metro.Controls;

namespace Dapplo.CaliburnMicro.Demo.MetroAddon.Configurations
{
	[IniSection("Metro")]
	public interface IUiConfiguration : IIniSection, ITransactionalProperties
	{

		[DefaultValue(Themes.BaseLight)]
		Themes Theme { get; set; }

		[DefaultValue(ThemeAccents.Orange)]
		ThemeAccents ThemeAccent { get; set; }

		HotKey HotKey { get; set; }
	}
}
