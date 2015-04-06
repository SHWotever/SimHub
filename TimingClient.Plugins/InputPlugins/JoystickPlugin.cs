using ACToolsUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TimingClient.Plugins.InputPlugins
{
    public class JoystickPlugin : IInputPlugin
    {
        Timer timer;

        private PluginManager pluginManager;
        private JoystickManager joystickManager = new JoystickManager();

        public string Name
        {
            get { return "Joystick Plugin"; }
        }

        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        public void Init(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
            this.timer = new Timer();
            this.timer.SynchronizingObject = pluginManager.ACManager.SynchronizingObject;
            this.timer.Interval = 10;
            this.timer.Elapsed += timer_Elapsed;

            var inputs = this.joystickManager.GetInputs();
            foreach (var input in inputs)
            {
                this.pluginManager.AddInput(input, typeof(JoystickPlugin));
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            joystickManager.ReadState();
            var inputs = joystickManager.ButtonPressed();
            foreach (var input in inputs)
            {
                this.pluginManager.TriggerInput(input, typeof(JoystickPlugin));
            }
        }

        public void End(PluginManager pluginManager)
        {
        }


        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            return null;
        }
    }
}
