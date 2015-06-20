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
                try
                {
                    foreach (var file in System.IO.Directory.GetFiles("SettingsPresets\\" + lstPresets.SelectedItem.ToString()))
                    {
                        if (!file.ToLower().EndsWith(".merge"))
                        {
                            string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileName(file));
                            System.IO.File.Copy(file, newpath, true);
                        }
                    }
                    foreach (var file in System.IO.Directory.GetFiles("SettingsPresets\\" + lstPresets.SelectedItem.ToString()))
                    {
                        if (file.EndsWith(".merge"))
                        {
                            string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileNameWithoutExtension(file));
                            //System.IO.File.Copy(file, newpath, true);
                            Helpers.MergeIniFiles(file, newpath);
                        }
                    }
                }
                catch { }

                Cancel();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void btnCreatePreset_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}