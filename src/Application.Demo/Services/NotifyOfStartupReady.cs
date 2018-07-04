using Application.Demo.UseCases.Toast.ViewModels;
using Dapplo.Addons;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Toasts;

namespace Application.Demo.Services
{
    /// <summary>
    /// Shows a toast when the application starts
    /// </summary>
    [Service(nameof(NotifyOfStartupReady), nameof(CaliburnStartOrder.ToastConductor), TaskSchedulerName = "ui")]
    public class NotifyOfStartupReady : IStartup
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
        public void Startup()
        {
            _toastConductor.ActivateItem(_startupReadyToastViewModel);
        }
    }
}
