using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace LauncherLight.Models
{
    public class ServerListModel : INotifyPropertyChanged
    {
        private void RaisePropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private CollectionViewSource _view;

        [JsonIgnore]
        public CollectionViewSource View
        {
            get { return _view; }
            set { _view = value; RaisePropertyChanged("View"); }
        }

        private void RefreshView()
        {
            if (_view != null) _view.View.Refresh();
        }

        private bool _NoPass;

        public bool NoPass
        {
            get { return _NoPass; }
            set { _NoPass = value; RaisePropertyChanged("NoPass"); RefreshView(); }
        }

        private bool _NoEmpty;

        public bool NoEmpty
        {
            get { return _NoEmpty; }
            set { _NoEmpty = value; RaisePropertyChanged("NoEmpty"); RefreshView(); }
        }

        private bool _NoFull;

        public bool NoFull
        {
            get { return _NoFull; }
            set { _NoFull = value; RaisePropertyChanged("NoFull"); RefreshView(); }
        }

        private bool _NoBooking = true;

        public bool NoBooking
        {
            get { return _NoBooking; }
            set { _NoBooking = value; RaisePropertyChanged("NoBooking"); RefreshView(); }
        }

        private bool _OnlyLan;

        public bool OnlyLan
        {
            get { return _OnlyLan; }
            set { _OnlyLan = value; RaisePropertyChanged("OnlyLan"); RefreshView(); }
        }

        private string _ServerFilter = null;

        public string ServerFilter
        {
            get { return _ServerFilter; }
            set { _ServerFilter = value; RaisePropertyChanged("ServerFilter"); RefreshView(); }
        }

        private bool _MissingContent = false;

        public bool MissingContent
        {
            get { return _MissingContent; }
            set { _MissingContent = value; RaisePropertyChanged("MissingContent"); RefreshView(); }
        }

        public void view_Filter(object sender, FilterEventArgs e)
        {
            var server = e.Item as ACServer;

            if (!this.MissingContent && server.MissingContent)
            {
                e.Accepted = false; return;
            }
            if (NoFull)
            {
                if (server.maxclients == server.clients) { e.Accepted = false; return; }
            }
            if (NoEmpty)
            {
                if (server.clients == 0) { e.Accepted = false; return; }
            }
            if (NoPass)
            {
                if (server.pass) { e.Accepted = false; return; }
            }
            if (NoBooking)
            {
                if (server.Booking) { e.Accepted = false; return; }
            }
            if (OnlyLan)
            {
                if (!server.Lan) { e.Accepted = false; return; }
            }
            if (!(string.IsNullOrWhiteSpace(ServerFilter)))
            {
                if (!(server.name ?? "").ToLower().Contains(ServerFilter.Trim().ToLower())) { e.Accepted = false; return; }
            }
        }
    }
}