using ACSharedMemory;
using System;
using System.Collections.Generic;

namespace TimingClient.Plugins
{
    public class PluginManager
    {
        private ACManager manager;

        public ACManager ACManager
        {
            get { return manager; }
        }

        private List<IDataPlugin> dataPlugins = new List<IDataPlugin>();
        private List<IInputPlugin> inputPlugins = new List<IInputPlugin>();
        private List<IOutputPlugin> outputPlugins = new List<IOutputPlugin>();

        public PluginManager(ACManager manager)
        {
            this.manager = manager;
            manager.DataUpdated += manager_DataUpdated;

            this.dataPlugins.Add(new TimingClient.Plugins.DataPlugins.PersistantTrackerPlugin.PersistantTrackerPlugin());

            PluginAction(i => i.Init());

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
        }

        private Dictionary<string, KeyValuePair<Type, object>> GeneratedProperties = new Dictionary<string, KeyValuePair<Type, object>>(StringComparer.InvariantCultureIgnoreCase);
        private Dictionary<string, string> GeneratedEvents = new Dictionary<string, string>();
        private Dictionary<string, string> GeneratedInputs = new Dictionary<string, string>();

        public void AddEvent(string eventName, Type pluginType)
        {
            eventName = GetName(eventName, pluginType);
            if (!this.GeneratedEvents.ContainsKey(eventName))
            {
                this.GeneratedEvents.Add(eventName, eventName);
            }
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

        public void AddInput(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);
            if (!this.GeneratedInputs.ContainsKey(inputName))
            {
                this.GeneratedInputs.Add(inputName, inputName);
            }
        }

        public void TriggerInput(string inputName, Type pluginType)
        {
            inputName = GetName(inputName, pluginType);

            //foreach (var plugin in inputPlugins)
            //    if (plugin is IEventPlugin) { (plugin as IEventPlugin).EventTriggered(eventName); }

            //foreach (var plugin in dataPlugins)
            //    if (plugin is IEventPlugin) { (plugin as IEventPlugin).EventTriggered(eventName); }

            //foreach (var plugin in outputPlugins)
            //    if (plugin is IEventPlugin) { (plugin as IEventPlugin).EventTriggered(eventName); }

        }

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
                if (this.GeneratedProperties[name].Key == pluginType)
                {
                    this.GeneratedProperties[name] = new KeyValuePair<Type, object>(this.GeneratedProperties[name].Key, value);
                }
            }
        }

        private string GetName(string name, Type pluginType)
        {
            return string.Concat(name, ".", pluginType.Name);
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}