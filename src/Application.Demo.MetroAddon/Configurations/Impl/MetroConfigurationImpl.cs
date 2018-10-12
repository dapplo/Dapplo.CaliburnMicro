using Dapplo.CaliburnMicro.Metro;
using Dapplo.Config.Ini;
using MahApps.Metro.Controls;

namespace Application.Demo.MetroAddon.Configurations.Impl
{
    internal class MetroConfigurationImpl : IniSectionBase<IMetroConfiguration>, IMetroConfiguration
    {
        public HotKey HotKey { get; set; }
        public Themes Theme { get; set; } = Themes.BaseLight;
        public ThemeAccents ThemeAccent { get; set; } = ThemeAccents.Blue;
    }
}
