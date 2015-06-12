using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LauncherLight.Models
{
    public static class AvailableRessources
    {
        public static Dictionary<string, CarDesc> Cars { get; set; }

        public static Dictionary<string, TrackDesc> Tracks { get; set; }
    }

    [ImplementPropertyChanged]
    public class ServerCar
    {
        public ServerCar(CarDesc car)
        {
            this.CarDesc = car;
        }

        public CarDesc CarDesc { get; set; }

        public int TotalEntries { get; set; }

        public int ConnectedEntries { get; set; }

        public bool IsPickup { get; set; }

        public bool IsFull { get; set; }

        public static ServerCar FromCar(CarDesc car)
        {
            return new ServerCar(car);
        }
    }

    [ImplementPropertyChanged]
    public class ACServerJson
    {
        public string ip { get; set; }

        public int port { get; set; }

        public int cport { get; set; }

        public int tport { get; set; }

        public string name { get; set; }

        public int clients { get; set; }

        public int maxclients { get; set; }

        public string track { get; set; }

        public List<string> cars { get; set; }

        public int timeofday { get; set; }

        public int session { get; set; }

        public List<string> sessiontypes { get; set; }

        public List<string> durations { get; set; }

        public long timeleft { get; set; }

        public List<string> country { get; set; }

        public bool pass { get; set; }

        public bool pickup { get; set; }

        public int timestamp { get; set; }

        public int lastupdate { get; set; }

        public bool l { get; set; }
    }

    [ImplementPropertyChanged]
    public class EntryList
    {
        public List<EntryItem> Cars { get; set; }
    }

    [ImplementPropertyChanged]
    public class EntryItem
    {
        public string Model { get; set; }

        public string Skin { get; set; }

        public string DriverName { get; set; }

        public string DriverTeam { get; set; }

        public bool IsConnected { get; set; }

        public bool IsRequestedGUID { get; set; }

        public bool IsEntryList { get; set; }
    }

    [ImplementPropertyChanged]
    public class ACServer : ACServerJson
    {
        public bool Unreachable { get; set; }

        public bool Lan { get; set; }

        private List<string> oldcars = new List<string>();

        private bool buildCars(bool force)
        {
            bool MissingContent = false;
            if (!force && ServerCars != null && string.Join(";", this.oldcars) == string.Join(";", this.cars))
            {
                return ServerCars.Any(i => i.CarDesc.IsMissing);
            }
            ServerCars = new ObservableCollection<ServerCar>();
            foreach (var car in cars)
            {
                CarDesc currentCar = null;
                ;

                if (AvailableRessources.Cars.TryGetValue(car, out currentCar))
                {
                    ServerCars.Add(ServerCar.FromCar(currentCar));
                }
                else
                {
                    ServerCars.Add(ServerCar.FromCar(new CarDesc() { Model = car, IsMissing = true }));
                    MissingContent = true;
                }
            }
            this.oldcars = this.cars;
            return MissingContent;
        }

        private bool buildTrack(bool force)
        {
            bool MissingContent = false;
            if (force || TrackDesc == null || TrackDesc.Track != track)
            {
                TrackDesc currentTrack = null;

                if (AvailableRessources.Tracks.TryGetValue(track, out currentTrack))
                {
                    if (currentTrack != TrackDesc)
                        TrackDesc = currentTrack;
                }
                else
                {
                    TrackDesc = new TrackDesc { Track = track, TrackInfo = new TrackInfo { name = track }, IsMissing = true };
                    MissingContent = true;
                }
            }
            return TrackDesc.IsMissing;
        }

        public bool MissingContent { get; set; }

        public ObservableCollection<ServerCar> ServerCars { get; set; }

        //private TrackDesc _track = null;

        public TrackDesc TrackDesc { get; set; }

        //public List<CarDesc> CarDescs
        //{
        //    get
        //    {
        //        return ServerCars.Select(i => i.CarDesc).ToList();
        //    }
        //}

        public void fill(bool force)
        {
            var mc = this.buildCars(force);
            var mt = this.buildTrack(force);

            this.MissingContent = mc || mt;

            var sess = this.sessiontypes.ToList();

            string currentsession = session.ToString();

            if (!sess.Contains(currentsession.ToString()))
            {
                currentsession = sess.FirstOrDefault();
            }

            List<KeyValuePair<string, long>> times = new List<KeyValuePair<string, long>>();
            for (int i = 0; i < sess.Count; i++)
            {
                times.Add(new KeyValuePair<string, long>(sess[i], long.Parse(durations[i])));
            }

            try
            {
                this.PracticeTime = TimeSpan.FromMinutes(times.FirstOrDefault(i => i.Key == "1").Value);
            }
            catch { this.PracticeTime = TimeSpan.MaxValue; }
            try
            {
                this.QualifyTime = TimeSpan.FromMinutes(times.FirstOrDefault(i => i.Key == "2").Value);
            }
            catch { this.QualifyTime = TimeSpan.MaxValue; }
            try
            {
                this.RaceLaps = times.FirstOrDefault(i => i.Key == "3").Value;
            }
            catch { }
            this.Booking = sess.Contains("0");
            this.Practice = sess.Contains("1");
            this.Qualify = sess.Contains("2");
            this.Race = sess.Contains("3");

            this.IsBooking = currentsession.ToString() == "0";
            this.IsPractice = currentsession.ToString() == "1";
            this.IsQualify = currentsession.ToString() == "2";
            this.IsRace = currentsession.ToString() == "3";

            this.IsFull = clients == maxclients;
        }

        public string AdressInfo { get { return string.Format("http://{0}:{1}/INFO", ip, cport); } }

        public string CurrentPassword { get; set; }

        public ServerCar SelectedCar { get; set; }

        public bool Booking { get; set; }

        public bool Practice { get; set; }

        public bool Qualify { get; set; }

        public bool Race { get; set; }

        public bool IsBooking { get; set; }

        public bool IsPractice { get; set; }

        public bool IsQualify { get; set; }

        public bool IsRace { get; set; }

        public long RaceLaps { get; set; }

        public TimeSpan QualifyTime { get; set; }

        public TimeSpan PracticeTime { get; set; }

        public bool IsFull { get; set; }
    }

    public class ACServerSample : ACServer
    {
        public ACServerSample()
        {
            this.Booking = true;
            this.clients = 20;
            this.maxclients = 20;
            this.name = "Server Name";
        }
    }
}