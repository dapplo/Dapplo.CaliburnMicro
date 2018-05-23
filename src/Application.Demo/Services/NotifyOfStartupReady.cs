using Application.Demo.UseCases.Toast.ViewModels;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Toasts;

namespace Application.Demo.Services
{
    /// <summary>
    /// Shows a toast when the application starts
    /// </summary>
    [ServiceOrder(int.MaxValue)]
    public class NotifyOfStartupReady : IUiStartup
    {
        private readonly ToastConductor _toastConductor;
        private readonly StartupReadyToastViewModel _startupReadyToastViewModel;

        public NotifyOfStartupReady(
            ToastConductor toastConductor,
            StartupReadyToastViewModel startupReadyToastViewModel)
        {
            _toastConductor = toastConductor;
            _startupReadyToastViewModel = startupReadyToastViewModel;
        }

        /// <inheritdoc />
        public void Start()
        {
            _toastConductor.ActivateItem(_startupReadyToastViewModel);
        }
    }
}
