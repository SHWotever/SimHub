using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LauncherLight.Models;

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

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastServerListSettings))
            {
                try
                {
                    datac = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerListModel>(Properties.Settings.Default.LastServerListSettings);
                }
                catch { }
            }
            datac = datac ?? new ServerListModel();
            this.DataContext = datac;
        }

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
                  try
                  {
                      foreach (var server in servers)
                          (datac.View.Source as ObservableCollection<ACServer>).Add(server);
                      if (datac.OnlyLan)
                      {
                          datac.View.View.Refresh();
                      }
                  }
                  catch { }
              });
        }

        public Action<ACServer> ServerSelected;
        public Action Cancel;

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ServerSelected((sender as FrameworkElement).DataContext as ACServer);
                SaveDefaults();
            }
        }

        private void SaveDefaults()
        {

            Properties.Settings.Default.LastServerListSettings = Newtonsoft.Json.JsonConvert.SerializeObject(datac);
            Properties.Settings.Default.Save();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstServers.SelectedValue != null)
            {
                ServerSelected(lstServers.SelectedValue as ACServer);
                SaveDefaults();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
            SaveDefaults();
        }
    }
}