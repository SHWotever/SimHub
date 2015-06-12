using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using ACSharedMemory.Reader;
using System;
using System.ComponentModel;
using System.Timers;

namespace ACSharedMemory
{
    public class Events
    {
        public bool IsNewLap { get; internal set; }

        public bool IsPreviousLapTest { get; internal set; }

        public bool SessionRestarted { get; internal set; }

        public bool GameStopped { get; internal set; }

        public bool GameStarted { get; internal set; }

        public bool CarChanged { get; internal set; }

        public bool TrackChanged { get; internal set; }

        public bool PitChanged { get; internal set; }

        //public string CurrentGearChanged { get; internal set; }
    }

    [Serializable]
    public class GameData
    {
        public bool GameRunning { get; internal set; }

        public string GamePath { get; internal set; }

        public ACData OldData { get; internal set; }

        public ACData NewData { get; internal set; }

        public TrackDesc Track { get; internal set; }

        public CarDesc Car { get; internal set; }

        public Events Events { get; internal set; }
    }

    public class ACManager
    {
        // Events
        public delegate void GameRunningChangedDelegate(bool running, ACManager manager);

        public event GameRunningChangedDelegate GameStateChanged;

        public delegate void GameStatusChangedDelegate(AC_STATUS status, ACManager manager);

        public event GameStatusChangedDelegate GameStatusChanged;

        public delegate void NewLapDelegate(int completedLapNumber, bool testLap, ACManager manager);

        public event NewLapDelegate NewLap;

        public delegate void SessionTypeChangedDelegate(AC_SESSION_TYPE sessionType, ACManager manager);

        public event SessionTypeChangedDelegate SessionTypeChanged;

        public delegate void SessionRestartDelegate(ACManager manager);

        public event SessionRestartDelegate SessionRestart;

        public delegate void CarChangedDelegate(CarDesc newCar, ACManager manager);

        public event CarChangedDelegate CarChanged;

        public delegate void TrackChangedDelegate(TrackDesc newTrack, ACManager manager);

        public event TrackChangedDelegate TrackChanged;

        public delegate void DataUpdatedDelegate(GameData data, ACManager manager);

        public event DataUpdatedDelegate DataUpdated;

        private Timer timer = new Timer();
        private ACReader ac;
        private GameData data = new GameData();

        public GameData Status
        {
            get { return data; }
        }

        //public override object InitializeLifetimeService()
        //{
        //    return null;
        //}

        private Guid guid = Guid.NewGuid();

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public ACManager()
        {
            timer = new Timer();
            timer.Interval = 20;
            timer.Elapsed += timer_Elapsed;
            ac = new ACReader();
            ac.Start();
        }

        public ISynchronizeInvoke SynchronizingObject
        {
            get { return this.timer.SynchronizingObject; }
            set { this.timer.SynchronizingObject = value; }
        }

        private void ResetEventData()
        {
            data.Events = new Events();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (timer)
            {
                try
                {
                    timer.Enabled = false;

                    ResetEventData();
                    data.OldData = data.NewData;

                    if (CheckGameRunning())
                    {
                        data.NewData = ac.GetData();

                        CheckGameStatusChanged();
                        CheckPitChanged();
                        CheckTrackChanged();
                        CheckCarChanged();

                        if (data.NewData.Graphics.Status == AC_STATUS.AC_LIVE)
                        {
                            PreParseData();
                            CheckSessionRestart();
                            CheckGameSessionTypeChanged();
                            CheckLapChanged();
                            SendDataUpdatedEvent();
                        }
                    }
                    else
                    {
                        bool sendFinalData = data.OldData != null;
                        data.NewData = null;

                        CheckTrackChanged();
                        CheckCarChanged();

                        //if (sendFinalData)
                        SendDataUpdatedEvent();
                    }
                }
                catch (Exception ex)
                {
                    Logging.Current.Warn(ex);
                }
                finally
                {
                    timer.Enabled = enabled;
                }
            }
        }

        private void PreParseData()
        {
            if (data.GameRunning)
            {
                data.NewData.Gear = ACHelper.GetGear(this.data.NewData.Physics.Gear);
                data.NewData.CurrentLapTime = TimeSpan.FromMilliseconds(this.data.NewData.Graphics.iCurrentTime);
                data.NewData.LastLapTime = TimeSpan.FromMilliseconds(this.data.NewData.Graphics.iLastTime);
                data.NewData.BestLapTime = TimeSpan.FromMilliseconds(this.data.NewData.Graphics.iBestTime);
                data.NewData.LastSectorTime = TimeSpan.FromMilliseconds(this.data.NewData.Graphics.LastSectorTime);
                data.NewData.SessionTimeLeft = TimeSpan.FromMilliseconds(this.data.NewData.Graphics.SessionTimeLeft);
                data.NewData.SpeedMph = this.data.NewData.Physics.SpeedKmh * 0.621371192f;
                data.NewData.SessionTypeName = this.data.NewData.Graphics.Session.ToString().Replace("AC_", "");
                data.NewData.StatusName = this.data.NewData.Graphics.Status.ToString().Replace("AC_", "");
            }
        }

