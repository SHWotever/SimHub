using AssettoCorsaSharedMemory;
using System;
using System.Windows;
using XSockets.Client40;
using XSockets.Client40.Common.Interfaces;

namespace TimingDisplay
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IController chatController { get; set; }

        private const string ServerURI = "ws://cortex:9898";
        private const string Origin = "http://localhost";

        private IXSocketClient Connection { get; set; }

        public MainWindow()
        {
            Connection = new XSocketClient(ServerURI, "http://localhost", "GameData");

            //Using the default timeout
            Connection.SetAutoReconnect();

            InitializeComponent();

            Connection.Controller("GameData").On<DataContainer>("gamedataevent", data => DataReceived(data));

            //    clockDisplay.Time = TimeSpan.FromMilliseconds(-50);

            Connection.OnConnected += Connection_OnConnected;
            Connection.OnDisconnected += Connection_OnDisconnected;
        }

        private void Connection_OnDisconnected(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => { this.Title = "Disconnected"; });
        }

        private void Connection_OnConnected(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() => { this.Title = "Connected"; });
        }

        public void DataReceived(DataContainer data)
        {
            this.Dispatcher.Invoke(() =>
            {
                var text = data.AllTimeDelta.ToString(@"mm\:ss\.ff");
                //if (!text.StartsWith("-"))
                //{
                //    text = "+" + text;
                //}
                //else {
                //}

                if (this.lblTimer.Content as string != text)
                {
                    this.lblTimer.Content = "+" + text;
                }
            });
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Connection.Open();
            }
            catch { }
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.WindowState.Maximized != this.WindowState)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}