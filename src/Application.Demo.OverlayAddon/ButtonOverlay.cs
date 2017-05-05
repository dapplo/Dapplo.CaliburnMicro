using System.ComponentModel.Composition;
using System.Windows.Controls;
using Dapplo.CaliburnMicro.Overlays;

namespace Application.Demo.OverlayAddon
{
    [Export("demo", typeof(IOverlay))]
    public class ButtonOverlay : Button, IOverlay
    {
        public double Left { get; } = 100;
        public double Top { get; } = 100;

        public ButtonOverlay()
        {
            Width = 100;
            Height = 20;
            Content = "Click me";
        }
    }
}
