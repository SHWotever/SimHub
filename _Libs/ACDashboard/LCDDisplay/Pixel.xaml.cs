using System.Windows.Controls;

namespace ACDashboard.LCDDisplay
{
    /// <summary>
    /// Logique d'interaction pour Pixel.xaml
    /// </summary>
    public partial class Pixel : UserControl
    {
        public Pixel()
        {
            InitializeComponent();
            SetStatus(false);
        }

        private bool CurrentStatus = true;

        public void SetStatus(bool status)
        {
            if (CurrentStatus != status)
            {
                if (!status)
                {
                    this.PixelGrid.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.PixelGrid.Visibility = System.Windows.Visibility.Visible;
                }
                CurrentStatus = status;
            }
        }
    }
}