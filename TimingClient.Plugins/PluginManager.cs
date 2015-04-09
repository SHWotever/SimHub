using ACSharedMemory;
using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimingClient.Plugins
{
    public class PluginManager
    {
        public readonly List<IDataPlugin> dataPlugins = new List<IDataPlugin>();
        public readonly List<IInputPlugin> inputPlugins = new List<IInputPlugin>();
        public readonly List<IOutputPlugin> outputPlugins = new List<IOutputPlugin>();
        public Dictionary<string, Action<PluginManager, string>> GeneratedActions = new Dictionary<string, Action<PluginManager, string>>();
        public Dictionary<string, string> GeneratedEvents = new Dictionary<string, string>();
        public Dictionary<string, string> GeneratedInputs = new Dictionary<string, string>();
        public Dictionary<string, KeyValuePair<Type, object>> GeneratedProperties = new Dictionary<string, KeyValuePair<Type, object>>(StringComparer.InvariantCultureIgnoreCase);
        private const string SettingsPath = "PluginsData\\PluginManagerSettings.json";
        private ACManager manager;
        private PluginManagerSettings settings = new PluginManagerSettings();

        public PluginManager(ACManager manager)
        {
            this.manager = manager;
            manager.DataUpdated += manager_DataUpdated;

            this.dataPlugins.Add(new TimingClient.Plugins.DataPlugins.DataCore.DataCorePlugin());
            this.dataPlugins.Add(new TimingClient.Plugins.DataPlugins.PersistantTracker.PersistantTrackerPlugin());
            this.inputPlugins.Add(new TimingClient.Plugins.InputPlugins.JoystickPlugin());
            this.outputPlugins.Add(new OutputPlugins.Dash.SerialDashPlugin());

            PluginAction(i => i.Init(this));

            LoadSettings();
        }

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

        public delegate bool InputTriggeredDelegate(string input);

        public event InputTriggeredDelegate InputTriggered;

        /// <summary>
        /// Instance of the current ACManager (game reader)
        /// </summary>
        public ACManager ACManager
        {
            get { return manager; }
        }

        public PluginManagerSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// Declare à new callable action
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        /// <param name="pluginType">Type of the declaring plugin</param>
        /// <param name="action">Delegate of the action method</param>
        public void AddAction(string actionName, Type pluginType, Action<PluginManager, string> action)
        {
            actionName = GetName(actionName, pluginType);
            if (!this.GeneratedEvents.ContainsKey(actionName))
            {
                this.GeneratedActions.Add(actionName, action);
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

        public object GetPropertyValue(string name, Type pluginType)
        {
            name = GetName(name, pluginType);
            if (this.GeneratedProperties.ContainsKey(name))
            {
                return this.GeneratedProperties[name].Value;
            }
            return null;
        }

        public object GetPropertyValue(string name)
        {
            if (this.GeneratedProperties.ContainsKey(name))
            {
                return this.GeneratedProperties[name].Value;
            }
            return null;
        }

        public void TriggerEvent(string eventName, Type pluginType)
        {
            eventName = GetName(eventName, pluginType);
            PluginAction(i =>
            {
                if (i is IEventPlugin)
                {
                    (i as IEventPlugin).EventTriggered(eventName);
                }
            });
        }

        public void TriggerInput(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);

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
                if (mapping.Trigger == inputName)
                {
                    Action<PluginManager, string> action;
                    if (this.GeneratedActions.TryGetValue(mapping.Target, out action))
                    {
                        if (action != null)
                        {
                            action(this, inputName);
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

        private void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);
        }
    }
}