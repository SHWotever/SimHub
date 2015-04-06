using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using ACSharedMemory.Reader;
using ACToolsUtilities;
using System;
using System.Collections.Generic;
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

        public string CurrentGear { get; internal set; }
    }

    public class GameData
    {
        public bool GameRunning { get; internal set; }

        public string GamePath { get; internal set; }

        public ACData OldData { get; internal set; }

        public ACData NewData { get; internal set; }

        public TrackDesc Track { get; internal set; }

        public CarDesc Car { get; internal set; }

        public Events Events { get; internal set; }

        public string Gear { get; internal set; }
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
                timer.Enabled = false;

                ResetEventData();
                data.OldData = data.NewData;

                if (CheckGameRunning())
                {
                    data.NewData = ac.GetData();

                    CheckGameStatusChanged();

                    if (data.NewData.Graphics.Status == AC_STATUS.AC_LIVE)
                    {
                        CheckTrackChanged();
                        CheckCarChanged();
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

                timer.Enabled = enabled;
            }
        }

        private void PreParseData()
        {
            if (data.GameRunning)
            {
                data.Gear = ACHelper.GetGear(this.data.NewData.Physics.Gear);
            }
        }

        private void SendDataUpdatedEvent()
        {
            if (DataUpdated != null)
            {
                DataUpdated(data, this);
            }
        }

        private void CheckLapChanged()
        {
            if (data.OldData == null ||
                data.OldData.Graphics.iCurrentTime > data.NewData.Graphics.iCurrentTime)
            {
                if (NewLap != null)
                {
                    data.Events.IsNewLap = true;
                    data.Events.IsPreviousLapTest = data.NewData.Graphics.iBestTime == 0;
                    NewLap(data.NewData.Graphics.CompletedLaps, data.Events.IsPreviousLapTest, this);
                }
            }
        }

        private void CheckCarChanged()
        {
            if (data.Car != null && !data.GameRunning)
            {
                data.Car = null;
                if (CarChanged != null)
                {
                    data.Events.CarChanged = true;
                    CarChanged(null, this);
                    return;
                }
            }
            else if (data.GameRunning)
            {
                if (data.Car == null || data.Car.Model != data.NewData.StaticInfo.CarModel)
                {
                    data.Car = ACHelper.GetCarData(data.NewData.StaticInfo);
                    if (CarChanged != null)
                    {
                        data.Events.CarChanged = true;
                        CarChanged(data.Car, this);
                        return;
                    }
                }
            }
        }

        private void CheckTrackChanged()
        {
            if (data.Track != null && !data.GameRunning)
            {
                data.Track = null;
                if (TrackChanged != null)
                {
                    data.Events.TrackChanged = true;
                    TrackChanged(null, this);
                    return;
                }
            }
            else if (data.GameRunning)
            {
                if (data.Track == null)
                {
                    data.Track = ACHelper.GetCurrentTrack(data.NewData.StaticInfo);
                    if (TrackChanged != null)
                    {
                        data.Events.TrackChanged = true;
                        TrackChanged(data.Track, this);

                    }
                }
            }
        }

        private void CheckSessionRestart()
        {
            if (data.OldData != null &&
                data.OldData.Graphics.CompletedLaps > data.NewData.Graphics.CompletedLaps && data.NewData.Graphics.iBestTime == 0)
            {
                if (SessionRestart != null)
                {
                    data.Events.SessionRestarted = true;
                    SessionRestart(this);
                }
            }
        }

        private void CheckGameStatusChanged()
        {
            if (data.OldData == null
                || data.OldData.Graphics.Status != data.NewData.Graphics.Status)
            {
                if (GameStatusChanged != null)
                {
                    GameStatusChanged(data.NewData.Graphics.Status, this);
                }
            }
        }

        private void CheckGameSessionTypeChanged()
        {
            if (data.OldData == null || data.OldData.Graphics.Session != data.NewData.Graphics.Session)
            {
                if (SessionTypeChanged != null)
                {
                    SessionTypeChanged(data.NewData.Graphics.Session, this);
                }
            }
        }

        private bool CheckGameRunning()
        {
            if (!ACHelper.IsProcessRunning())
            {
                SetGameState(false);
                return false;
            }

            if (!ac.IsRunning)
            {
                SetGameState(false);
                return false;
            }

            data.GamePath = ACHelper.GetAsettoCorsaPath();
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
                    GameStateChanged(running, this);
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