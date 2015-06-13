using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LauncherLight.UserControls
{
    /// <summary>
    /// Logique d'interaction pour SettingsPresets.xaml
    /// </summary>
    public partial class SettingsPresets : UserControl
    {
        public Action Cancel;

        public SettingsPresets()
        {
            InitializeComponent();

            if (!Directory.Exists("SettingsPresets"))
            {
                Directory.CreateDirectory("SettingsPresets");
            }

            this.lstPresets.DataContext = Directory.GetDirectories("SettingsPresets").Select(i => Path.GetFileName(i));
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstPresets.SelectedValue != null)
            {
                Cancel();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }
    }
}