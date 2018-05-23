using System.Deployment.Application;
using System.Windows;
using Dapplo.CaliburnMicro.ClickOnce;

namespace Application.Demo.ClickOnce
{
    public class HandleClickOnceRestarts : IHandleClickOnceRestarts
    {
        private readonly IClickOnceService _clickOnceService;

        public HandleClickOnceRestarts(IClickOnceService clickOnceService)
        {
            _clickOnceService = clickOnceService;
        }

        public void HandleRestart(UpdateCheckInfo updateCheckInfo)
        {
            if (MessageBox.Show($"Update {updateCheckInfo.AvailableVersion} was found, do you want to restart?", "Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _clickOnceService.Restart();
            }
        }
    }
}
