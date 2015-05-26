using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Properties.Settings.Default.Save();
            this.Close();

        }
    }
}
