using ACSharedMemory;
using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading.Tasks;

namespace ACHub.Plugins
{
    /// <summary>
    /// Plugin manager (HUB)
    /// </summary>
    [Serializable]
    public class PluginManager
    {

        public class GeneratedAction
        {
            public Action<PluginManager, string> ActionStart { get; set; }
            public Action<PluginManager, string> ActionEnd { get; set; }

            public Task RepeatTask { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="running"></param>
        /// <param name="manager"></param>
        public delegate void GameRunningChangedDelegate(bool running, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event GameRunningChangedDelegate GameStateChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="manager"></param>
        public delegate void GameStatusChangedDelegate(AC_STATUS status, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event GameStatusChangedDelegate GameStatusChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="completedLapNumber"></param>
        /// <param name="testLap"></param>
        /// <param name="manager"></param>
        public delegate void NewLapDelegate(int completedLapNumber, bool testLap, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event NewLapDelegate NewLap;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionType"></param>
        /// <param name="manager"></param>
        public delegate void SessionTypeChangedDelegate(AC_SESSION_TYPE sessionType, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event SessionTypeChangedDelegate SessionTypeChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public delegate void SessionRestartDelegate(PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event SessionRestartDelegate SessionRestart;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newCar"></param>
        /// <param name="manager"></param>
        public delegate void CarChangedDelegate(CarDesc newCar, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event CarChangedDelegate CarChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTrack"></param>
        /// <param name="manager"></param>
        public delegate void TrackChangedDelegate(TrackDesc newTrack, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event TrackChangedDelegate TrackChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="manager"></param>
        public delegate void DataUpdatedDelegate(GameData data, PluginManager manager);
        /// <summary>
        /// 
        /// </summary>
        public event DataUpdatedDelegate DataUpdated;

        /// <summary>
        ///
        /// </summary>
        public readonly List<IDataPlugin> dataPlugins = new List<IDataPlugin>();

        /// <summary>
        ///
        /// </summary>
        public readonly List<IInputPlugin> inputPlugins = new List<IInputPlugin>();

        /// <summary>
        ///
        /// </summary>
        public readonly List<IOutputPlugin> outputPlugins = new List<IOutputPlugin>();

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, GeneratedAction> GeneratedActions = new Dictionary<string, GeneratedAction>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> GeneratedEvents = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> GeneratedInputs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, KeyValuePair<Type, object>> GeneratedProperties = new Dictionary<string, KeyValuePair<Type, object>>(StringComparer.InvariantCultureIgnoreCase);

        private const string SettingsPath = "PluginsData\\PluginManagerSettings.json";
        private ACManager _manager;
        private PluginManagerSettings settings = new PluginManagerSettings();

        /// <summary>
        /// AC Data
        /// </summary>
        public GameData Status
        {
            get { return _manager.Status; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="manager"></param>
        public PluginManager(ACManager manager)
        {

            this.ACManager = manager;
            this.settings = JsonExtensions.FromJsonFile<PluginManagerSettings>(SettingsPath);
            LoadSettings();

            this.dataPlugins.Add(new ACHub.Plugins.DataPlugins.DataCore.DataCorePlugin());
            this.dataPlugins.Add(new ACHub.Plugins.DataPlugins.PersistantTracker.PersistantTrackerPlugin());
            this.dataPlugins.Add(new ACHub.Plugins.DataPlugins.Shell.ShellPlugin());
            this.inputPlugins.Add(new ACHub.Plugins.InputPlugins.JoystickPlugin());
            this.outputPlugins.Add(new OutputPlugins.Dash.SerialDashPlugin());

            PluginAction(i => i.Init(this));


        }
        /// <summary>
        /// DTor
        /// </summary>
        ~PluginManager()
        {
            foreach (var plugin in this.inputPlugins)
            {
                plugin.End(this);
            }
            foreach (var plugin in this.dataPlugins)
            {
                plugin.End(this);
            }
            foreach (var plugin in this.outputPlugins)
            {
                plugin.End(this);
            }
            SaveSettings();
        }

        /// <summary>
        /// Input has been triggered delegate
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate bool InputTriggeredDelegate(string input);

        /// <summary>
        /// Input has been triggered
        /// </summary>
        public event InputTriggeredDelegate InputTriggered;

        /// <summary>
        /// Instance of the current ACManager (game reader)
        /// </summary>
        public ACManager ACManager
        {
            private get { return _manager; }
            set
            {

                value.GameStateChanged += manager_GameStateChanged;
                value.GameStatusChanged += manager_GameStatusChanged;
                value.NewLap += manager_NewLap;
                value.SessionTypeChanged += manager_SessionTypeChanged;
                value.SessionRestart += manager_SessionRestart;
                value.CarChanged += manager_CarChanged;
                value.TrackChanged += manager_TrackChanged;
                value.DataUpdated += manager_DataUpdated;
                _manager = value;
            }
        }

        void manager_TrackChanged(ACSharedMemory.Models.Track.TrackDesc newTrack, ACManager manager)
        {
            if (this.TrackChanged != null) { this.TrackChanged(newTrack, this); }
        }

        void manager_CarChanged(ACSharedMemory.Models.Car.CarDesc newCar, ACManager manager)
        {
            if (this.CarChanged != null) { this.CarChanged(newCar, this); }
        }

        void manager_SessionRestart(ACManager manager)
        {
            if (this.SessionRestart != null) { this.SessionRestart(this); }
        }

        void manager_SessionTypeChanged(AC_SESSION_TYPE sessionType, ACManager manager)
        {
            if (this.SessionTypeChanged != null) { this.SessionTypeChanged(sessionType, this); }
        }

        void manager_NewLap(int completedLapNumber, bool testLap, ACManager manager)
        {
            if (this.NewLap != null) { this.NewLap(completedLapNumber, testLap, this); }
        }

        void manager_GameStatusChanged(AC_STATUS status, ACManager manager)
        {
            if (this.GameStatusChanged != null) { this.GameStatusChanged(status, this); }
        }

        void manager_GameStateChanged(bool running, ACManager manager)
        {
            if (this.GameStateChanged != null) { this.GameStateChanged(running, this); }
        }

        /// <summary>
        /// Settings
        /// </summary>
        public PluginManagerSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// Declares a new callable action
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="pluginType"></param>
        /// <param name="actionStart"></param>
        /// <param name="actionEnd"></param>
        public void AddAction(string actionName, Type pluginType, Action<PluginManager, string> actionStart, Action<PluginManager, string> actionEnd = null)
        {
            actionName = GetName(actionName, pluginType);
            if (!this.GeneratedActions.ContainsKey(actionName))
            {
                this.GeneratedActions.Add(actionName, new GeneratedAction { ActionStart = actionStart, ActionEnd = actionEnd });
            }
        }

        /// <summary>
        /// Declare a new event
        /// </summary>
        /// <param name="eventName"> name of the event</param>
        /// <param name="pluginType">Type of the declaring plugin</param>
        public void AddEvent(string eventName, Type pluginType)
        {
            eventName = GetName(eventName, pluginType);
            if (!this.GeneratedEvents.ContainsKey(eventName))
            {
                this.GeneratedEvents.Add(eventName, eventName);
            }
        }

        /// <summary>
        /// Declare a new Input name (ie J0B1)
        /// </summary>
        /// <param name="inputName">Name of the input</param>
        /// <param name="pluginType">Type of the declaring plugin</param>
        public void AddInput(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);
            if (!this.GeneratedInputs.ContainsKey(inputName))
            {
                this.GeneratedInputs.Add(inputName, inputName);
            }
        }

        /// <summary>
        /// Declare a new property (data) wich will be accessible by other plugins
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="pluginType">Type of the declaring plugin</param>
        /// <param name="propertyType">Type of the property</param>
        public void AddProperty(string name, Type pluginType, Type propertyType)
        {
            name = GetName(name, pluginType);
            if (!this.GeneratedProperties.ContainsKey(name))
            {
                this.GeneratedProperties.Add(name, new KeyValuePair<Type, object>(propertyType, GetDefaultValue(propertyType)));
            }
        }

        /// <summary>
        /// Clear all declared actions for a plugin
        /// </summary>
        /// <param name="pluginType">Type of the declaring plugin</param>
        public void ClearActions(Type pluginType)
        {
            string actionName = GetName("", pluginType);
            var keys = GeneratedActions.Keys.Where(i => i.StartsWith(actionName)).ToList();
            foreach (var key in keys)
            {
                GeneratedActions.Remove(key);
            }
        }

        /// <summary>
        /// Clear all declared properties for a plugin
        /// </summary>
        /// <param name="pluginType">Type of the declaring plugin</param>
        public void ClearProperties(Type pluginType)
        {
            string propertyName = GetName("", pluginType);
            var keys = GeneratedProperties.Keys.Where(i => i.StartsWith(propertyName)).ToList();
            foreach (var key in keys)
            {
                GeneratedProperties.Remove(key);
            }
        }
        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pluginType"></param>
        /// <returns></returns>
        public object GetPropertyValue(string name, Type pluginType)
        {
            name = GetName(name, pluginType);
            if (this.GeneratedProperties.ContainsKey(name))
            {
                return this.GeneratedProperties[name].Value;
            }
            Logging.Current.WarnFormat("Property {0} not found", name);
            return null;
        }

        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetPropertyValue(string name)
        {
            if (this.GeneratedProperties.ContainsKey(name))
            {
                return this.GeneratedProperties[name].Value;
            }
            Logging.Current.WarnFormat("Property {0} not found", name);
            return null;
        }

        /// <summary>
        /// Save settings
        /// </summary>
        public void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);
        }

        /// <summary>
        /// Set current value for property
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pluginType"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(string name, Type pluginType, object value)
        {
            name = GetName(name, pluginType);
            if (this.GeneratedProperties.ContainsKey(name))
            {
                //if (value==null)
                {
                    this.GeneratedProperties[name] = new KeyValuePair<Type, object>(this.GeneratedProperties[name].Key, value);
                }
            }
        }
        /// <summary>
        /// Trigger input for all plugins
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="pluginType"></param>
        public void TriggerEvent(string eventName, Type pluginType)
        {
            eventName = GetName(eventName, pluginType);
            Logging.Current.InfoFormat("EventTriggered : {0}", eventName);

            PluginAction(i =>
            {
                if (i is IEventPlugin)
                {
                    (i as IEventPlugin).EventTriggered(eventName);
                }
            });

            foreach (var mapping in settings.EventActionMapping)
            {
                if (mapping.Trigger == eventName)
                {
                    GeneratedAction action;
                    if (this.GeneratedActions.TryGetValue(mapping.Target, out action))
                    {
                        if (action != null)
                        {
                            action.ActionStart(this, eventName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Trigger input
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="pluginType"></param>
        public void TriggerInput(string inputName, Type pluginType, PressType pressType)
        {
            inputName = GetName(inputName, pluginType);
            Logging.Current.DebugFormat("InputTriggered : {0}, {1}", inputName, pressType);

            if (InputTriggered != null)
            {
                var cancel = InputTriggered(inputName);
                if (cancel)
                    return;
            }

            PluginAction(i =>
            {
                if (i is IEventPlugin)
                {
                    (i as IEventPlugin).InputTriggered(inputName);
                }
            });


            foreach (var mapping in settings.InputActionMapping)
            {
                if (mapping.Trigger == inputName && mapping.PressType == pressType)
                {
                    GeneratedAction action;
                    if (this.GeneratedActions.TryGetValue(mapping.Target, out action))
                    {
                        Logging.Current.InfoFormat("Action '{0}' triggered after input: {0}, {1}", mapping.Target, inputName, pressType);
                        if (action != null)
                        {
                            action.ActionStart(this, inputName);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Trigger input
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="pluginType"></param>
        public void TriggerInputPress(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);
            Logging.Current.DebugFormat("InputPressed : {0}", inputName);

            foreach (var mapping in settings.InputActionMapping)
            {
                if (mapping.Trigger == inputName && mapping.PressType == PressType.During)
                {
                    GeneratedAction action;

                    if (this.GeneratedActions.TryGetValue(mapping.Target, out action))
                    {
                        lock (action)
                        {

                            Logging.Current.InfoFormat("Action start '{0}' triggered after input pressed : {0}, {1}", mapping.Target, inputName);
                            if (action != null)
                            {
                                action.ActionStart(this, inputName);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Trigger input
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="pluginType"></param>
        public void TriggerInputRelease(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);
            Logging.Current.DebugFormat("InputPressed : {0}", inputName);

            foreach (var mapping in settings.InputActionMapping)
            {
                if (mapping.Trigger == inputName && mapping.PressType == PressType.During)
                {
                    GeneratedAction action;
                    if (this.GeneratedActions.TryGetValue(mapping.Target, out action))
                    {
                        Logging.Current.InfoFormat("Action end '{0}' triggered after input released : {0}, {1}", mapping.Target, inputName);
                        if (action != null)
                        {
                            if (action.ActionEnd != null)
                            {
                                action.ActionEnd(this, inputName);
                            }
                        }
                    }
                }
            }
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        private string GetName(string name, Type pluginType)
        {
            return string.Concat(pluginType.Name, ".", name);
        }

        private void LoadSettings()
        {
            this.settings = JsonExtensions.FromJsonFile<PluginManagerSettings>(SettingsPath);

            if (this.settings == null)
                settings = new PluginManagerSettings();
        }

        private void manager_DataUpdated(GameData data, ACManager manager)
        {
            Dictionary<string, object> outputdata = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            PluginAction(i =>
            {
                if (i is IDataPlugin)
                {
                    (i as IDataPlugin).DataUpdate(this, data);
                }
            });

            PluginAction(i =>
            {
                if (i is IOutputPlugin)
                {
                    (i as IOutputPlugin).DataUpdate(this, data);
                }
            });

            if (this.DataUpdated != null) { DataUpdated(data, this); }
        }

        private void PluginAction(Action<IPlugin> action)
        {
            foreach (var plugin in this.inputPlugins)
            {
                action(plugin);
            }
            foreach (var plugin in this.dataPlugins)
            {
                action(plugin);
            }
            foreach (var plugin in this.outputPlugins)
            {
                action(plugin);
            }
        }
    }
}