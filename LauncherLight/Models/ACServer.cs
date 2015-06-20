using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace LauncherLight.Models
{
    public static class AvailableRessources
    {
        public static Dictionary<string, LLCarDesc> Cars { get; set; }

        public static Dictionary<string, LLTrackDesc> Tracks { get; set; }
    }

    [ImplementPropertyChanged]
    public class ServerCar
    {
        public ServerCar(LLCarDesc car)
        {
            this.CarDesc = car;
        }

        public LLCarDesc CarDesc { get; set; }

        public int TotalEntries { get; set; }

        public int ConnectedEntries { get; set; }

        public bool IsPickup { get; set; }

        public bool IsFull { get; set; }

        public static ServerCar FromCar(LLCarDesc car)
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
        private Timer t;

        public ACServer()
        {
            t = new Timer();
            t.Interval = 1000;
            t.Elapsed += t_Elapsed;
            t.AutoReset = true;
            //      LastRefresh = DateTime.MinValue;
            StartRefresh();
            t.Enabled = true;
        }

        ~ACServer()
        {
            t.Enabled = false;
        }

        private void StartRefresh()
        {
            t.Enabled = true;
        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            ComputeTime();
        }

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
                LLCarDesc currentCar = null;

                if (AvailableRessources.Cars.TryGetValue(car, out currentCar))
                {
                    ServerCars.Add(ServerCar.FromCar(currentCar));
                }
                else
                {
                    ServerCars.Add(ServerCar.FromCar(new LLCarDesc() { Model = car, IsMissing = true }));
                    MissingContent = true;
                }
            }
            this.oldcars = this.cars;
            return MissingContent;
        }

        private bool buildTrack(bool force)
        {
            if (force || TrackDesc == null || TrackDesc.Track != track)
            {
                LLTrackDesc currentTrack = null;

                if (AvailableRessources.Tracks.TryGetValue(track, out currentTrack))
                {
                    if (currentTrack != TrackDesc)
                        TrackDesc = currentTrack;
                }
                else
                {
                    TrackDesc = new LLTrackDesc { Track = track, TrackInfo = new TrackInfo { name = track }, IsMissing = true };
                }
            }
            return TrackDesc.IsMissing;
        }

        public bool MissingContent { get; set; }

        public ObservableCollection<ServerCar> ServerCars { get; set; }

        //private TrackDesc _track = null;

        public LLTrackDesc TrackDesc { get; set; }

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

            this.Booking = sess.Contains("0");
            this.Practice = sess.Contains("1");
            this.Qualify = sess.Contains("2");
            this.Race = sess.Contains("3");

            this.IsBooking = currentsession.ToString() == "0";
            this.IsPractice = currentsession.ToString() == "1";
            this.IsQualify = currentsession.ToString() == "2";
            this.IsRace = currentsession.ToString() == "3";

            try
            {
                this.BookingTime = FromMinutes(times.FirstOrDefault(i => i.Key == "0").Value);
            }
            catch { this.BookingTime = TimeSpan.MaxValue; }

            try
            {
                this.PracticeTime = FromMinutes(times.FirstOrDefault(i => i.Key == "1").Value);
            }
            catch { this.PracticeTime = TimeSpan.MaxValue; }

            try
            {
                this.QualifyTime = FromMinutes(times.FirstOrDefault(i => i.Key == "2").Value);
            }
            catch { this.QualifyTime = TimeSpan.MaxValue; }
            try
            {
                this.RaceLaps = times.FirstOrDefault(i => i.Key == "3").Value;
            }
            catch { }

            ComputeTime();

            this.IsFull = clients == maxclients;
            this.StartRefresh();
        }

        private void ComputeTime()
        {
            lock (this)
            {
                try
                {
                    if (IsBooking)
                    {
                        this.LiveBookingTime = TimeSpan.FromSeconds(Math.Max(timeleft - (DateTime.Now - LastRefresh).TotalSeconds, 0));
                    }
                    else
                    {
                        this.LiveBookingTime = BookingTime;
                    }
                }
                catch { }

                try
                {
                    if (IsPractice)
                    {
                        this.LivePracticeTime = TimeSpan.FromSeconds((int)Math.Max(timeleft - (DateTime.Now - LastRefresh).TotalSeconds, 0));
                    }
                    else
                    {
                        this.LivePracticeTime = PracticeTime;
                    }
                }
                catch { }
                try
                {
                    if (IsQualify)
                    {
                        this.LiveQualifyTime = TimeSpan.FromSeconds((int)Math.Max(timeleft - (DateTime.Now - LastRefresh).TotalSeconds, 0));
                    }
                    else
                    {
                        this.LiveQualifyTime = QualifyTime;
                    }
                }
                catch { }
            }
        }

        private TimeSpan FromMinutes(long minutes)
        {
            if (minutes > TimeSpan.MaxValue.TotalMinutes)
            {
                return TimeSpan.MaxValue;
            }
            else
            {
                return TimeSpan.FromMinutes(minutes);
            }
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

        public TimeSpan BookingTime { get; set; }

        public TimeSpan LiveQualifyTime { get; set; }

        public TimeSpan LiveBookingTime { get; set; }

        public TimeSpan LivePracticeTime { get; set; }

        public bool IsFull { get; set; }

        public DateTime LastRefresh { get; set; }
    }

    public class ACServerSample : ACServer
    {
        public ACServerSample()
        {
            this.Booking = true;
            this.clients = 20;
            this.maxclients = 20;
            this.name = "Server Name";
            this.pass = true;
            this.pickup = true;

            Booking = true;

            Practice = true;

            Qualify = true;

            Race = true;
        }
    }
}