using ACToolsUtilities;
using ACToolsUtilities.Input;
using System.Timers;

namespace ACHub.Plugins.InputPlugins
{
    /// <summary>
    /// Joystick input control
    /// </summary>
    public class JoystickPlugin : IInputPlugin
    {
        private JoystickManager joystickManager = new JoystickManager();
        private PluginManager pluginManager;
        private JoystickPluginSettingsControl settingsControl = null;
        private Timer timer;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return "Joystick Plugin"; }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
        }

        /// <summary>
        /// GetSettingsControl
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new JoystickPluginSettingsControl();
            }
            return settingsControl;
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
            this.timer = new Timer();

            this.timer.Interval = 2;
            this.timer.Elapsed += timer_Elapsed;
            this.timer.Enabled = true;
            var inputs = this.joystickManager.GetInputs();
            foreach (var input in inputs)
            {
                this.pluginManager.AddInput(input, typeof(JoystickPlugin));
            }

            im.LongPress += im_LongPress;
        }

        private void im_LongPress(string input)
        {
            pluginManager.TriggerInput(input, typeof(JoystickPlugin), PressType.LongPress);
            settingsControl.Refresh(input + " long press");
        }

        private InputManager im = new InputManager();

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            joystickManager.ReadState();
            var inputs = joystickManager.GetState();
            //foreach (var input in inputs)
            //{
            //    this.pluginManager.TriggerInput(input, typeof(JoystickPlugin),0);
            //}

            im.SetCurrentInputs(inputs);

            foreach (var i in im.GetShortInputs())
            {
                pluginManager.TriggerInput(i, typeof(JoystickPlugin), PressType.ShortPress);
                settingsControl.Refresh(i + " short pressed");
            }

            foreach (var i in im.GetNewInputs())
            {
                pluginManager.TriggerInputPress(i, typeof(JoystickPlugin));
                settingsControl.Refresh(i + " pressed");
            }

            foreach (var i in im.GetReleasedInputs())
            {
                pluginManager.TriggerInputRelease(i, typeof(JoystickPlugin));
                settingsControl.Refresh(i + " released");
            }
            //if (settingsControl != null)
            //{
            //    settingsControl.Refresh(inputs);
            //}
        }
    }
}