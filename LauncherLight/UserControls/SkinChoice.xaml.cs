using ACSharedMemory.Models.Car;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LauncherLight.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ServerList.xaml
    /// </summary>
    public partial class SkinChoice : UserControl
    {
        public SkinChoice()
        {
            InitializeComponent();
        }

        public Action<Skin> SkinSelected;
        public Action Cancel;

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                SkinSelected((sender as FrameworkElement).DataContext as Skin);
            }
        }

        public void SetSkins(IEnumerable<Skin> skins)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.lstSkins.DataContext = skins;
            });
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstSkins.SelectedValue != null)
            {
                SkinSelected(lstSkins.SelectedValue as Skin);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void SkinItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender as FrameworkElement != null && e.ClickCount == 2)
            {
                SkinSelected((sender as FrameworkElement).DataContext as Skin);
            }
        }
    }
}