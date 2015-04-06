using ACSharedMemory;
using ACSharedMemory.Models.Track;
using ACSharedMemory.Reader;
using ACToolsUtilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TimingClient
{
    public partial class MainForm : Form
    {
        private ACReader ac = new ACReader();
        private List<KeyValuePair<TimeSpan, float>> BestLapData;
        private TimeSpan BestLapTime;
        private List<KeyValuePair<TimeSpan, float>> CarPosition;
        private int currentLap = 0;
        private int currentPlayer = 0;
        private TrackDesc currentTrackDesc;
        private DashDisplay dash;
        private bool firstlogrun = true;
        private string lastBestTime = "";
        private AC_STATUS lastStatus = 0;
        private int lastTime = 0;
        private System.Timers.Timer logtimer = new System.Timers.Timer();
        private List<float[]> MapPosition = new List<float[]>();
        private List<KeyValuePair<TimeSpan, float>> MyBestLapData;
        private TimeSpan MyBestLapTime;
        private List<KeyValuePair<TimeSpan, float>> SessionBestLapData;
        private TimeSpan SessionBestLapTime;
        private int sessionId = 0;
        private System.Timers.Timer timer = new System.Timers.Timer();
        private ACDashboard.Dashboard wpfDash;

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

            timer.Elapsed += timer_Elapsed;
            timer.Interval = 8;
            numericUpDown1.Value = (decimal)this.timer.Interval;

            Task.Factory.StartNew(() =>
            {
                dash = new DashDisplay();
                timer.Enabled = true;

                ac.Start();

                logtimer.Elapsed += Logtimer_Elapsed;
                logtimer.Interval = 1000;
                logtimer.Enabled = true;
            });
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

        private TimeSpan GetLapDelta(ACSharedMemory.Graphics g, List<KeyValuePair<TimeSpan, float>> lapData)
        {
            TimeSpan timedeltaResult = new TimeSpan();
            if (lapData != null && lapData.Count > 2)
            {
                var idx = 0;
                var currentTime = ACHelper.ToTimeSpan(g.iCurrentTime);
                for (int i = 0; i <= BestLapData.Count; i++)
                {
                    if (lapData[i].Value <= g.NormalizedCarPosition)
                    {
                        idx = i;
                        break;
                    }
                }
                var bestLapTime = lapData[idx];

                // Interpolate
                if (idx > 0 && idx < lapData.Count - 1)
                {
                    var nextBestLapTime = lapData[idx - 1];
                    var estimatedSpeed = ((float)(nextBestLapTime.Value - bestLapTime.Value)) / ((float)((nextBestLapTime.Key - bestLapTime.Key).Milliseconds));
                    int additionalTime = (int)((g.NormalizedCarPosition - nextBestLapTime.Value) / estimatedSpeed);
                    var timeDelta = (ACHelper.ToTimeSpan(g.iCurrentTime) - (bestLapTime.Key + ACHelper.ToTimeSpan(additionalTime)));
                    //  lblTiming.Text = timeDelta.ToString();
                    timedeltaResult = timeDelta;
                }
                // Direct
                else
                {
                    var timeDelta = (ACHelper.ToTimeSpan(g.iCurrentTime) - bestLapTime.Key);
                    // lblTiming.Text = timeDelta.ToString();
                    timedeltaResult = timeDelta;
                }
            }
            return timedeltaResult;
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

        private void Logtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            logtimer.Enabled = false;
            try
            {
                if (ac.IsRunning)
                {
                    var p = ac.ReadPhysics();
                    var g = ac.ReadGraphics();
                    var s = ac.ReadStaticInfo();

                    if (g.Status != AC_STATUS.AC_OFF)
                    {
                        if (firstlogrun)
                        {
                            System.IO.File.AppendAllText("debug_p.csv", CSVLogger.LogHeader(typeof(Physics)));
                            System.IO.File.AppendAllText("debug_g.csv", CSVLogger.LogHeader(typeof(Graphics)));
                            firstlogrun = false;
                        }
                        System.IO.File.AppendAllText("debug_p.csv", CSVLogger.LogObject(p));
                        System.IO.File.AppendAllText("debug_g.csv", CSVLogger.LogObject(g));
                    }
                }
            }
            catch { }
            logtimer.Enabled = true;
        }

        private void ResetCarPosition()
        {
            CarPosition = new List<KeyValuePair<TimeSpan, float>>();
            CarPosition.Add(new KeyValuePair<TimeSpan, float>(TimeSpan.FromMilliseconds(0), 0));
        }

        private void ResetSession()
        {
            DumpTrackpositions();
            lastStatus = AC_STATUS.AC_OFF;
            currentLap = 1;
            sessionId = -1;
            lastTime = 0;
            //dash.Reset();
            ResetCarPosition();
        }

        private void SetCurrentPlayer()
        {
            using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
            {
                var session = acdb.Session.Where(i => i.SessionId == sessionId).First();
                session.PlayerId = (this.listBox1.SelectedItem as Player).PlayerId;
                acdb.SaveChanges();
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                lock (String.Intern("parsingLock"))
                {
                    var data = new DataContainer();

                    if (!ac.IsRunning)
                    {
                        ResetSession();
                        dash.Refresh(data);
                        wpfDash.SetData(data);
                        return;
                    }
                    ac.ReadPhysics();

                    try
                    {
                        var p = ac.ReadPhysics();
                        var g = ac.ReadGraphics();
                        var s = ac.ReadStaticInfo();
                        string acPath = ACHelper.GetAsettoCorsaPath();

                        data.GameRunning = true;
                        data.Physics = p;
                        data.Graphics = g;
                        data.StaticInfo = s;

                        // Trouve t'on l'exe ?
                        if (acPath == null)
                        {
                            ResetSession();
                            wpfDash.SetData(data);
                            return;
                        }

                        // session terminée ?
                        if (g.Status == AC_STATUS.AC_OFF)
                        {
                            ResetSession();
                            wpfDash.SetData(data);
                            return;
                        }

                        this.Invoke((MethodInvoker)delegate
                         {
                             this.txtG.Text = Newtonsoft.Json.JsonConvert.SerializeObject(g, Newtonsoft.Json.Formatting.Indented);
                             this.txtP.Text = Newtonsoft.Json.JsonConvert.SerializeObject(p, Newtonsoft.Json.Formatting.Indented);
                             this.txtS.Text = Newtonsoft.Json.JsonConvert.SerializeObject(s, Newtonsoft.Json.Formatting.Indented);
                         });

                        // Start
                        bool isStart = (lastStatus == AC_STATUS.AC_OFF && (g.Status == AC_STATUS.AC_LIVE || g.Status == AC_STATUS.AC_PAUSE));
                        isStart = isStart || (currentLap - 1) > g.CompletedLaps;

                        if (isStart)
                        {
                            ResetSession();

                            // Session start !
                            using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
                            {
                                currentTrackDesc = ACHelper.GetCurrentTrack(s);

                                Track track = GetTrack(currentTrackDesc, acdb);
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
                                currentLap = g.CompletedLaps;
                                lastTime = g.iCurrentTime;

                                FindBestLap(acdb, currentTrackDesc, car.Code);
                            }
                        }
                        else if (sessionId == -1)
                        {
                            ResetSession();
                            wpfDash.SetData(data);
                            return;
                        }

                        this.Invoke((MethodInvoker)delegate
                        {
                            // Update player
                            if (this.currentPlayer != (this.listBox1.SelectedItem as Player).PlayerId)
                            {
                                SetCurrentPlayer();
                            }
                        });

                        // Track laps
                        if (g.iCurrentTime < lastTime)
                        {
                            DumpTrackpositions();

                            TimeSpan maxWaitTime = new TimeSpan(0, 0, 3);
                            DateTime waitStart = DateTime.Now;

                            while (g.iLastTime <= 0 && (DateTime.Now - waitStart) <= maxWaitTime)
                            {
                                Thread.Sleep(50);
                                p = ac.ReadPhysics();
                                g = ac.ReadGraphics();
                                s = ac.ReadStaticInfo();
                            }

                            using (AsettoCorsaTrackingEntities acdb = new AsettoCorsaTrackingEntities())
                            {
                                if (g.iLastTime > 0)
                                {
                                    CarPosition.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iLastTime), 1));

                                    var lap = new Lap
                                    {
                                        LapNumber = currentLap,
                                        LapTime = ACHelper.ToTimeSpan(g.iLastTime),
                                        SessionId = this.sessionId,
                                        Timings = Newtonsoft.Json.JsonConvert.SerializeObject(CarPosition)
                                    };

                                    // Increment laps
                                    currentLap++;

                                    acdb.Lap.Add(lap);
                                    acdb.SaveChanges();
                                }
                                ResetCarPosition();

                                FindBestLap(acdb, currentTrackDesc, s.CarModel);
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

                            this.MapPosition.Add(g.CarCoordinates);
                        }

                        data.AllTimeDelta = GetLapDelta(g, BestLapData);
                        data.MyTimeDelta = GetLapDelta(g, MyBestLapData);
                        data.SessionTimeDelta = GetLapDelta(g, SessionBestLapData);
                        data.AllTimeBest = BestLapTime;
                        data.MyTimeBest = MyBestLapTime;
                        data.SessionTimeBest = SessionBestLapTime;
                        data.TrackDesc = currentTrackDesc;
                        lastStatus = g.Status;
                        lastBestTime = g.BestTime;
                        lastTime = g.iCurrentTime;
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.currentPlayer = (this.listBox1.SelectedItem as Player).PlayerId;
                        });
                    }
                    catch
                    {
                        ResetSession();
                    }
                    this.Invoke((MethodInvoker)delegate
                      {
                          wpfDash.SetData(data);
                      });

                    dash.Refresh(data);

                    // SendData(data);
                }
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        private void DumpTrackpositions()
        {
            if (currentTrackDesc != null && MapPosition != null && MapPosition.Count > 0)
            {
                System.IO.File.WriteAllText("trackdata__" + currentTrackDesc.TrackCode + "__" + currentLap + ".json",
                                                Newtonsoft.Json.JsonConvert.SerializeObject(MapPosition, Newtonsoft.Json.Formatting.Indented)
                                            );
                MapPosition = new List<float[]>();
            }
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.timer.Interval = (int)numericUpDown1.Value;
        }
    }
}