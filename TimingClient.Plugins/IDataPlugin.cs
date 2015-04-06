using ACSharedMemory;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TimingClient.Plugins
{
    public interface IPlugin
    {
        string Name { get; }

        string Version { get; }

        void Init(PluginManager pluginManager);

        void End(PluginManager pluginManager);

        Control GetSettingsControl(PluginManager pluginManager);
    }

    public interface IOutputPlugin : IPlugin
    {
    }

    public interface IInputPlugin : IPlugin
    {
    }

    public interface IEventPlugin : IPlugin
    {
        void EventTriggered(string eventName);
    }

    public interface IDataPlugin : IPlugin
    {
        void DataUpdate(PluginManager pluginManager, GameData data);

        void DoAction(PluginManager pluginManager, string command);
    }
}