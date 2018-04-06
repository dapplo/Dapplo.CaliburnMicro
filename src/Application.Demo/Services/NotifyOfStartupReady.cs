using System.ComponentModel.Composition;
using Application.Demo.UseCases.Toast.ViewModels;
using Dapplo.CaliburnMicro;
using Dapplo.CaliburnMicro.Toasts;

namespace Application.Demo.Services
{
    /// <summary>
    /// Shows a toast when the application starts
    /// </summary>
    [UiStartupAction(StartupOrder = int.MaxValue)]
    public class NotifyOfStartupReady : IUiStartupAction
    {
        private readonly ToastConductor _toastConductor;
        private readonly StartupReadyToastViewModel _startupReadyToastViewModel;

        [ImportingConstructor]
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
