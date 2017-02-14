using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Ks.Dust.Camera.MainControl.Application;
using TextBox = System.Windows.Controls.TextBox;

namespace Ks.Dust.Camera.MainControl.Views
{
    /// <summary>
    /// Interaction logic for AppSetup.xaml
    /// </summary>
    public partial class AppSetup
    {
        public AppSetup()
        {
            InitializeComponent();
            Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(LoadConfigs);
            });
        }

        private void LoadConfigs()
        {
            foreach (var textBox in WindowsExtensionMethod.FindVisualChildren<TextBox>(this))
            {
                textBox.Text = typeof(Config).GetProperty(textBox.Tag.ToString()).GetValue(null, null).ToString();
            }
            FinishLoading();
        }

        private void FinishLoading()
        {
            foreach (var textBox in WindowsExtensionMethod.FindVisualChildren<TextBox>(this))
            {
                textBox.IsEnabled = true;
                textBox.TextChanged += ParamsChanged;
            }
        }

        private void ParamsChanged(object sender, TextChangedEventArgs e)
        {
            BtnApply.IsEnabled = BtnConfirm.IsEnabled = true;
        }

        private void OnClose(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void Confirm(object sender, RoutedEventArgs args)
        {
            Apply(sender, args);
            OnClose(sender, args);
        }

        private void Apply(object sender, RoutedEventArgs args)
        {
            foreach (var textBox in WindowsExtensionMethod.FindVisualChildren<TextBox>(this))
            {
                typeof(Config).GetProperty(textBox.Tag.ToString()).SetValue(null, textBox.Text, BindingFlags.Default, null, null, null);
            }

            LblInformation.Content = "设置已保存。";
        }

        private void SetVedioStorageDirectory(object sender, RoutedEventArgs args)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                TxtVedioStorageDir.Text = dialog.SelectedPath;
            }
        }
    }
}
