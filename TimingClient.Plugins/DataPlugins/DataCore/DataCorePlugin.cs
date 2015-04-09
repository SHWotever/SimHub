using ACToolsUtilities;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TimingClient.Plugins.DataPlugins.DataCore
{
    public class DataCorePlugin : IDataPlugin
    {
        private PluginManager pluginManager;
        private const string SettingsPath = "PluginsData\\DataCorePlugin.json";
        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
            UpdateData(pluginManager, "GameData", data);
            foreach (var expr in settings.Expressions)
            {
                try
                {
                    var assemblies = expr.Assemblies.ToList();
                    assemblies.Add(typeof(PluginManager).Assembly.Location);
                    var result = EvalProvider.EvalCode<PluginManager, object>(pluginManager, expr.Code, null, assemblies.ToArray());
                    pluginManager.SetPropertyValue(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), result.TypedMethod(pluginManager));
                }
                catch
                {
                    pluginManager.SetPropertyValue(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), null);
                }
            }
        }

        public string Name
        {
            get { return "Core Data Plugin"; }
        }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void Init(PluginManager pluginManager)
        {

            this.pluginManager = pluginManager;
            DeclareType(pluginManager, typeof(ACSharedMemory.GameData), "GameData");
            pluginManager.ACManager.NewLap += ACManager_NewLap;
            pluginManager.ACManager.SessionRestart += ACManager_SessionRestart;
            pluginManager.ACManager.GameStatusChanged += ACManager_GameStatusChanged;
            pluginManager.ACManager.GameStateChanged += ACManager_GameStateChanged;

            pluginManager.AddEvent("NewLap", typeof(DataCorePlugin));
            pluginManager.AddEvent("SessionRestarted", typeof(DataCorePlugin));
            pluginManager.AddEvent("SessionStatusChanged", typeof(DataCorePlugin));
            pluginManager.AddEvent("GameStarted", typeof(DataCorePlugin));
            pluginManager.AddEvent("GameStopped", typeof(DataCorePlugin));

            LoadSettings();
        }

        private void ACManager_GameStateChanged(bool running, ACSharedMemory.ACManager manager)
        {
            if (!running)
            {
                pluginManager.AddEvent("GameStopped", typeof(DataCorePlugin));
            }
            else
            {
                pluginManager.AddEvent("GameStarted", typeof(DataCorePlugin));
            }
        }

        private void ACManager_GameStatusChanged(ACSharedMemory.AC_STATUS status, ACSharedMemory.ACManager manager)
        {
            pluginManager.AddEvent("SessionStatusChanged", typeof(DataCorePlugin));
        }

        private void ACManager_SessionRestart(ACSharedMemory.ACManager manager)
        {
            pluginManager.AddEvent("SessionRestarted", typeof(DataCorePlugin));
        }

        private void ACManager_NewLap(int completedLapNumber, bool testLap, ACSharedMemory.ACManager manager)
        {
            pluginManager.TriggerEvent("NewLap", typeof(DataCorePlugin));
        }

        private void DeclareType(PluginManager pluginManager, Type type, string currentName)
        {
            //
            if (type == typeof(string) || type.IsEnum || type.IsPrimitive || type.IsArray || type.Name == "List`1")
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

        private void UpdateData(PluginManager pluginManager, string currentName, object value)
        {
            if (value == null)
                return;

            var type = value.GetType();

            if (type == typeof(string) || type.IsEnum || type.IsPrimitive || type.IsArray || type.Name == "List`1")
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

        private void LoadSettings()
        {
            this.settings = JsonExtensions.FromJsonFile<DataCorePluginSettings>(SettingsPath);

            if (this.settings == null)
                settings = new DataCorePluginSettings();
            DeclareProperties();
        }

        public void DeclareProperties()
        {
            pluginManager.ClearProperties(typeof(DataCorePlugin));
            DeclareType(pluginManager, typeof(ACSharedMemory.GameData), "GameData");
            foreach (var expr in settings.Expressions)
            {
                pluginManager.AddProperty(string.Concat("CustomExpression.", expr.Name), typeof(DataCorePlugin), typeof(object));
            }
        }

        public void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);
            DeclareProperties();
        }

        DataCorePluginSettings settings = new DataCorePluginSettings();

        public DataCorePluginSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        DataCorePluginSettingsControl settingsControl = null;
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new DataCorePluginSettingsControl(this, pluginManager);
            }
            return settingsControl;
        }

        public void End(PluginManager pluginManager)
        {
        }
    }
}