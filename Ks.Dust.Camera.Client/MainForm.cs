using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.Client.App;
using Ks.Dust.Camera.Client.Views;
using Ks.Dust.Camera.MainControl.Camera;
using Ks.Dust.Camera.MainControl.Storage;
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

        //private HikIpc _contorlSdk;

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
            _selectedCameraLogin = CameraLogins.FirstOrDefault(obj => obj.DeviceGuid == node.Id);
            if (node.Category == AverageCategory.Device && _selectedCameraLogin != null)
            {
                TxtSelectedCamera.Text = node.Name;
                BtnConnect.Enabled = true;

            }
            else
            {
                TxtSelectedCamera.Text = string.Empty;
                BtnConnect.Enabled = false;
            }
        }
    }
}
