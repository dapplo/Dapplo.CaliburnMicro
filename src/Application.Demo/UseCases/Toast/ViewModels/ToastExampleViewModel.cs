using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.Toasts.ViewModels;

namespace Application.Demo.UseCases.Toast.ViewModels
{
    [Export]
    public class ToastExampleViewModel : ToastBaseViewModel
    {
        public string Message => "Hello World";
    }
}
