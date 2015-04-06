using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace ACToolsUtilities.Automation
{
    public class AHKMacroRunner
    {
        private List<KeyValuePair<string, string>> Macros = new List<KeyValuePair<string, string>>();
        private JoystickManager joystickManager = new JoystickManager();
        private Timer timer = new Timer();

        public AHKMacroRunner(bool enabled = false)
        {
            LoadMacros();
            timer.Interval = 5;
            timer.Elapsed += timer_Elapsed;
            this.Enabled = enabled;
        }

        public bool Enabled
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            joystickManager.ReadState();
            lastInputs = joystickManager.ButtonPressed();
            RunMacros();
        }

        private void LoadMacros()
        {
            if (!System.IO.Directory.Exists("Macros"))
            {
                System.IO.Directory.CreateDirectory("Macros");
            }

            this.Macros.Clear();
            foreach (var file in System.IO.Directory.GetFiles("Macros", "*.ahk"))
            {
                var keyMap = Path.GetFileNameWithoutExtension(file).Split(',')[0];
                Macros.Add(new KeyValuePair<string, string>(keyMap, file));
            }
        }

        private List<string> lastInputs;

        public bool TestKeyMapping(string mapping)
        {
            foreach (var str in (mapping ?? "").Split('|'))
            {
                if (lastInputs.Contains(mapping))
                {
                    return true;
                }
            }
            return false;
        }

        private void RunMacros()
        {
            foreach (var macro in Macros)
            {
                if (TestKeyMapping(macro.Key))
                {
                    Task.Factory.StartNew(() =>
                    {
                        Process.Start(macro.Value);
                    });
                }
            }
        }
    }
}