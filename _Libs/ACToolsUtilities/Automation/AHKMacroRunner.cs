using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using WindowsInput;

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
            timer.Interval = 2;
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
            lock (timer)
            {
                try
                {
                    joystickManager.ReadState();
                    lastInputs = joystickManager.ButtonPressed();
                    RunMacros();

                    if (lastInputs.Contains("J0B00"))
                    {
                        (new InputSimulator()).Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
                        (new InputSimulator()).Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_T);
                    }

                    if (joystickManager.ButtonReleased().Contains("J0B00"))
                    {
                        (new InputSimulator()).Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_T);
                        (new InputSimulator()).Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
                    }

                    if (lastInputs.Contains("J0B15"))
                    {
                        (new InputSimulator()).Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.CONTROL);
                        (new InputSimulator()).Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_O);
                    }

                    if (joystickManager.ButtonReleased().Contains("J0B15"))
                    {
                        (new InputSimulator()).Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_O);
                        (new InputSimulator()).Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.CONTROL);
                    }

                }
                catch { }
            }
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