using ACSharedMemory;
using ACSharedMemory.Models.Track;
using ACSharedMemory.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class TrackRecord : Form
    {
        private ACReader ac = new ACReader();
        private int currentLap = 0;
        private TrackDesc currentTrackDesc;
        private AC_STATUS lastStatus = 0;
        private int lastTime = 0;
        private List<float[]> MapPosition = new List<float[]>();

        public TrackRecord()
        {
            InitializeComponent();
            this.Timer.Tick += Timer_Tick;
        }

        public string OutputFolder;

        protected override void OnShown(EventArgs e)
        {
            this.lblOutputFolder.Text = OutputFolder;
            this.lblPosition.Text = "";
            this.lblCurrentTrack.Text = "Waiting for race to start";
            this.btnStart_Click(null, null);
            base.OnShown(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Timer.Enabled = true;
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            ac.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Timer.Enabled = false;
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            ResetTelemetry();
            this.Close();
            ac.Stop();
        }

        private TelemetryWriter TelemetryWriter = null;

        public void ResetTelemetry()
        {
            lock (this)
            {
                if (TelemetryWriter != null)
                {
                    TelemetryWriter.close();
                    TelemetryWriter = null;
                }
            }
        }

        public void AddTelemetry(ACSharedMemory.Graphics g, Physics p, StaticInfo s)
        {
            if (TelemetryWriter == null)
            {
                TelemetryWriter = new TelemetryWriter(
                    Path.Combine(OutputFolder,
                    "trackdata__" + currentTrackDesc.TrackCode + "__" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".tracktelemetry")
                    );
            }
            TelemetryWriter.Write(new TelemetryContainer { Graphics = g, Physics = p }, s);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (String.Intern("parsingLock"))
                {
                    if (!ac.IsRunning)
                    {
                        ResetSession();
                        return;
                    }

                    try
                    {
                        var p = ac.ReadPhysics();
                        var g = ac.ReadGraphics();
                        var s = ac.ReadStaticInfo();
                        string acPath = ACHelper.GetAsettoCorsaPath();

                        // Trouve t'on l'exe ?
                        if (acPath == null)
                        {
                            ResetSession();
                            return;
                        }

                        // session terminée ?
                        if (g.Status == AC_STATUS.AC_OFF)
                        {
                            return;
                        }

                        // Start
                        bool isStart = (lastStatus == AC_STATUS.AC_OFF && (g.Status == AC_STATUS.AC_LIVE || g.Status == AC_STATUS.AC_PAUSE));
                        isStart = isStart || (currentLap - 1) > g.CompletedLaps;

                        if (isStart)
                        {
                            ResetSession();
                            currentTrackDesc = ACHelper.GetCurrentTrack(s);
                            currentLap = g.CompletedLaps;
                            lastTime = g.iCurrentTime;
                            ResetTelemetry();
                        }

                        // Track laps
                        if (g.iCurrentTime < lastTime)
                        {
                            ResetTelemetry();
                        }
                        else if (g.Status == AC_STATUS.AC_LIVE)
                        {
                            AddTelemetry(g, p, s);
                        }

                        this.lblCurrentTrack.Text = currentTrackDesc.TrackCode;
                        this.lblOutputFolder.Text = OutputFolder;
                        this.lblPosition.Text = g.CarCoordinates[0].ToString("0.00000") + " x " + g.CarCoordinates[2].ToString("0.00000");

                        lastStatus = g.Status;
                        lastTime = g.iCurrentTime;
                    }
                    catch
                    {
                        ResetSession();
                    }
                }
            }
            finally
            {
            }
        }

        private void ResetSession()
        {
            DumpTrackpositions();
            ResetTelemetry();
            lastStatus = AC_STATUS.AC_OFF;
            lastTime = 0;
            this.lblPosition.Text = "";
            this.lblCurrentTrack.Text = "Waiting for race to start";
        }

        private void DumpTrackpositions()
        {
            if (currentTrackDesc != null && MapPosition != null && MapPosition.Count > 0)
            {
                System.IO.File.WriteAllText(Path.Combine(OutputFolder, "trackdata__" + currentTrackDesc.TrackCode + "__" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".trackrecord"),
                                                Newtonsoft.Json.JsonConvert.SerializeObject(MapPosition, Newtonsoft.Json.Formatting.Indented)
                                            );
                MapPosition = new List<float[]>();
            }
        }

        private TimeSpan toTimeSpan(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }
    }
}