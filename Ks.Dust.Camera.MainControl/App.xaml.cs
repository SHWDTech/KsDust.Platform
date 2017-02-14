using System.Windows;
using Ks.Dust.Camera.MainControl.Application;

namespace Ks.Dust.Camera.MainControl
{
    partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Config.ConfigCheck();
            base.OnStartup(e);
        }
    }
}
