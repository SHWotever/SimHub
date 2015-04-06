using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ACDashboard
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private Timer timer = new Timer();

        public Dashboard()
        {
            InitializeComponent();
            this.MouseDoubleClick += Dashboard_MouseDoubleClick;
            timer.Interval = 50;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
           {
               this.lblLCDTime.Text = DateTime.Now.ToString("HH:mm:ss");
           });
        }

        private void Dashboard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //this.AllowsTransparency = false;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
        }

        public void SetData(ACSharedMemory.DataContainer data)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (!data.GameRunning) return;
                PresentationSource source = PresentationSource.FromVisual(this);

                double dpiX = 96;
                double dpiY = 96;
                if (source != null)
                {
                    dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                    dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
                }

                this.speedGauge.CurrentValue = Math.Min(this.speedGauge.MaxValue, data.Physics.SpeedKmh);

                // RPMS
                if (data.StaticInfo.MaxRpm != 0)
                {
                    if ((int)this.rpmGauge.MaxValue != Math.Ceiling((double)data.StaticInfo.MaxRpm / 1000d))
                    {
                        this.rpmGauge.MaxValue = Math.Ceiling((double)data.StaticInfo.MaxRpm / 1000d);
                        this.rpmGauge.MajorDivisionsCount = Math.Ceiling((double)data.StaticInfo.MaxRpm / 1000d);
                    }
                }

                this.rpmGauge.CurrentValue = Math.Max(0d, (double)data.Physics.Rpms / 1000d);
                this.gearLabel.Content = GetGear(data.Physics.Gear);
                this.lblAllTimeBest.Content = data.AllTimeBest.ToString(@"mm\:ss\:fff");
                this.lblSessionTimeBest.Content = data.SessionTimeBest.ToString(@"mm\:ss\:fff");
                this.lblMyTimeBest.Content = data.MyTimeBest.ToString(@"mm\:ss\:fff");
                this.lblTime.Content = DateTime.Now.ToString("HH:mm:ss");
                this.pb2.Value = data.Physics.Gas * 100d;
                this.pb1.Value = data.Physics.Brake * 100d;

                if (data.TrackDesc != null && data.TrackDesc.MapSettings != null)
                {
                    if (!mapImageSource.Equals(data.TrackDesc.MapSettings.MapImagePath, StringComparison.OrdinalIgnoreCase))
                    {
                        mapImage.Source = new BitmapImage(new Uri(data.TrackDesc.MapSettings.MapImagePath));
                        mapImage.Width = mapImage.Source.Width / 96d * (mapImage.Source as BitmapImage).DpiX;
                        mapImage.Height = mapImage.Source.Height / 96d * (mapImage.Source as BitmapImage).DpiY;
                    }
                    mapImageSource = data.TrackDesc.MapSettings.MapImagePath;

                    double imagePositionX = (data.TrackDesc.MapSettings.X_OFFSET + data.Graphics.CarCoordinates[0]) / data.TrackDesc.MapSettings.SCALE_FACTOR;
                    double imagePositionY = (data.TrackDesc.MapSettings.Z_OFFSET + data.Graphics.CarCoordinates[2]) / data.TrackDesc.MapSettings.SCALE_FACTOR;

                    var coordinates = data.Graphics.CarCoordinates;

                    var tt = (TranslateTransform)((TransformGroup)mapCanvas.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    tt.X = -imagePositionX + mapCanvas.ActualWidth / 2;
                    tt.Y = -imagePositionY + mapCanvas.ActualHeight / 2;

                    //var tt2 = (TranslateTransform)((TransformGroup)mapCanvas.RenderTransform).Children.First(tr => tr is TranslateTransform);
                    //tt2.X = mapCanvas.ActualWidth / 2;
                    //tt2.Y = mapCanvas.ActualHeight / 2;

                    if (oldCoordinates != null)
                    {
                        if (coordinates[0] - oldCoordinates[0] != 0 || coordinates[2] - oldCoordinates[2] != 0)
                        {
                            double angle = Vector.AngleBetween(new Vector(0, -1), new Vector(coordinates[0] - oldCoordinates[0], coordinates[2] - oldCoordinates[2]));
                            //mapParentGrid.RenderTransform = new RotateTransform(-angle, mapParentGrid.ActualWidth / 2, mapParentGrid.ActualHeight / 2);

                            var rt = (RotateTransform)((TransformGroup)mapCanvas.RenderTransform)
                                .Children.First(tr => tr is RotateTransform);
                            rt.CenterX = 0;
                            rt.CenterY = 0;
                            rt.Angle = -angle;
                        }
                    }

                    oldCoordinates = data.Graphics.CarCoordinates;

                    //mapImage.Margin = new Thickness(
                    //       -imagePositionX + mapParentGrid.ActualWidth / 2,
                    //       -imagePositionY + mapParentGrid.ActualHeight / 2,
                    //       0,
                    //       0);

                    //this.Title = string.Format("{0}      {1}      {2}      {3}      {4}",
                    //    data.Graphics.CarCoordinates[0].ToString("0.0000000"),
                    //    data.Graphics.CarCoordinates[1].ToString("0.0000000"),
                    //    data.Graphics.CarCoordinates[2].ToString("0.0000000"),
                    //    this.mapImage.Margin.Top,
                    //    this.mapImage.Margin.Left
                    //    );
                    //this.mapScroll.ScrollToHorizontalOffset(data.Graphics.CarCoordinates[0] + data.TrackDesc.MapSettings.X_OFFSET);
                    //this.mapScroll.ScrollToHorizontalOffset(data.Graphics.CarCoordinates[1] + data.TrackDesc.MapSettings.Y_OFFSET);
                }
            });
        }

        private float[] oldCoordinates;
        private string mapImageSource = "";

        private string GetGear(int gear)
        {
            gear = gear - 1;
            if (gear == 0)

                return "N";
            else if (gear == -1)

                return "R";
            else

                return gear.ToString();
        }
    }
}