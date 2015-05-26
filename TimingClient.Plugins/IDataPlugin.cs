using ACSharedMemory;
using System.Windows.Forms;

namespace ACHub.Plugins
{
    /// <summary>
    /// Base Plugin interface for output
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Plugin initialisation
        /// </summary>
        /// <param name="pluginManager"></param>
        void Init(PluginManager pluginManager);

        /// <summary>
        /// Plugin stop
        /// </summary>
        /// <param name="pluginManager"></param>
        void End(PluginManager pluginManager);

        /// <summary>
        /// Returns setting control
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        Control GetSettingsControl(PluginManager pluginManager);
    }

    /// <summary>
    /// Plugin interface for output
    /// </summary>
    public interface IOutputPlugin : IPlugin
    {
        /// <summary>
        /// Date update frm  plugin mamanger
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data"></param>
        void DataUpdate(PluginManager pluginManager, GameData data);
    }

    /// <summary>
    /// Plugin interface
    /// </summary>
    public interface IInputPlugin : IPlugin
    {
    }

    /// <summary>
    /// Plugin interface for input or events
    /// </summary>
    public interface IEventPlugin : IPlugin
    {
        /// <summary>
        /// Called when an event is triggered
        /// </summary>
        /// <param name="eventName"></param>
        void EventTriggered(string eventName);

        /// <summary>
        /// Called when an input is triggered
        /// </summary>
        /// <param name="inputName"></param>
        void InputTriggered(string inputName);
    }

    /// <summary>
    /// Plugion interface for data events
    /// </summary>
    public interface IDataPlugin : IPlugin
    {
        /// <summary>
        /// Called when date are updated
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data"></param>
        void DataUpdate(PluginManager pluginManager, GameData data);
    }
}