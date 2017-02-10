using System.Collections.Generic;
using System.IO;
using System.Windows;
using Dust.Platform.Storage.Model;
using Ks.Dust.Camera.MainControl.Application;
using Ks.Dust.Camera.MainControl.Storage;
using Newtonsoft.Json;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public List<CameraNode> CameraNodes { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadLocalStorage();
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

        public void ResfreashCameraNodes(List<CameraNode> nodes)
        {
            if (nodes == null) return;
            CameraNodes = nodes;
            TrCamera.ItemsSource = CameraNodes;
        }

        private void OnNodeSelected(object sender, RoutedEventArgs args)
        {
            var node = (CameraNode)TrCamera.SelectedItem;
            if (node.Category == AverageCategory.Device)
            {
                TxtSelectedCamera.Text = node.Name;
                BtnConnect.IsEnabled = true;
            }
            else
            {
                BtnConnect.IsEnabled = false;
            }
        }

        private void LoadLocalStorage()
        {
            if (!File.Exists(Config.CameraNodesJsonFile)) return;
            using (var reader = new StreamReader(File.OpenRead(Config.CameraNodesJsonFile)))
            {
                var nodes = JsonConvert.DeserializeObject<CameraNodeStorage>(reader.ReadToEnd());
                ResfreashCameraNodes(nodes.Nodes);
            }
        }
    }
}
