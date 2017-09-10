using System;
using System.IO;
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

        protected override void OnExit(ExitEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Config.NotFinishedDownloadFileName))
            {
                try
                {
                    File.Delete(Config.NotFinishedDownloadFileName);
                }
                catch (Exception ex)
                {
                    SimpleLog.Error("delete unfinished file failed.", ex);
                }
            }
            base.OnExit(e);
        }
    }
}
