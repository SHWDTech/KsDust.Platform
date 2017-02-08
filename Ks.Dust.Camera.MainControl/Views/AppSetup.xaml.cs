using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ks.Dust.Camera.MainControl.Application;

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
                Dispatcher.Invoke(new Action(LoadConfigs));
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
            }

            BtnConfirm.IsEnabled = BtnApply.IsEnabled = true;
            LblInformation.Content = "更新完毕。";
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
    }
}
