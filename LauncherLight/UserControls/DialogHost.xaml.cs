using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace LauncherLight.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ServerDialog.xaml
    /// </summary>
    public partial class DialogHost : BaseMetroDialog
    {
        public DialogHost()
        {
            InitializeComponent();
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
        }

        internal DialogHost(MetroWindow parentWindow)
            : this(parentWindow, null)
        {
        }

        internal DialogHost(MetroWindow parentWindow, LoginDialogSettings settings)
            : base(parentWindow, settings)
        {
            InitializeComponent();
        }

        public FrameworkElement ContentControl
        {
            set
            {
                this.grdContentControl.Children.Clear();
                this.grdContentControl.Children.Add(value);
                value.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            }
        }
    }
}