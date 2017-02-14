using System;
using System.Windows.Forms;
using Ks.Dust.Camera.Client.Camera;

namespace Ks.Dust.Camera.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!HikNvr.Initial())
            {
                MessageBox.Show("初始化视频控件失败，请尝试重新启动！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
