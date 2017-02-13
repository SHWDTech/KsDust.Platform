using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Ks.Dust.Camera.Client.App;

namespace Ks.Dust.Camera.Client.Views
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            LoadConfigs();
            BindTextBoxEvent();
        }

        private void BindTextBoxEvent()
        {
            foreach (var textBox in setupParamsPanel.Controls.OfType<TextBox>())
            {
                textBox.TextChanged += SetupParamsChanged;
            }
        }

        private void Close(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupParamsChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void Apply(object sender, EventArgs e)
        {
            foreach (var textBox in setupParamsPanel.Controls.OfType<TextBox>())
            {
                typeof(AppConfig).GetProperty(textBox.Tag.ToString()).SetValue(null, textBox.Text, BindingFlags.Default, null, null, null);
            }

            btnApply.Enabled = false;
        }

        private void Confirm(object sender, EventArgs e)
        {
            if (btnApply.Enabled)
            {
                Apply(sender, e);
            }
            Close();
        }

        private void LoadConfigs()
        {
            foreach (var textBox in setupParamsPanel.Controls.OfType<TextBox>())
            {
                textBox.Text = typeof(AppConfig).GetProperty(textBox.Tag.ToString()).GetValue(null, null).ToString();
            }
        }
    }
}
