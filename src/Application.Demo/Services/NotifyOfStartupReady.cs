using System.ComponentModel.Composition;
using Application.Demo.UseCases.Toast.ViewModels;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Toasts;

namespace Application.Demo.Services
{
    /// <summary>
    /// Shows a toast when the application starts
    /// </summary>
    [Export(typeof(IUiService))]
    public class NotifyOfStartupReady : IUiService
    {
        [ImportingConstructor]
        public NotifyOfStartupReady(
            ToastConductor toastConductor,
            StartupReadyToastViewModel startupReadyToastViewModel)
        {
            toastConductor.ActivateItem(startupReadyToastViewModel);
        }
    }
}
