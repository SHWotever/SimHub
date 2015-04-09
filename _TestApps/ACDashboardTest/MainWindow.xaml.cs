using ACDashboard;
using System.Windows;

namespace ACDashboardTest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var Dash = new Dashboard();
            Dash.Show();
        }
    }
}