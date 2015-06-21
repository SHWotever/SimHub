using ACSharedMemory;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACHub.Plugins.DataPlugins.PersistantTracker
{
    internal class AveragingValues
    {
        public AveragingValues()
        {
            AveragingSeconds = 4;
        }

        public double AveragingSeconds { get; set; }

        private List<KeyValuePair<DateTime, double>> Values = new List<KeyValuePair<DateTime, double>>();

        public void AddValue(double value)
        {
            var reftime = DateTime.Now;
            Values.Add(new KeyValuePair<DateTime, double>(reftime, value));

            while (Values.Count > 0 && (reftime - Values[0].Key).TotalSeconds > AveragingSeconds)
            {
                Values.RemoveAt(0);
            }
        }

        public double GetAverage()
        {
            if (Values.Count == 0)
            {
                return 0;
            }
            else
            {
                return Values.Average(i => i.Value);
            }
        }

        public void Clear()
        {
            Values.Clear();
        }
    }

    /// <summary>
    /// Lap timings tracker
    /// </summary>
    public class PersistantTrackerPlugin : IDataPlugin
    {
        private AveragingValues sessionRelativeData = new AveragingValues();
        private AveragingValues allTimeRelativeData = new AveragingValues();

        private Guid sessionId = new Guid();

        private PluginManager pluginManager;

        private DataRecord record { get; set; }

        private DataRecord best { get; set; }

        private DataRecord sessionBest { get; set; }

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
            pluginManager.AddProperty("AllTimeBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), typeof(double?));
            pluginManager.AddProperty("AllTimeBest", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("AllTimeBestLastLapDelta", typeof(PersistantTrackerPlugin), typeof(double));

            pluginManager.AddProperty("SessionBestLiveDelta", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("SessionBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), typeof(double?));
            pluginManager.AddProperty("SessionBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), typeof(double?));
            pluginManager.AddProperty("SessionBest", typeof(PersistantTrackerPlugin), typeof(TimeSpan?));
            pluginManager.AddProperty("SessionBestLastLapDelta", typeof(PersistantTrackerPlugin), typeof(double));

            pluginManager.SetPropertyValue("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), null);
            pluginManager.SetPropertyValue("AllTimeBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), 0);
            pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBestLastLapDelta", typeof(PersistantTrackerPlugin), 0);

            pluginManager.SetPropertyValue("SessionBestLiveDelta", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
            pluginManager.SetPropertyValue("SessionBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), null);
            pluginManager.SetPropertyValue("SessionBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), 0);
            pluginManager.SetPropertyValue("SessionBest", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
            pluginManager.SetPropertyValue("SessionBestLastLapDelta", typeof(PersistantTrackerPlugin), 0);

            pluginManager.SetPropertyValue("AllTimeBestLastSector", typeof(PersistantTrackerPlugin), TimeSpan.FromSeconds(0));
            pluginManager.SetPropertyValue("AllTimeBestLastSectorDelta", typeof(PersistantTrackerPlugin), 0);

            pluginManager.AddEvent("NewAllTimeBest", typeof(PersistantTrackerPlugin));
            pluginManager.AddEvent("NewSessionBest", typeof(PersistantTrackerPlugin));
            pluginManager.AddEvent("NewRecord", typeof(PersistantTrackerPlugin));

            pluginManager.CarChanged += manager_CarChanged;
            pluginManager.NewLap += manager_NewLap;
            pluginManager.SessionRestart += manager_SessionRestart;
            pluginManager.GameStatusChanged += manager_GameStatusChanged;
            pluginManager.GameStateChanged += pluginManager_GameStateChanged;
        }

        private void pluginManager_GameStateChanged(bool running, PluginManager manager)
        {
            SavePartial();

            sessionId = Guid.NewGuid();
            sessionBest = null;
        }

        private void manager_GameStatusChanged(AC_STATUS status, PluginManager manager)
        {
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
                var files = new List<string>();

                files.AddRange(JsonExtensions.GetFiles(storageFolder, "*_*.json").ToList());
                files.AddRange(JsonExtensions.GetFiles(storageFolder, "*_*.json.gz").ToList());

                var result = files.OrderBy(i => i).FirstOrDefault();
                if (result != null)
                {
                    DataRecord resultRecord = null;
                    if (Path.GetExtension(result).ToLower().Contains("gz"))
                    {
                        resultRecord = JsonExtensions.FromJsonGZipFile<DataRecord>(result);
                    }
                    else
                    {
                        resultRecord = JsonExtensions.FromJsonFile<DataRecord>(result);
                    }
                    Logging.Current.Info("Best time found :" + resultRecord.LapTime.ToString());

                    return resultRecord;
                }
            }
            return null;
        }

        private bool newRecordEvent = false;

        private bool newBestEvent = false;
        private bool newSessionBestEvent = false;

        private void manager_NewLap(int completedLapNumber, bool testLap, PluginManager manager)
        {
            SavePartial();
            sessionRelativeData.Clear();
            allTimeRelativeData.Clear();
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
                record.SessionId = sessionId;
                record.LapNumber = completedLapNumber;
                record.LapTime = ACHelper.ToTimeSpan(manager.Status.NewData.Graphics.iLastTime);
                record.CarPositions.Add(new KeyValuePair<TimeSpan, float>(record.LapTime, 1));

                record.RecordDate = DateTime.Now;
                string file = Path.Combine(
                    GetStoragePath(manager),
                    record.LapTime.ToString(@"hh\.mm\.ss\.ffff") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json");

                //record.ToJsonFile(file);
                record.ToJsonGZipFile(file + ".gz");
            }
            if (!testLap)
            {
                if (sessionBest == null)
                {
                    sessionBest = record;
                    pluginManager.SetPropertyValue("SessionBestLastLapDelta", typeof(PersistantTrackerPlugin), 0);
                }
                else
                {
                    pluginManager.SetPropertyValue("SessionBestLastLapDelta", typeof(PersistantTrackerPlugin), (manager.Status.NewData.LastLapTime - sessionBest.LapTime).TotalMilliseconds / 1000d);
                    if (sessionBest.LapTime > record.LapTime)
                    {
                        sessionBest = record;
                        newSessionBestEvent = true;
                        Logging.Current.Info("New session best");
                    }
                }
            }
            // Create ne container and maintain session id
            record = new DataRecord { SessionId = sessionId };

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
            TimeSpan AllTimeBestDelta = TimeSpan.Zero;
            TimeSpan? AllTimeBest = TimeSpan.Zero;

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
                    //record.CarPositions.Add(new KeyValuePair<TimeSpan, float>(ACHelper.ToTimeSpan(g.iCurrentTime), g.NormalizedCarPosition));
                    record.CarCoordinates.Add(new KeyValuePair<TimeSpan, float[]>(ACHelper.ToTimeSpan(g.iCurrentTime), g.CarCoordinates));
                }

                if (best == null)
                {
                    pluginManager.SetPropertyValue("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
                    pluginManager.SetPropertyValue("AllTimeBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), null);
                    pluginManager.SetPropertyValue("AllTimeBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), null);
                    pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
                }
                else
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

                    pluginManager.SetPropertyValue("AllTimeBestLiveDelta", typeof(PersistantTrackerPlugin), AllTimeBestDelta);
                    pluginManager.SetPropertyValue("AllTimeBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), (double?)(AllTimeBestDelta.TotalMilliseconds / 1000));
                    pluginManager.SetPropertyValue("AllTimeBest", typeof(PersistantTrackerPlugin), AllTimeBest);

                    pluginManager.SetPropertyValue("AllTimeLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), (double?)Math.Round((double)AllTimeBestDelta.TotalMilliseconds / 1000 - allTimeRelativeData.GetAverage(), 2));
                    allTimeRelativeData.AddValue(AllTimeBestDelta.TotalMilliseconds / 1000);
                }

                if (sessionBest == null)
                {
                    pluginManager.SetPropertyValue("SessionBestLiveDelta", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
                    pluginManager.SetPropertyValue("SessionBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), null);
                    pluginManager.SetPropertyValue("SessionBest", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
                    pluginManager.SetPropertyValue("SessionBestLastLapDelta", typeof(PersistantTrackerPlugin), TimeSpan.Zero);
                    pluginManager.SetPropertyValue("SessionBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), null);
                }
                else
                {
                    var sessionbestdelta = ACHelper.GetLapDeltaAlternative(data.NewData.Graphics, sessionBest.CarCoordinates);

                    pluginManager.SetPropertyValue("SessionBestLiveDelta", typeof(PersistantTrackerPlugin), sessionbestdelta);
                    pluginManager.SetPropertyValue("SessionBestLiveDeltaSeconds", typeof(PersistantTrackerPlugin), (double?)(sessionbestdelta.TotalMilliseconds / 1000));
                    pluginManager.SetPropertyValue("SessionBest", typeof(PersistantTrackerPlugin), sessionBest.LapTime);
                    pluginManager.SetPropertyValue("SessionBestLiveDeltaProgressSeconds", typeof(PersistantTrackerPlugin), (double?)Math.Round((double)sessionbestdelta.TotalMilliseconds / 1000 - sessionRelativeData.GetAverage(), 2));
                    sessionRelativeData.AddValue((double)(sessionbestdelta.TotalMilliseconds / 1000));
                }
            }

            if (newRecordEvent)
            {
                pluginManager.TriggerEvent("NewAllTimeBest", typeof(PersistantTrackerPlugin));
            }

            if (newBestEvent)
            {
                pluginManager.TriggerEvent("NewRecord", typeof(PersistantTrackerPlugin));
            }
            if (newSessionBestEvent)
            {
                pluginManager.TriggerEvent("NewSessionBest", typeof(PersistantTrackerPlugin));
            }
            newSessionBestEvent = false;
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