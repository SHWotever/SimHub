using ACSharedMemory;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ACHub.Plugins.DataPlugins.PersistantTracker
{
    /// <summary>
    /// Lap timings tracker
    /// </summary>
    public class PersistantTrackerPlugin : IDataPlugin
    {
        private PluginManager pluginManager;

        private DataRecord record { get; set; }

        private DataRecord best { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return "Persistant tracker Plugin"; }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            record = new DataRecord();
            this.pluginManager = pluginManager;

            pluginManager.AddProperty("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("AllTimeBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), typeof(double?));
            pluginManager.AddProperty("AllTimeBest", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("AllTimeBestLastLapDelta", typeof(PersistantTrackerPlugin), typeof(double));

            pluginManager.SetPropertyValue("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBestLastLapDelta", typeof(PersistantTrackerPlugin), 0);

            pluginManager.SetPropertyValue("AllTimeBestLastSector", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBestLastSectorDelta", typeof(PersistantTrackerPlugin), 0);

            pluginManager.AddEvent("NewAllTimeBest", typeof(PersistantTrackerPlugin));
            pluginManager.AddEvent("NewRecord", typeof(PersistantTrackerPlugin));

            pluginManager.CarChanged += manager_CarChanged;
            pluginManager.NewLap += manager_NewLap;
            pluginManager.SessionRestart += manager_SessionRestart;
            pluginManager.GameStatusChanged += manager_GameStatusChanged;
        }

        private void manager_GameStatusChanged(AC_STATUS status, PluginManager manager)
        {
            SavePartial();
            record = new DataRecord();
        }

        private void SavePartial()
        {
            if (record != null && record.CarCoordinates.Count > 0)
            {
                record.ToJsonFile(
                Path.Combine("PluginsData", "PersistantTracker",
              "lastpartial.json"));
            }
        }

        private void manager_SessionRestart(PluginManager manager)
        {
            SavePartial();
            record = new DataRecord();
        }

        private void manager_CarChanged(ACSharedMemory.Models.Car.CarDesc newCar, PluginManager manager)
        {
            SavePartial();
            record = new DataRecord();
            best = FindBestLap(manager);
        }

        private string GetStoragePath(PluginManager manager)
        {
            return Path.Combine("PluginsData", "PersistantTracker", manager.Status.Track.TrackCode, manager.Status.Car.Model);
        }

        private DataRecord FindBestLap(PluginManager manager)
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
                var result = JsonExtensions.GetFiles(storageFolder, "*_*.json").OrderBy(i => i).FirstOrDefault();
                if (result != null)
                {
                    var resultRecord = JsonExtensions.FromJsonFile<DataRecord>(result);
                    Logging.Current.Info("Best time found :" + resultRecord.LapTime.ToString());

                    List<KeyValuePair<TimeSpan, float>> tmp = new List<KeyValuePair<TimeSpan, float>>(resultRecord.CarPositions);

                    return resultRecord;
                }
            }
            return null;
        }

        private bool newRecordEvent = false;
        private bool newBestEvent = false;

        private void manager_NewLap(int completedLapNumber, bool testLap, PluginManager manager)
        {
            SavePartial();

            // Save Lap
            if (!testLap)
            {
                if (manager.Status.OldData != null && manager.Status.NewData != null)
                {
                    if (record.SectorsTime.ContainsKey(manager.Status.OldData.Graphics.CurrentSectorIndex))
                    {
                        record.SectorsTime.Remove(manager.Status.OldData.Graphics.CurrentSectorIndex);
                    }
                    record.SectorsTime.Add(manager.Status.OldData.Graphics.CurrentSectorIndex, TimeSpan.FromMilliseconds(manager.Status.NewData.Graphics.LastSectorTime));
                }

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

            // Update (or reset) last lap delta
            if (best != null && !testLap)
            {
                pluginManager.SetPropertyValue("AllTimeBestLastLapDelta",
                    typeof(PersistantTrackerPlugin),
                    (manager.Status.NewData.LastLapTime - best.LapTime).TotalMilliseconds / 1000d);
            }
            else
            {
                pluginManager.SetPropertyValue("AllTimeBestLastLapDelta",
                    typeof(PersistantTrackerPlugin),
                    0);
            }

            var newbest = FindBestLap(manager);

            if (newbest != null)
            {
                if (newbest != null && best == null)
                {
                    newRecordEvent = true;
                    Logging.Current.Info("New record saved");
                }
                else if (best == null && newbest != null && best.LapId == newbest.LapId)
                {
                    newBestEvent = true;
                    Logging.Current.Info("New best saved");
                }
            }

            if (newbest != null && newbest.SectorsTime == null)
            {
                newbest.SectorsTime = new Dictionary<int, TimeSpan>();
            }

            best = newbest;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            SavePartial();
            record = new DataRecord();
        }

        /// <summary>
        /// Date updae
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data"></param>
        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
            TimeSpan? AllTimeBestDelta = null;
            TimeSpan? AllTimeBest = null;

            if (data.GameRunning)
            {
                if (data.OldData != null && data.OldData.Graphics.CurrentSectorIndex != data.NewData.Graphics.CurrentSectorIndex)
                {
                    if (record.SectorsTime.ContainsKey(data.OldData.Graphics.CurrentSectorIndex))
                    {
                        record.SectorsTime.Remove(data.OldData.Graphics.CurrentSectorIndex);
                    }
                    record.SectorsTime.Add(data.OldData.Graphics.CurrentSectorIndex, TimeSpan.FromMilliseconds(data.NewData.Graphics.LastSectorTime));
                }

                var g = data.NewData.Graphics;
                if (record.CarPositions.Count == 0 || record.CarPositions.Last().Key.TotalMilliseconds + 50 < ACHelper.ToTimeSpan(g.iCurrentTime).TotalMilliseconds)
                {
                    record.CarPositions.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iCurrentTime), g.NormalizedCarPosition));
                    record.CarCoordinates.Add(new KeyValuePair<TimeSpan, float[]>(ACHelper.ToTimeSpan(g.iCurrentTime), g.CarCoordinates));
                }

                if (best != null)
                {
                    if (best.CarCoordinates == null || best.CarCoordinates.Count == 0)
                    {
                        AllTimeBestDelta = ACHelper.GetLapDeltaAlternative(data.NewData.Graphics, best.CarPositions);
                        AllTimeBest = best.LapTime;
                    }
                    else
                    {
                        AllTimeBestDelta = ACHelper.GetLapDeltaAlternative(data.NewData.Graphics, best.CarCoordinates);
                        AllTimeBest = best.LapTime;
                    }
                }
            }

            pluginManager.SetPropertyValue("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), AllTimeBestDelta);
            pluginManager.SetPropertyValue("AllTimeBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin),
                !AllTimeBestDelta.HasValue ? null : (double?)(AllTimeBestDelta.Value.TotalMilliseconds / 1000));
            pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), AllTimeBest);

            if (newRecordEvent)
            {
                pluginManager.TriggerEvent("NewAllTimeBest", typeof(PersistantTrackerPlugin));
            }
            if (newBestEvent)
            {
                pluginManager.TriggerEvent("NewRecord", typeof(PersistantTrackerPlugin));
            }

            newRecordEvent = false;
            newBestEvent = false;
        }

        /// <summary>
        /// Settings control
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            return null;
        }
    }
}