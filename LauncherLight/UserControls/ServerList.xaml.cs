using LauncherLight.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LauncherLight.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ServerList.xaml
    /// </summary>
    public partial class ServerList : UserControl
    {
        //[ImplementPropertyChanged]
        private ServerListModel datac = new ServerListModel();

        public ServerList()
        {
            InitializeComponent();
            datac = new ServerListModel();
            this.DataContext = datac;
        }

        private CollectionViewSource view;

        public void SetSevers(List<ACServer> servers)
        {
            this.Dispatcher.Invoke(() =>
            {
                var s = servers.Where(i => !i.MissingContent);

                datac.View = new CollectionViewSource();
                datac.View.Filter += datac.view_Filter;
                datac.View.SortDescriptions.Add(new SortDescription("name", ListSortDirection.Ascending));
                datac.View.Source = new ObservableCollection<ACServer>(servers);
            });
        }

        public void SetLanServers(List<ACServer> servers)
        {
            this.Dispatcher.Invoke(() =>
              {
                  foreach (var server in servers)
                      (datac.View.Source as ObservableCollection<ACServer>).Add(server);

                  datac.View.View.Refresh();
              });
        }

        public Action<ACServer> ServerSelected;
        public Action Cancel;

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ServerSelected((sender as FrameworkElement).DataContext as ACServer);
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstServers.SelectedValue != null)
            {
                ServerSelected(lstServers.SelectedValue as ACServer);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }
    }
}