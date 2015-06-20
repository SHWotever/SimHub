using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace LauncherLight
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            txtGamePath.Text = Properties.Settings.Default.GamePath;
            txtServerPath.Text = Properties.Settings.Default.ServerPath;
            cbUseRename.IsChecked = Properties.Settings.Default.UseRename;
            cbEnableServerFeature.IsChecked = Properties.Settings.Default.EnableServerFeature;
            txtPseudo.Text = Properties.Settings.Default.PlayerName;
            txtSteamId.Text = Properties.Settings.Default.SteamID;
            modDays.Value = Properties.Settings.Default.LastModsDays;
            cbUseRename.Visibility = System.Windows.Visibility.Collapsed;
            //#if DEBUG
            cbUseRename.Visibility = System.Windows.Visibility.Visible;
            //#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GamePath = txtGamePath.Text;
            Properties.Settings.Default.ServerPath = txtServerPath.Text;
            Properties.Settings.Default.UseRename = cbUseRename.IsChecked.GetValueOrDefault(false);
            Properties.Settings.Default.EnableServerFeature = cbEnableServerFeature.IsChecked.GetValueOrDefault(false);
            Properties.Settings.Default.PlayerName = txtPseudo.Text;
            Properties.Settings.Default.SteamID = txtSteamId.Text;
            Properties.Settings.Default.LastModsDays = (int)modDays.Value.GetValueOrDefault(0);
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnAutoDetectSteamId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string logfile = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "logs", "launcher.log");
                var lines = File.ReadAllLines(logfile);
                var steamline = lines.FirstOrDefault(i => i.StartsWith("Steam ID:"));
                txtSteamId.Text = string.Join("", steamline.Reverse().Take(17).Reverse());
            }
            catch
            {
                MessageBox.Show("Can't find steam Id");
            }
        }
    }
}