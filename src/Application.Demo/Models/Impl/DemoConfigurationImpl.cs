using Dapplo.Config.Ini;

namespace Application.Demo.Models.Impl
{
    public class DemoConfigurationImpl : IniSectionBase<IDemoConfiguration>, IDemoConfiguration
    {
        public string Language { get; set; }
    }
}
