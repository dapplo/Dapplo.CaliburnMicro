using System.ComponentModel.Composition;
using System.Deployment.Application;
using System.Windows;
using Dapplo.CaliburnMicro.ClickOnce;

namespace Application.Demo.ClickOnce
{
    [Export(typeof(IHandleClickOnceRestarts))]
    public class HandleClickOnceRestarts : IHandleClickOnceRestarts
    {
        [Import]
        private IClickOnceService ClickOnceService { get; set; }
        public void HandleRestart(UpdateCheckInfo updateCheckInfo)
        {
            if (MessageBox.Show($"Update {updateCheckInfo.AvailableVersion} was found, do you want to restart?", "Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ClickOnceService.Restart();
            }
        }
    }
}
