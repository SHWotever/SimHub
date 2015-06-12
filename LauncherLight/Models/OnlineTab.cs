using PropertyChanged;
using System.Timers;

namespace LauncherLight.Models
{
    [ImplementPropertyChanged]
    public class OnlineTab
    {
        public ACServer CurrentServer { get; set; }

        private Timer timer = new Timer();

        public OnlineTab()
        {
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentServer != null)
            {
                timer.Enabled = false;
                try
                {
                    Helpers.RefreshServer(CurrentServer, Properties.Settings.Default.SteamID, false);
                }
                finally
                {
                    timer.Enabled = true;
                }
            }
            //throw new NotImplementedException();
        }
    }
}