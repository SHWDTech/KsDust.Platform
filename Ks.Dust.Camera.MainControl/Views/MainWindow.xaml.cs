using System.Windows;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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
    }
}
