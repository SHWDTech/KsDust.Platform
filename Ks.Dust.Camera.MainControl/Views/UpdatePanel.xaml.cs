using System;
using System.IO;
using System.Net;
using System.Windows;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Storage;
using Newtonsoft.Json;
using SHWDTech.Platform.Utility;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for UpdatePanel.xaml
    /// </summary>
    public partial class UpdatePanel
    {
        private string _errorCode = string.Empty;

        public UpdatePanel()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            base.Show();
            ExecuteUpdate();
        }

        public void ExecuteUpdate()
        {
            LblUpdateMessage.Text = "开始更新摄像头信息。。。";
            LblUpdateMessage.Text = !RequestServerData() 
                ? $"更新数据失败，请检查网络连接或系统设置，错误号：{_errorCode}" 
                : "更新完成。";

            BtnConfirm.IsEnabled = true;
        }

        private bool RequestServerData()
        {
            try
            {
                var client = WebRequest.Create($"{Config.ServerAddress}:{Config.ServerPort}/api/CameraNode");
                var response = (HttpWebResponse)client.GetResponse();
                var stream = response.GetResponseStream();
                if (stream == null) return false;
                using (var reader = new StreamReader(stream))
                {
                    var responseJson = reader.ReadToEnd();
                    UpdateLocalData(responseJson);
                }
            }
            catch (Exception ex)
            {
                _errorCode = $"{DateTime.Now: yyyyMMddHHMMssfff}";
                LogService.Instance.Error($"更新设备信息失败，错误号：{_errorCode}", ex);
                return false;
            }

            return true;
        }

        private void UpdateLocalData(string jsonString)
        {
            using (var writer = new StreamWriter(File.Create(Config.CameraNodesTempJsonFile)))
            {
                var cameraNodes = JsonConvert.DeserializeObject<CameraNodeStorage>(jsonString);
                writer.Write(JsonConvert.SerializeObject(cameraNodes, Formatting.Indented));
                ((MainWindow)Owner).ResfreashCameraNodes(cameraNodes);
            }
            File.Delete(Config.CameraNodesJsonFile);
            File.Move(Config.CameraNodesTempJsonFile, Config.CameraNodesJsonFile);
        }

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Close();
        }
    }
}
