using ACSharedMemory;
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
        void DataUpdate(PluginManager pluginManager, GameData data);
    }

    public interface IInputPlugin : IPlugin
    {
    }

    public interface IEventPlugin : IPlugin
    {
        void EventTriggered(string eventName);

        void InputTriggered(string inputName);
    }

    public interface IDataPlugin : IPlugin
    {
        void DataUpdate(PluginManager pluginManager, GameData data);

    }
}