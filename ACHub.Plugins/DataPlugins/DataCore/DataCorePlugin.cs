using ACToolsUtilities;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace ACHub.Plugins.DataPlugins.DataCore
{
    /// <summary>
    /// Core plugin
    /// </summary>
    public class DataCorePlugin : IDataPlugin
    {
        private const string SettingsPath = "PluginsData\\DataCorePlugin.json";
        private const string SettingsCoreExpressionsPath = "CoreData\\DataCorePlugin\\CustomExpressions";
        private const string SettingsUserExpressionsPath = "PluginsData\\DataCorePlugin\\CustomExpressions";
        private float fuel_AverageConsumptionPerLap;
        private bool _fuel_InvalidLap = true;

        private bool fuel_InvalidLap
        {
            get { return _fuel_InvalidLap; }
            set
            {
                _fuel_InvalidLap = value;
                if (value)
                {
                    Logging.Current.Info("CurrentLap invalidated for Fuel stats");
                }
                else
                {
                    Logging.Current.Info("Fuel stats tracking resetted");
                }

            }
        }
        private float fuel_LastLapFuel = 0;
        private PluginManager pluginManager;
        private DataCorePluginSettings settings = new DataCorePluginSettings();

        private DataCorePluginSettingsControl settingsControl = null;

        /// <summary>
        /// Plugin name
        /// </summary>
        public string Name
        {
            get { return "Core Data Plugin"; }
        }

        /// <summary>
        /// Plugin settings
        /// </summary>
        public DataCorePluginSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        private Dictionary<string, FuelStatistics> stats = new Dictionary<string, FuelStatistics>();

        /// <summary>
        /// On new data
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data"></param>
        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
            if (data.GameRunning)
            {
                //if (data.NewData != null)
                //{
                //    if (data.NewData.Physics.SpeedKmh > 2)
                //    {

                //        //float triggerdelta  = data.NewData.Physics.SpeedKmh * 50/100

                //        if (data.NewData.Physics.WheelSlip.Any(i => i > 0.2))
                //        {
                //            pluginManager.TriggerEvent("WheelSlip", typeof(DataCorePlugin));
                //        }


                //    }
                //}
                if (data.NewData != null && data.Events.PitChanged)
                {
                    if (data.NewData.Graphics.IsInPit > 0)
                    {
                        pluginManager.TriggerEvent("PitEnter", typeof(DataCorePlugin));
                    }
                    else
                    {
                        pluginManager.TriggerEvent("PitOut", typeof(DataCorePlugin));
                    }
                }

                if (data.NewData != null)
                {
                    try
                    {
                        fuel_AverageConsumptionPerLap = -1;

                        string filename = GetFuelStorage(data);
                        if (!stats.ContainsKey(filename))
                        {
                            var tmp = JsonExtensions.FromJsonFile<FuelStatistics>(filename) ?? new FuelStatistics();
                            stats.Add(filename, tmp);
                        }

                        var fueldata = stats[filename];
                        if (fueldata.Consumption.Count() > 0)
                        {
                            fuel_AverageConsumptionPerLap = fueldata.Consumption.Take(5).Average();
                        }

                    }
                    catch { }
                }

                if (data.NewData != null && data.NewData.StaticInfo.MaxFuel > 0)
                {
                    pluginManager.SetPropertyValue("Computed.Fuel_Percent", typeof(DataCorePlugin), (double)(data.NewData.Physics.Fuel / data.NewData.StaticInfo.MaxFuel * 100f));
                }

                if (data.NewData != null && data.OldData != null)
                {
                    if (data.NewData.Physics.Fuel > data.OldData.Physics.Fuel)
                    {
                        fuel_InvalidLap = true;
                    }
                    if (data.NewData.Graphics.IsInPit > 0) { fuel_InvalidLap = true; }
                }

                pluginManager.SetPropertyValue("Computed.Fuel_RemainingLaps", typeof(DataCorePlugin), null);
                pluginManager.SetPropertyValue("Computed.Fuel_LitersPerLap", typeof(DataCorePlugin), null);

                if (fuel_AverageConsumptionPerLap > 0)
                {
                    pluginManager.SetPropertyValue("Computed.Fuel_RemainingLaps", typeof(DataCorePlugin), data.NewData.Physics.Fuel / fuel_AverageConsumptionPerLap);
                    pluginManager.SetPropertyValue("Computed.Fuel_LitersPerLap", typeof(DataCorePlugin), fuel_AverageConsumptionPerLap);
                }
            }


            UpdateData(pluginManager, "GameData", data);
            foreach (var expr in settings.Expressions)
            {
                try
                {
                    var assemblies = expr.Assemblies.ToList();
                    assemblies.Add(typeof(PluginManager).Assembly.Location);
                    assemblies.Add(typeof(ACSharedMemory.ACManager).Assembly.Location);

                    var result = EvalProvider.EvalCode<PluginManager, object>(pluginManager, expr.Code, null, assemblies.ToArray());
                    pluginManager.SetPropertyValue(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), result.TypedMethod(pluginManager));
                }
                catch
                {
                    pluginManager.SetPropertyValue(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), null);
                }
            }
            // UpdateVolumeProperty(pluginManager);
        }

        private static string GetFuelStorage(ACSharedMemory.GameData data)
        {
            string filename =
                Path.Combine("PluginsData", "DataCorePlugin", "FuelTracking", string.Concat(data.Car.Model, "_", data.Track.TrackCode, ".json"));
            return filename;
        }

        /// <summary>
        /// On plugin shutdown
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            this.SaveSettings();
        }

        /// <summary>
        /// Returns settings control
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new DataCorePluginSettingsControl(this, pluginManager);
            }
            return settingsControl;
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
            DeclareType(pluginManager, typeof(ACSharedMemory.GameData), "GameData");

            pluginManager.NewLap += ACManager_NewLap;
            pluginManager.SessionRestart += ACManager_SessionRestart;
            pluginManager.GameStatusChanged += ACManager_GameStatusChanged;
            pluginManager.GameStateChanged += ACManager_GameStateChanged;

            pluginManager.AddEvent("NewLap", typeof(DataCorePlugin));
            pluginManager.AddEvent("NewValidLap", typeof(DataCorePlugin));
            pluginManager.AddEvent("SessionRestarted", typeof(DataCorePlugin));
            pluginManager.AddEvent("SessionStatusChanged", typeof(DataCorePlugin));
            pluginManager.AddEvent("GameStarted", typeof(DataCorePlugin));
            pluginManager.AddEvent("GameStopped", typeof(DataCorePlugin));
            pluginManager.AddEvent("GameVolumeChanged", typeof(DataCorePlugin));
            pluginManager.AddEvent("PitEnter", typeof(DataCorePlugin));
            pluginManager.AddEvent("PitOut", typeof(DataCorePlugin));

            pluginManager.AddEvent("WheelSlip", typeof(DataCorePlugin));

            pluginManager.AddAction("StartReplay", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_R); });
            pluginManager.AddAction("StartSlowMotion", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_S); });
            pluginManager.AddAction("SwitchABS", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A); });
            pluginManager.AddAction("SwitchTractionControl", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_T); });
            pluginManager.AddAction("SwitchInGameApps", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_H); });
            pluginManager.AddAction("RestartSession", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_O); });
            pluginManager.AddAction("SwitchPlayersNames", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_L); });
            pluginManager.AddAction("SwitchAutomaticGearbox", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_G); });
            pluginManager.AddAction("SwitchRacingLine", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_I); });
            pluginManager.AddAction("SwitchDamageDisplayer", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SimulateKeyPress(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_Q); });

            pluginManager.AddAction("IncrementGameVolume", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning)SetGameVolume(10); });
            pluginManager.AddAction("DecrementGameVolume", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) SetGameVolume(-10); });

            pluginManager.AddAction("IncrementSystemVolume", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) VolumeMixer.VolumeUp(); VolumeMixer.VolumeUp(); }); ;
            pluginManager.AddAction("DecrementSystemVolume", typeof(DataCorePlugin), (pm, a) => { if (pm.Status.GameRunning) VolumeMixer.VolumeDown(); VolumeMixer.VolumeDown(); });

            LoadSettings();
        }

        /// <summary>
        /// Save settings
        /// </summary>
        public void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);

            JsonExtensions.CleanFolder(SettingsUserExpressionsPath);
            foreach (var expression in this.settings.Expressions.Where(i => !i.IsCore))
            {
                expression.ToJsonFile(Path.Combine(SettingsUserExpressionsPath, expression.Name + ".json"));
            }

            JsonExtensions.CleanFolder(SettingsCoreExpressionsPath);
            foreach (var expression in this.settings.Expressions.Where(i => i.IsCore))
            {
                expression.ToJsonFile(Path.Combine(SettingsCoreExpressionsPath, expression.Name + ".json"));
            }

            DeclareProperties();
        }

        private void ACManager_GameStateChanged(bool running, PluginManager manager)
        {
            if (!running)
            {
                pluginManager.TriggerEvent("GameStopped", typeof(DataCorePlugin));
            }
            else
            {
                pluginManager.TriggerEvent("GameStarted", typeof(DataCorePlugin));
            }
        }

        private void ACManager_GameStatusChanged(ACSharedMemory.AC_STATUS status, PluginManager manager)
        {
            pluginManager.TriggerEvent("SessionStatusChanged", typeof(DataCorePlugin));
        }

        private void ACManager_NewLap(int completedLapNumber, bool testLap, PluginManager manager)
        {
            string filename = GetFuelStorage(manager.Status);

            var data = JsonExtensions.FromJsonFile<FuelStatistics>(filename) ?? new FuelStatistics();

            fuel_AverageConsumptionPerLap = -1;

            if (!testLap && !fuel_InvalidLap && fuel_LastLapFuel - manager.Status.NewData.Physics.Fuel > 0)
            {
                data.Consumption.Insert(0, fuel_LastLapFuel - manager.Status.NewData.Physics.Fuel);
                data.ToJsonFile(filename);
            }

            if (stats.ContainsKey(filename))
            {
                stats.Remove(filename);
            }

            fuel_InvalidLap = false;
            fuel_LastLapFuel = manager.Status.NewData.Physics.Fuel;

            pluginManager.TriggerEvent("NewLap", typeof(DataCorePlugin));

            if (!testLap)
            {
                pluginManager.TriggerEvent("NewValidLap", typeof(DataCorePlugin));
            }
        }

        private void ACManager_SessionRestart(PluginManager manager)
        {
            pluginManager.TriggerEvent("SessionRestarted", typeof(DataCorePlugin));
        }

        private void DeclareProperties()
        {
            pluginManager.ClearProperties(typeof(DataCorePlugin));
            DeclareType(pluginManager, typeof(ACSharedMemory.GameData), "GameData");
            foreach (var expr in settings.Expressions)
            {
                pluginManager.AddProperty(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), typeof(object));
            }
            pluginManager.AddProperty(string.Concat("GameVolume"), typeof(DataCorePlugin), typeof(int));

            pluginManager.AddProperty("Computed.Fuel_Percent", typeof(DataCorePlugin), typeof(double));
            pluginManager.AddProperty("Computed.Fuel_RemainingLaps", typeof(DataCorePlugin), typeof(double));
            pluginManager.AddProperty("Computed.Fuel_LitersPerLap", typeof(DataCorePlugin), typeof(double));

            UpdateVolumeProperty(pluginManager);
        }

        private void DeclareType(PluginManager pluginManager, Type type, string currentName)
        {
            //
            if (type == typeof(TimeSpan) || type == typeof(string) || type.IsEnum || type.IsPrimitive || type.IsArray || type.Name == "List`1")
            {
                if (!type.IsArray && !(type.Name == "List`1"))
                {
                    pluginManager.AddProperty(currentName, typeof(DataCorePlugin), type);
                    //Debug.WriteLine(currentName);
                }
            }
            else
            {
                var props = type.GetProperties();
                foreach (var prop in props)
                {
                    DeclareType(pluginManager, prop.PropertyType, string.Concat(currentName, ".", prop.Name));
                }

                var fields = type.GetFields();
                foreach (var field in fields)
                {
                    DeclareType(pluginManager, field.FieldType, string.Concat(currentName, ".", field.Name));
                }
            }
        }

        private float GetGameVolume()
        {
            var process = Process.GetProcessesByName("acs").FirstOrDefault();
            if (process != null)
            {
                float? volume = 0;
                volume = VolumeMixer.GetApplicationVolume(process.Id);
                return volume.GetValueOrDefault(0);
            }
            return 0;
        }

        private void LoadSettings()
        {
            this.settings = JsonExtensions.FromJsonFile<DataCorePluginSettings>(SettingsPath) ?? new DataCorePluginSettings();
            this.settings.Expressions = new List<Expression>();



            foreach (var expressionfile in JsonExtensions.GetFiles(SettingsUserExpressionsPath, "*.json"))
            {
                var exp = JsonExtensions.FromJsonFile<Expression>(expressionfile);
                if (exp != null)
                {
                    this.settings.Expressions.Add(exp);
                    exp.IsCore = false;
                }
                else
                {
                    Logging.Current.WarnFormat("Expression file {0} cannot be read", expressionfile);
                }
            }

            foreach (var expressionfile in JsonExtensions.GetFiles(SettingsCoreExpressionsPath, "*.json"))
            {
                var exp = JsonExtensions.FromJsonFile<Expression>(expressionfile);
                if (exp != null)
                {
                    this.settings.Expressions.Add(exp);
                    exp.IsCore = true;
                }
                else
                {
                    Logging.Current.WarnFormat("Expression file {0} cannot be read", expressionfile);
                }
            }

            DeclareProperties();
        }

        private void SetGameVolume(int delta)
        {
            var process = Process.GetProcessesByName("acs").FirstOrDefault();
            if (process != null)
            {
                float? volume = 0;
                volume = VolumeMixer.GetApplicationVolume(process.Id);

                volume = Math.Max(0, Math.Min(100, volume.GetValueOrDefault(0) + delta));
                VolumeMixer.SetApplicationVolume(process.Id, volume.GetValueOrDefault(0));
            }

            UpdateVolumeProperty(pluginManager);
            pluginManager.TriggerEvent("GameVolumeChanged", typeof(DataCorePlugin));
        }

        private void SimulateKeyPress(params VirtualKeyCode[] keys)
        {
            var isim = new InputSimulator();
            for (int i = 0; i < keys.Length; i++)
            {
                isim.Keyboard.KeyDown(keys[i]);
            }
            Thread.Sleep(20);
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                isim.Keyboard.KeyUp(keys[i]);
            }
        }

        private void UpdateData(PluginManager pluginManager, string currentName, object value)
        {
            if (value == null)
                return;

            var type = value.GetType();

            if (type == typeof(TimeSpan) || type == typeof(string) || type.IsEnum || type.IsPrimitive || type.IsArray || type.Name == "List`1")
            {
                if (!type.IsArray && !(type.Name == "List`1"))
                {
                    pluginManager.SetPropertyValue(currentName, typeof(DataCorePlugin), value);
                    //Debug.WriteLine(currentName);
                }
            }
            else
            {
                var props = type.GetProperties();
                foreach (var prop in props)
                {
                    UpdateData(pluginManager, string.Concat(currentName, ".", prop.Name), prop.GetValue(value));
                }

                var fields = type.GetFields();
                foreach (var field in fields)
                {
                    UpdateData(pluginManager, string.Concat(currentName, ".", field.Name), field.GetValue(value));
                }
            }
        }

        private void UpdateVolumeProperty(PluginManager pluginManager)
        {
            pluginManager.SetPropertyValue(string.Concat("GameVolume"), typeof(DataCorePlugin), (int)GetGameVolume());
        }
    }
}