        private void SendDataUpdatedEvent()
        {
            if (DataUpdated != null)
            {
                SecureDo(() => DataUpdated(data, this));
            }
        }

        private void CheckLapChanged()
        {
            if (data.OldData == null ||
                data.OldData.Graphics.iCurrentTime > data.NewData.Graphics.iCurrentTime)
            {
                data.Events.IsNewLap = true;
                data.Events.IsPreviousLapTest = data.NewData.Graphics.iBestTime == 0;
                if (NewLap != null)
                {
                    SecureDo(() => NewLap(data.NewData.Graphics.CompletedLaps, data.Events.IsPreviousLapTest, this));
                }
            }
        }

        private void CheckPitChanged()
        {
            if (data.OldData == null ||
                    data.OldData.Graphics.IsInPit != data.NewData.Graphics.IsInPit)
            {
                data.Events.PitChanged = true;
            }
        }

        private void CheckCarChanged()
        {
            if (data.Car != null && !data.GameRunning)
            {
                data.Car = null; data.Events.CarChanged = true;
                if (CarChanged != null)
                {
                    SecureDo(() => CarChanged(null, this));
                    return;
                }
            }
            else if (data.GameRunning)
            {
                if (data.Car == null || data.Car.Model != data.NewData.StaticInfo.CarModel)
                {
                    if (!string.IsNullOrWhiteSpace(data.NewData.StaticInfo.CarModel))
                    {
                        data.Car = ACHelper.GetCarData(data.NewData.StaticInfo); data.Events.CarChanged = true;
                        if (CarChanged != null)
                        {
                            SecureDo(() => CarChanged(data.Car, this));
                            return;
                        }
                    }
                }
            }
        }

        private void CheckTrackChanged()
        {
            if (data.Track != null && !data.GameRunning)
            {
                data.Track = null;
                data.Events.TrackChanged = true;
                if (TrackChanged != null)
                {
                    SecureDo(() => TrackChanged(null, this));
                    return;
                }
            }
            else if (data.GameRunning)
            {
                if (data.Track == null)
                {
                    if (!string.IsNullOrWhiteSpace(data.NewData.StaticInfo.Track))
                    {
                        data.Track = ACHelper.GetCurrentTrack(data.NewData.StaticInfo); data.Events.TrackChanged = true;
                        if (TrackChanged != null)
                        {
                            SecureDo(() => TrackChanged(data.Track, this));
                        }
                    }
                }
            }
        }

        private void CheckSessionRestart()
        {
            if (data.OldData != null &&
                data.OldData.Graphics.CompletedLaps > data.NewData.Graphics.CompletedLaps && data.NewData.Graphics.iBestTime == 0)
            {
                data.Events.SessionRestarted = true;
                if (SessionRestart != null)
                {
                    SecureDo(() => SessionRestart(this));
                }
            }
        }

        private void SecureDo(Action a)
        {
            try
            {
                a();
            }
            catch (Exception ex)
            {
                Logging.Current.Warn(ex);
            }
        }

        private void CheckGameStatusChanged()
        {
            if (data.OldData == null
                || data.OldData.Graphics.Status != data.NewData.Graphics.Status)
            {
                if (GameStatusChanged != null)
                {
                    SecureDo(() => GameStatusChanged(data.NewData.Graphics.Status, this));
                }
            }
        }

        private void CheckGameSessionTypeChanged()
        {
            if (data.OldData == null || data.OldData.Graphics.Session != data.NewData.Graphics.Session)
            {
                if (SessionTypeChanged != null)
                {
                    SecureDo(() => SessionTypeChanged(data.NewData.Graphics.Session, this));
                }
            }
        }

        private bool CheckGameRunning()
        {
            var path = ACHelper.GetAsettoCorsaPath();

            if (path == null)
            {
                SetGameState(false);
                return false;
            }

            if (!ac.IsRunning)
            {
                SetGameState(false);
                return false;
            }

            data.GamePath = path;
            SetGameState(true);
            return true;
        }

        private void SetGameState(bool running)
        {
            if (data.GameRunning != running)
            {
                if (running)
                {
                    data.Events.GameStarted = true;
                }
                else
                {
                    data.Events.GameStopped = true;
                }

                data.GameRunning = running;
                if (GameStateChanged != null)
                {
                    SecureDo(() => GameStateChanged(running, this));
                }
            }
        }

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                timer.Enabled = value;
            }
        }

        public void Start()
        {
            this.Enabled = true;
        }

        public void Stop()
        {
            this.Enabled = false;
        }

        public double UpdateInterval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }
    }
}