using ACSharedMemory;
using ACSharedMemory.Models.Track;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TimingClient.V2
{
    public partial class MainForm : Form
    {
        private List<KeyValuePair<TimeSpan, float>> BestLapData;
        private TimeSpan BestLapTime;
        private List<KeyValuePair<TimeSpan, float>> CarPosition;

        private int currentPlayer = 0;

        private DashDisplay dash;

        private List<KeyValuePair<TimeSpan, float>> MyBestLapData;
        private TimeSpan MyBestLapTime;
        private List<KeyValuePair<TimeSpan, float>> SessionBestLapData;
        private TimeSpan SessionBestLapTime;
        private int sessionId = 0;
        private ACDashboard.Dashboard wpfDash;
      

        private ACManager manager = new ACManager();

        public MainForm()
        {
            InitializeComponent();

            this.listBox1.SelectedIndex = 0;

            this.Show();

            wpfDash = new ACDashboard.Dashboard();
            ElementHost.EnableModelessKeyboardInterop(wpfDash);
            wpfDash.Show();

            using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
            {
                try
                {
                    this.listBox1.Items.Clear();
                    this.listBox1.Items.AddRange(acdb.Player.OrderBy(i => i.PlayerName).ToArray());
                    this.listBox1.DisplayMember = "PlayerName";
                    this.listBox1.ValueMember = "PlayerId";
                    this.listBox1.SelectedIndex = 0;
                    this.listBox1.SelectedIndex = this.listBox1.Items.IndexOf(this.listBox1.Items.Cast<Player>().First(i => i.PlayerName == TimingClient.Properties.Settings.Default.DefaultPlayer));
                }
                catch { }
            }

            Task.Factory.StartNew(() =>
            {
                dash = new DashDisplay();
                manager.UpdateInterval = 10;
                manager.DataUpdated += manager_DataUpdated;
                manager.GameStateChanged += manager_GameStateChanged;
                manager.Enabled = true;
                manager.Start();
            });
        }

        void manager_GameStateChanged(bool running, ACManager manager)
        {
            Program.MacroEngine.Enabled = running;
            Debug.WriteLine("MacroEngine enabled : " + running.ToString());
        }

        private void manager_DataUpdated(GameData data, ACManager manager)
        {
            var outputdata = new DataContainer();
            if (data.GameRunning)
            {
                outputdata.GameRunning = true;
                var p = data.NewData.Physics;
                var g = data.NewData.Graphics;
                var s = data.NewData.StaticInfo;

                if (g.Status == AC_STATUS.AC_OFF)
                {
                    ResetSession();
                    return;
                }

                if (data.Events.GameStarted || data.Events.SessionRestarted)
                {
                    ResetSession();

                    // Session start !
                    using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
                    {
                        Track track = GetTrack(data.Track, acdb);
                        Car car = GetCar(s, acdb);

                        // Open session
                        Session sess = new Session
                        {
                            StartDate = DateTime.Now,
                            Track = track.Code,
                            Car = car.Code,
                        };

                        this.Invoke((MethodInvoker)delegate
                        {
                            sess.PlayerId = (this.listBox1.SelectedItem as Player).PlayerId;
                        });
                        acdb.Session.Add(sess);
                        acdb.SaveChanges();

                        this.sessionId = sess.SessionId;

                        FindBestLap(acdb, data.Track, car.Code);
                    }
                }

                SetCurrentPlayer();

                if (data.Events.IsNewLap)
                {
                    using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
                    {
                        if (!data.Events.IsPreviousLapTest)
                        {
                            CarPosition.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iLastTime), 1));
                            var lap = new Lap
                            {
                                LapNumber = data.NewData.Graphics.CompletedLaps,
                                LapTime = ACHelper.ToTimeSpan(g.iLastTime),
                                SessionId = this.sessionId,
                                Timings = Newtonsoft.Json.JsonConvert.SerializeObject(CarPosition)
                            };

                            acdb.Lap.Add(lap);
                            acdb.SaveChanges();
                        }
                        ResetCarPosition();
                        FindBestLap(acdb, data.Track, s.CarModel);
                    }
                }

                if (g.Status == AC_STATUS.AC_LIVE)
                {
                    // Track timings all 100ms
                    if (CarPosition.Count == 0 || CarPosition.Last().Key.TotalMilliseconds + 50 < ACHelper.ToTimeSpan(g.iCurrentTime).TotalMilliseconds)
                    {
                        if (CarPosition.Count == 0 || CarPosition.Last().Value < g.NormalizedCarPosition)
                        {
                            CarPosition.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iCurrentTime), g.NormalizedCarPosition));
                        }
                    }
                }

                outputdata.AllTimeDelta = ACHelper.GetLapDeltaReverse(g, BestLapData);
                outputdata.MyTimeDelta = ACHelper.GetLapDeltaReverse(g, MyBestLapData);
                outputdata.SessionTimeDelta = ACHelper.GetLapDeltaReverse(g, SessionBestLapData);
                outputdata.AllTimeBest = BestLapTime;
                outputdata.MyTimeBest = MyBestLapTime;
                outputdata.SessionTimeBest = SessionBestLapTime;
                outputdata.TrackDesc = data.Track;
            }

            dash.Refresh(outputdata);
            wpfDash.SetData(outputdata);

        }

        private void FindBestLap(AsettoCorsaTrackingEntities acdb, TrackDesc track, string carCode)
        {
            Lap bestLap = null;

            this.SessionBestLapData = null;
            this.BestLapData = null;
            this.MyBestLapData = null;
            this.SessionBestLapTime = TimeSpan.Zero;
            this.MyBestLapTime = TimeSpan.Zero;
            this.BestLapTime = TimeSpan.Zero;

            bestLap = acdb.Lap.Where(i => i.Session.Track == track.TrackCode && i.Session.Car == carCode && i.Timings != null)
                .OrderBy(i => i.LapTime).FirstOrDefault();
            if (bestLap != null)
            {
                this.BestLapData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<TimeSpan, float>>>(bestLap.Timings);
                this.BestLapTime = bestLap.LapTime;
                BestLapData.Reverse();
            }

            bestLap = acdb.Lap.Where(i => i.Session.Track == track.TrackCode && i.Session.Car == carCode && i.Timings != null && i.Session.Player.PlayerId == this.currentPlayer)
             .OrderBy(i => i.LapTime).FirstOrDefault();
            if (bestLap != null)
            {
                this.MyBestLapData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<TimeSpan, float>>>(bestLap.Timings);
                this.MyBestLapTime = bestLap.LapTime;
                this.MyBestLapData.Reverse();
            }

            bestLap = acdb.Lap.Where(i => i.Session.Track == track.TrackCode && i.Session.Car == carCode && i.Timings != null && i.Session.SessionId == this.sessionId)
             .OrderBy(i => i.LapTime).FirstOrDefault();
            if (bestLap != null)
            {
                this.SessionBestLapData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<TimeSpan, float>>>(bestLap.Timings);
                this.SessionBestLapTime = bestLap.LapTime;
                this.SessionBestLapData.Reverse();
            }
        }

        private Car GetCar(StaticInfo s, AsettoCorsaTrackingEntities acdb)
        {
            Car car = acdb.Car.FirstOrDefault(i => i.Code == s.CarModel);

            if (car == null)
            {
                car = new Car
                {
                    Code = s.CarModel
                };

                acdb.Car.Add(car);
            }

            // Update car
            var cd = ACHelper.GetCarData(s);
            var ci = cd.CarInfo;

            car.acceleration = ci.specs.acceleration;

            car.brand = ci.brand;
            car.Class = ci.@class;
            car.description = ci.description;
            car.name = ci.name;
            car.parent = ci.parent;

            car.PreviewPng = cd.GetCarPreview();
            car.BadgePng = cd.GetCarBadge();

            if (ci.specs != null)
            {
                car.bhp = ci.specs.bhp;
                car.pwratio = ci.specs.pwratio;
                car.topspeed = ci.specs.topspeed;
                car.torque = ci.specs.torque;
                car.weight = ci.specs.weight;
            }

            return car;
        }

        private Track GetTrack(TrackDesc trackDesc, AsettoCorsaTrackingEntities acdb)
        {
            Track track = acdb.Track.FirstOrDefault(i => i.Code == trackDesc.TrackCode);
            if (track == null)
            {
                track = new Track
                {
                    Code = trackDesc.TrackCode
                };

                acdb.Track.Add(track);
            }

            // Update track
            TrackInfo ti = trackDesc.TrackInfo;
            track.City = ti.city;
            track.Country = ti.country;
            track.Description = ti.description;
            track.Length = ti.length;
            track.Name = ti.name;
            track.Width = ti.width;
            track.OutlinePng = trackDesc.getTrackOutline();
            track.PreviewPng = trackDesc.getTrackPreview();
            return track;
        }

        private void ResetCarPosition()
        {
            CarPosition = new List<KeyValuePair<TimeSpan, float>>();
            CarPosition.Add(new KeyValuePair<TimeSpan, float>(TimeSpan.FromMilliseconds(0), 0));
        }

        private void ResetSession()
        {
            sessionId = -1;
            ResetCarPosition();
        }

        private void SetCurrentPlayer()
        {
            this.Invoke((MethodInvoker)delegate
            {
                // Update player
                if (sessionId != -1 && this.currentPlayer != (this.listBox1.SelectedItem as Player).PlayerId)
                {
                    using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
                    {
                        this.currentPlayer = (this.listBox1.SelectedItem as Player).PlayerId;
                        var session = acdb.Session.Where(i => i.SessionId == sessionId).First();
                        session.PlayerId = this.currentPlayer;
                        acdb.SaveChanges();
                    }
                }
            });
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.manager.UpdateInterval = (int)numericUpDown1.Value;
        }
    }
}