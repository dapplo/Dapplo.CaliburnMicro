using ToastNotifications.Core;

namespace Dapplo.CaliburnMicro.Toasts
{
    /// <summary>
    /// This extends NotificationDisplayPart to be able supply the MessageOptions
    /// </summary>
    public abstract class ToastView : NotificationDisplayPart
    {
        /// <summary>
        /// Specify MessageOptions for your toast 
        /// </summary>
        public MessageOptions Options { get; set; }

        /// <inheritdoc />
        public override MessageOptions GetOptions() => Options;
    }
}
