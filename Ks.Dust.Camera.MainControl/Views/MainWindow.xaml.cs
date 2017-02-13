using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Camera;
using Ks.Dust.Camera.MainControl.Storage;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<CameraNode> CameraNodes { get; set; }

        private List<CameraLogin> CameraLogins { get; set; }

        private CameraLogin _selectedCameraLogin;

        private CameraNode _selectedCameraNode;

        private HikIpc _contorlSdk;

        private bool _cameraLoged;

        public MainWindow()
        {
            InitializeComponent();
            LoadLocalStorage();
            InitSdk();
        }

        private void InitSdk()
        {
            if (!HikIpc.Initial())
            {
                MessageBox.Show("初始化摄像头模块失败，请尝试重新启动！","警告！",MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
            }
        }

        private void OpenConfigWindow(object sender, RoutedEventArgs args)
        {
            var configWindow = new AppSetup()
            {
                Owner = this
            };
            configWindow.ShowDialog();
        }

        private void UpdateCameraNodes(object sender, RoutedEventArgs args)
        {
            var updatePanel = new UpdatePanel() {Owner = this};
            updatePanel.Show();
        }

        public void ResfreashCameraNodes(CameraNodeStorage storage)
        {
            if (storage == null) return;
            CameraNodes = storage.Nodes;
            TrCamera.ItemsSource = CameraNodes;
            CameraLogins = storage.Logins;
        }

        private void OnNodeSelected(object sender, RoutedEventArgs args)
        {
            var node = (CameraNode)TrCamera.SelectedItem;
            _selectedCameraNode = node;
            _selectedCameraLogin = CameraLogins.FirstOrDefault(obj => obj.DeviceGuid == node.Id);
            if (node.Category == AverageCategory.Device && _selectedCameraLogin != null)
            {
                TxtSelectedCamera.Text = node.Name;
                BtnConnect.IsEnabled = true;
                
            }
            else
            {
                TxtSelectedCamera.Text = string.Empty;
                BtnConnect.IsEnabled = false;
            }
        }

        private void LoginCamera(object sender, RoutedEventArgs args)
        {
            if (_selectedCameraLogin == null) return;
            if (_cameraLoged)
            {
                _cameraLoged = !_contorlSdk.Logout();
                if (_cameraLoged)
                {
                    MessageBox.Show("摄像头退出登录失败，请重新尝试！","提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            _contorlSdk = new HikIpc();
            _cameraLoged = _contorlSdk.Login(_selectedCameraLogin);
            BtnConnect.IsEnabled = !_cameraLoged;
            LblConnectStatus.Content = _cameraLoged ? _selectedCameraNode.Name : string.Empty;
        }

        private void LoadLocalStorage()
        {
            if (!File.Exists(Config.CameraNodesJsonFile)) return;
            using (var reader = new StreamReader(File.OpenRead(Config.CameraNodesJsonFile)))
            {
                var storage = JsonConvert.DeserializeObject<CameraNodeStorage>(reader.ReadToEnd());
                ResfreashCameraNodes(storage);
            }
        }

        private void SearchHistory(object sender, RoutedEventArgs args)
        {
            BtnSearchHistory.IsEnabled = false;
            LvHistory.ItemsSource = null;//清空文件列表 
            if (DpStart.Value == null || DpEnd.Value == null)
            {
                MessageBox.Show("请选择开始时间和结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var struFileCondV40 = new CHCNetSDK.NET_DVR_FILECOND_V40
            {
                lChannel = _contorlSdk.DefaultChannel,
                dwFileType = 0xff,
                dwIsLocked = 0xff,
                struStartTime =
                {
                    dwYear = (uint) DpStart.Value.Value.Year,
                    dwMonth = (uint) DpStart.Value.Value.Month,
                    dwDay = (uint) DpStart.Value.Value.Day,
                    dwHour = (uint) DpStart.Value.Value.Hour,
                    dwMinute = (uint) DpStart.Value.Value.Minute,
                    dwSecond = (uint) DpStart.Value.Value.Second
                },
                struStopTime =
                {
                    dwYear = (uint) DpEnd.Value.Value.Year,
                    dwMonth = (uint) DpEnd.Value.Value.Month,
                    dwDay = (uint) DpEnd.Value.Value.Day,
                    dwHour = (uint) DpEnd.Value.Value.Hour,
                    dwMinute = (uint) DpEnd.Value.Value.Minute,
                    dwSecond = (uint) DpEnd.Value.Value.Second
                }
            };

            var records = _contorlSdk.SearchHistory(struFileCondV40);

            LvHistory.ItemsSource = records;
            BtnSearchHistory.IsEnabled = true;
        }

        private void StartPlayBack(object sender, RoutedEventArgs args)
        {
            if (!(LvHistory.SelectedItem is CameraHistoryRecord))
            {
                MessageBox.Show("必须选择一个记录！", "提示！", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            var record = (CameraHistoryRecord) LvHistory.SelectedItem;
            Dispatcher.Invoke(() =>
            {
                var playret = _contorlSdk.PlayBack(record.FileName, PbPreview.Handle);
                if (playret > 0)
                {
                    BtnPlayHistory.IsEnabled = false;
                }
            });
        }
    }
}
