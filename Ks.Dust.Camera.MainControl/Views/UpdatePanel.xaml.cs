using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;
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

            if (!RequestServerData())
            {
                LblUpdateMessage.Text = $"更新数据失败，请检查网络连接或系统设置，错误号：{_errorCode}";
            }

            LblUpdateMessage.Text = "更新完成。";
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
                    SaveJsonFile(responseJson);
                }
            }
            catch (Exception ex)
            {
                _errorCode = $"{DateTime.Now : yyyyMMddHHMMssfff}";
                LogService.Instance.Error($"更新设备信息失败，错误号：{_errorCode}", ex);
                return false;
            }

            return true;
        }

        private void SaveJsonFile(string jsonString)
        {
            using (var writer = new StreamWriter(File.Create($"{Environment.CurrentDirectory}\\Storage\\_cameraNodes_temp.json")))
            {
                var cameraNodes = JsonConvert.DeserializeObject<List<CameraNode>>(jsonString);
                writer.Write(JsonConvert.SerializeObject(cameraNodes, Formatting.Indented));
                ((MainWindow)Owner).ResfreashCameraNodes(cameraNodes);
            }

            File.Delete($"{Environment.CurrentDirectory}\\Storage\\cameraNodes.json");
            File.Move($"{Environment.CurrentDirectory}\\Storage\\_cameraNodes_temp.json", $"{Environment.CurrentDirectory}\\Storage\\cameraNodes.json");
        }

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Close();
        }
    }
}
