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
            cbUseRename.Visibility = System.Windows.Visibility.Collapsed;
#if DEBUG
            cbUseRename.Visibility = System.Windows.Visibility.Visible;
#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GamePath = txtGamePath.Text;
            Properties.Settings.Default.ServerPath = txtServerPath.Text;
            Properties.Settings.Default.UseRename = cbUseRename.IsChecked.GetValueOrDefault(false);
            Properties.Settings.Default.EnableServerFeature = cbEnableServerFeature.IsChecked.GetValueOrDefault(false);
            Properties.Settings.Default.PlayerName = txtPseudo.Text;
            Properties.Settings.Default.SteamID = txtSteamId.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}