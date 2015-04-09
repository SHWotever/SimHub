using ACToolsUtilities.Serialisation;
using System;
using System.Windows.Forms;
namespace TimingClient.Plugins.OutputPlugins.Dash
{
    public class SerialDashPlugin : IOutputPlugin, IEventPlugin
    {

        private const string SettingsPath = "PluginsData\\SerialDashPlugin.json";

        private PluginManager pluginManager;

        public PluginManager PluginManager
        {
            get { return pluginManager; }
        }
        private global::SerialDash.SerialDashController dash = new global::SerialDash.SerialDashController("auto");

        public global::SerialDash.SerialDashController Dash
        {
            get { return dash; }
        }

        private SerialDashPluginSettings settings;

        public SerialDashPluginSettings Settings
        {
            get { return settings; }
        }
        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
        }

        public void ApplySettings()
        {
            this.dash.SetInvertedScreen(0, Settings.ReverseScreen0);
            this.dash.SetInvertedScreen(1, Settings.ReverseScreen1);
            this.dash.SetInvertedScreen(2, Settings.ReverseScreen2);
            this.dash.SetInvertedScreen(3, Settings.ReverseScreen3);

            SaveSettings();
        }

        public string Name
        {
            get { return "TM1638 Plugin"; }
        }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void Init(PluginManager pluginManager)
        {
            LoadSettings();

            this.pluginManager = pluginManager;

            this.dash.ButtonPressed += dash_ButtonPressed;

            CreateActions(pluginManager);

            for (int screen = 0; screen < global::SerialDash.SerialDashController.MAXSUPPORTED_SCREENS; screen++)
            {
                foreach (var item in Enum.GetValues(typeof(global::SerialDash.Buttons)))
                {
                    pluginManager.AddInput("SCREEN" + screen.ToString() + "_" + ((global::SerialDash.Buttons)item).ToString().ToUpper(), typeof(SerialDashPlugin));
                }
            }
        }

        private void CreateActions(PluginManager pluginManager)
        {
            pluginManager.AddAction("IncrementIntensity", typeof(SerialDashPlugin), (manager, actionname) => { IncrementIntensity(); });
            pluginManager.AddAction("DecrementIntensity", typeof(SerialDashPlugin), (manager, actionname) => { DecrementIntensity(); });
            pluginManager.AddAction("NextScreen", typeof(SerialDashPlugin), (manager, actionname) => { ScreenIndex++; });
            pluginManager.AddAction("PreviousScreen", typeof(SerialDashPlugin), (manager, actionname) => { ScreenIndex--; });
        }

        void dash_ButtonPressed(int screen, global::SerialDash.Buttons buttons)
        {
            PluginManager.TriggerInput("SCREEN" + screen.ToString() + "_" + buttons.ToString().ToUpper(), typeof(SerialDashPlugin));
        }

        private int ScreenIndex = 0;

        public void IncrementIntensity()
        {
            dash.IncrementIntensity();
        }

        public void DecrementIntensity()
        {
            dash.DecrementIntensity();
        }

        public void End(PluginManager pluginManager)
        {
        }

        SerialDashSettingsControl settingsControl = null;
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new SerialDashSettingsControl(this);
            }
            return settingsControl;
        }

        public void EventTriggered(string eventName)
        {
        }

        public void InputTriggered(string inputName)
        {
        }

        private void LoadSettings()
        {
            this.settings = JsonExtensions.FromJsonFile<SerialDashPluginSettings>(SettingsPath);

            if (this.settings == null)
                settings = new SerialDashPluginSettings();

            ApplySettings();
        }

        private void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);
        }
    }
}