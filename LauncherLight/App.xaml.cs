using System.Windows;

namespace LauncherLight
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //OnlineWindow wnd = new OnlineWindow();
            MainWindow wnd = new MainWindow();
            wnd.Show();
        }
    }
}