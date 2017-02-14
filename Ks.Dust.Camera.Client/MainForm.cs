using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.Client.App;
using Ks.Dust.Camera.Client.Camera;
using Ks.Dust.Camera.Client.Views;
using Ks.Dust.Camera.MainControl.Camera;
using Newtonsoft.Json;

namespace Ks.Dust.Camera.Client
{
    public partial class MainForm : Form
    {
        private readonly Setup _setupWindow = new Setup();

        private List<CameraNode> CameraNodes { get; set; }

        private List<CameraLogin> CameraLogins { get; set; }

        private CameraLogin _selectedCameraLogin;

        private CameraNode _selectedCameraNode;

        private HikNvr _contorlSdk;

        private string _selectedFile;

        private bool _cameraLoged;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenSetup(object sender, EventArgs e)
        {
            _setupWindow.ShowDialog(this);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            LoadLocalStorage();
            _contorlSdk = new HikNvr();
        }

        private void LoadLocalStorage()
        {
            if (!File.Exists(AppConfig.CameraNodesJsonFile)) return;
            using (var reader = new StreamReader(File.OpenRead(AppConfig.CameraNodesJsonFile)))
            {
                var storage = JsonConvert.DeserializeObject<CameraNodeStorage>(reader.ReadToEnd());
                ResfreashCameraNodes(storage);
            }
        }

        public void ResfreashCameraNodes(CameraNodeStorage storage)
        {
            if (storage == null) return;
            CameraNodes = storage.Nodes;
            cameraNodesTreeView.Nodes.Clear();
            TransferToTreeViewNodes(CameraNodes, cameraNodesTreeView.Nodes);
            CameraLogins = storage.Logins;
        }

        private void TransferToTreeViewNodes(List<CameraNode> cameraNodes, TreeNodeCollection parent)
        {
            foreach (var cameraNode in cameraNodes)
            {
                var node = parent.Add(cameraNode.Id.ToString(), cameraNode.Name);
                node.Tag = cameraNode;
                if (cameraNode.Children != null)
                {
                    TransferToTreeViewNodes(cameraNode.Children, node.Nodes);
                }
            }
        }

        private void OnCameraSelected(object sender, TreeViewEventArgs e)
        {
            var node = (CameraNode)e.Node.Tag;
            _selectedCameraNode = node;
            _selectedCameraLogin = CameraLogins.FirstOrDefault(obj => obj.DeviceGuid == node.Id);
            if (_cameraLoged) return;
            if (node.Category == AverageCategory.Device && _selectedCameraLogin != null)
            {
                TxtSelectedCamera.Text = node.Name;
                BtnCameraLogin.Enabled = true;
            }
            else
            {
                TxtSelectedCamera.Text = string.Empty;
                BtnCameraLogin.Enabled = false;
            }
        }

        private void CameraLogin(object sender, EventArgs e)
        {
            _cameraLoged = _contorlSdk.Login(_selectedCameraLogin);
            if (!_cameraLoged)
            {
                UpdateAppInfo($"摄像头登录失败，错误码：{_contorlSdk.LastErrorCode}");
                return;
            }
            BtnCameraLogin.Enabled = false;
            btnCameraLogoff.Enabled = true;
            lblConnectedCamera.Text = _selectedCameraNode.Name;
            UpdateAppInfo("摄像头登录成功。");
        }

        private void CameraLogOff(object sender, EventArgs e)
        {
            if (!_cameraLoged) return;
            _cameraLoged = !_contorlSdk.Logout();
            if (!_cameraLoged)
            {
                UpdateAppInfo($"摄像头登出失败，错误码：{_contorlSdk.LastErrorCode}");
            }
        }

        private void SearchHistory(object sender, EventArgs e)
        {
            if (!_cameraLoged)
            {
                MessageBox.Show("请先登录摄像头！");
                return;
            }
            lvPlayBack.Items.Clear();
            var struFileCondV40 = new CHCNetSDK.NET_DVR_FILECOND_V40()
            {
                lChannel = _contorlSdk.DefaultChannel,
                dwFileType = 0xFF,
                dwIsLocked = 0xFF,
                struStartTime = new CHCNetSDK.NET_DVR_TIME()
                {
                    dwYear = (uint)startDateTimePicker.Value.Year,
                    dwMonth = (uint)startDateTimePicker.Value.Month,
                    dwDay = (uint)startDateTimePicker.Value.Day,
                    dwHour = (uint)startDateTimePicker.Value.Hour,
                    dwMinute = (uint)startDateTimePicker.Value.Minute,
                    dwSecond = (uint)startDateTimePicker.Value.Second
                },
                struStopTime = new CHCNetSDK.NET_DVR_TIME()
                {
                    dwYear = (uint)endDateTimePicker.Value.Year,
                    dwMonth = (uint)endDateTimePicker.Value.Month,
                    dwDay = (uint)endDateTimePicker.Value.Day,
                    dwHour = (uint)endDateTimePicker.Value.Hour,
                    dwMinute = (uint)endDateTimePicker.Value.Minute,
                    dwSecond = (uint)endDateTimePicker.Value.Second
                }
            };
            var historyList = _contorlSdk.SearchHistory(struFileCondV40);
            foreach (var record in historyList)
            {
                lvPlayBack.Items.Add(new ListViewItem(new[] { record.FileName, record.StartDateTime, record.EndDateTime }));
            }
        }

        private void UpdateAppInfo(string message)
        {
            lblAppInfo.Text = message;
        }

        private void StartPlayBack(object sender, EventArgs e)
        {
            var ret = _contorlSdk.PlayBack(_selectedFile, pbPreviewer.Handle);
            if (ret < 0)
            {
                UpdateAppInfo($"启动回放失败，错误码：{ret}");
                return;
            }
            btnStopPlayback.Enabled = true;
            UpdateAppInfo($"开始回放，文件名：{_selectedFile}");
        }

        private void PlayBackSelected(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedFile))
            {
                btnStartPlayback.Enabled = true;
                btnDownloadPlayback.Enabled = true;
            }
            _selectedFile = lvPlayBack.SelectedItems[0].Text;
        }
    }
}
