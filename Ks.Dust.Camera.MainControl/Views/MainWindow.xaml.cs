using System.Collections.Generic;
using System.Windows;
using Dust.Platform.Storage.Model;

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
    }
}
