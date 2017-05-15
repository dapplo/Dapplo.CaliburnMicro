using Caliburn.Micro;
using ToastNotifications.Core;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// This is the base interface for toasts
    /// </summary>
    public interface IToast : IScreen, INotification
    {
    }
}
