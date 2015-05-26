using ACSharedMemory;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ACHub.Plugins.DataPlugins.Shell
{
    internal class ShellPlugin : IDataPlugin
    {
        public string Name
        {
            get { return "Shell Plugin"; }
        }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void DataUpdate(PluginManager pluginManager, GameData data)
        {
        }

        public void End(PluginManager pluginManager)
        {
        }

        public Control GetSettingsControl(PluginManager pluginManager)
        {
            return null;
        }

        public void Init(PluginManager pluginManager)
        {
            if (!System.IO.Directory.Exists("ShellMacros"))
            {
                System.IO.Directory.CreateDirectory("ShellMacros");
            }
            if (System.IO.Directory.Exists("ShellMacros"))
            {
                foreach (var file in System.IO.Directory.GetFiles("ShellMacros"))
                {
                    pluginManager.AddAction("Run_" + Path.GetFileName(file), typeof(ShellPlugin), (a, b) =>
                    {
                        try
                        {
                            if(a.Status.GameRunning)
                            Process.Start(file);
                        }
                        catch { }
                    });
                }
            }
        }
    }
}