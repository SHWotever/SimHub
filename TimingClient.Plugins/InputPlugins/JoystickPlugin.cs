using ACToolsUtilities;
using System.Timers;

namespace TimingClient.Plugins.InputPlugins
{
    public class JoystickPlugin : IInputPlugin
    {
        private Timer timer;

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
            this.timer.Enabled = true;
            var inputs = this.joystickManager.GetInputs();
            foreach (var input in inputs)
            {
                this.pluginManager.AddInput(input, typeof(JoystickPlugin));
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            joystickManager.ReadState();
            var inputs = joystickManager.ButtonPressed();
            foreach (var input in inputs)
            {
                this.pluginManager.TriggerInput(input, typeof(JoystickPlugin));
            }
            if (settingsControl != null)
            {
                settingsControl.Refresh(inputs);
            }
        }

        public void End(PluginManager pluginManager)
        {
        }

        private JoystickPluginSettingsControl settingsControl = null;

        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new JoystickPluginSettingsControl();
            }
            return settingsControl;
        }

    }
}