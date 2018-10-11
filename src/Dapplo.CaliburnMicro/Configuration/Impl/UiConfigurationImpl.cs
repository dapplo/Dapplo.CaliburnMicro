using Dapplo.Config.Ini;
using Dapplo.Windows.User32.Structs;
using System.Collections.Generic;
using System.Windows;

namespace Dapplo.CaliburnMicro.Configuration.Impl
{
    internal class UiConfigurationImpl : IniSectionBase<IUiConfiguration>, IUiConfiguration
    {
        public WindowStartupLocation DefaultWindowStartupLocation { get; set; }
        public bool AreWindowLocationsStored { get; set; }
        public IDictionary<string, WindowPlacement> WindowLocations { get; set; }
    }
}
