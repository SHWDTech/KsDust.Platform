using System.Windows;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Storage;

namespace Ks.Dust.Camera.MainControl
{
    partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Config.ConfigCheck();
            ApplicationStorage.LoadDownloadFile();
            base.OnStartup(e);
        }
    }
}
