using ACSharedMemory;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TimingClient.Plugins.DataPlugins.PersistantTrackerPlugin
{
    public class PersistantTrackerPlugin : IDataPlugin
    {
        private ACManager ACManager;

        private DataRecord record { get; set; }

        private DataRecord best { get; set; }

        public string Name
        {
            get { return "Persistant tracker Plugin"; }
        }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void Init(PluginManager pluginManager)
        {
            this.ACManager = pluginManager.ACManager;

            pluginManager.AddProperty("AllTimeBestDelta", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("AllTimeBest", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));

            ACManager.CarChanged += manager_CarChanged;
            ACManager.NewLap += manager_NewLap;
            ACManager.SessionRestart += manager_SessionRestart;
            ACManager.GameStatusChanged += manager_GameStatusChanged;
        }

        private void manager_GameStatusChanged(AC_STATUS status, ACManager manager)
        {
            record = new DataRecord();
        }

        private void manager_SessionRestart(ACManager manager)
        {
            record = new DataRecord();
        }

        private void manager_CarChanged(ACSharedMemory.Models.Car.CarDesc newCar, ACManager manager)
        {
            record = new DataRecord();
            best = FindBestLap(manager);
        }

        private string GetStoragePath(ACManager manager)
        {
            return Path.Combine("PluginsData", "PersistantTracker", manager.Status.Track.TrackCode, manager.Status.Car.Model);
        }

        public DataRecord FindBestLap(ACManager manager)
        {
            if (manager.Status.Track == null)
            {
                return null;
            }

            if (manager.Status.Car == null)
            {
                return null;
            }

            var storageFolder = GetStoragePath(manager);
            if (Directory.Exists(storageFolder))
            {
                var result = Directory.GetFiles(storageFolder, "*_*.json").OrderBy(i => i).FirstOrDefault();
                if (result != null)
                {
                    var resultRecord = JsonExtensions.FromJsonFile<DataRecord>(result);
                    Debug.WriteLine("Best time found :" + resultRecord.LapTime.ToString());
                    return resultRecord;
                }
            }
            return null;
        }

        private void manager_NewLap(int completedLapNumber, bool testLap, ACManager manager)
        {
            // Save Lap
            if (!testLap)
            {
                record.LapNumber = completedLapNumber;
                record.LapTime = ACHelper.ToTimeSpan(manager.Status.NewData.Graphics.iLastTime);
                record.CarPositions.Add(new KeyValuePair<TimeSpan, float>(record.LapTime, 1));
                record.RecordDate = DateTime.Now;
                record.ToJsonFile(
                    Path.Combine(
                    GetStoragePath(manager),
                    record.LapTime.ToString(@"hh\.mm\.ss\.ffff") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json"));
            }

            // Create ne container and maintain session id
            record = new DataRecord { SessionId = record.SessionId };

            var newbest = FindBestLap(manager);

            if (newbest != null)
            {
                if (newbest != null && best == null)
                {
                    Debug.WriteLine("New record !!!");
                }
                else if (best == null && newbest != null && best.LapId == newbest.LapId)
                {
                    Debug.WriteLine("New best !!!");
                }
            }

            best = newbest;
        }

        public void End(PluginManager pluginManager)
        {
            record = new DataRecord();
        }

        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
            TimeSpan? AllTimeBestDelta = null;
            TimeSpan? AllTimeBest = null;

            if (data.GameRunning)
            {
                var g = data.NewData.Graphics;
                if (record.CarPositions.Count == 0 || record.CarPositions.Last().Key.TotalMilliseconds + 50 < ACHelper.ToTimeSpan(g.iCurrentTime).TotalMilliseconds)
                {
                    record.CarPositions.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iCurrentTime), g.NormalizedCarPosition));
                }

                if (best != null)
                {
                    AllTimeBestDelta = ACHelper.GetLapDelta(data.NewData.Graphics, best.CarPositions);
                    AllTimeBest = best.LapTime;
                }
            }

            pluginManager.SetPropertyValue("AllTimeBestDelta", typeof(PersistantTrackerPlugin), AllTimeBestDelta);
            pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), AllTimeBest);
        }

        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            return null;
        }

        public List<string> GetActions(PluginManager pluginManager)
        {
            return null;
        }

        public void DoAction(PluginManager pluginManager, string command)
        {
        }
    }
}