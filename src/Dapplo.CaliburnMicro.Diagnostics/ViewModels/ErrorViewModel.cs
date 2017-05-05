using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace Dapplo.CaliburnMicro.Diagnostics.ViewModels
{
    /// <summary>
    /// This view model shows the error that occurred
    /// </summary>
    [Export]
    public class ErrorViewModel : Screen
    {
        [Import]
        private IVersionProvider VersionProvider { get; set; }

        /// <summary>
        /// Checks if the current version is the latest
        /// </summary>
        public bool IsMostRecent => VersionProvider.Current.Equals(VersionProvider.Latest);
    }
}
