using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace HandBrake
{
    class Program
    {
        static SerialPort sp;
        static void Main(string[] args)
        {
            sp = new SerialPort("Com4", 9600);

            //sp.DataReceived += sp_DataReceived;
            sp.Open();
            sp.ReadExisting();
            while (true)
            {
                var test = sp.ReadLine();
                if (test != "high\r") { continue; }
                while (test == "high\r")
                {
                    test = sp.ReadLine();
                }
                Console.WriteLine("press");
                keybd_event((byte)WindowsInput.Native.VirtualKeyCode.VK_B, 0x8f, 0, (UIntPtr)0); // Tab Press

                while (test == "low\r")
                {
                    test = sp.ReadLine();
                }

                keybd_event((byte)WindowsInput.Native.VirtualKeyCode.VK_B, 0x8f, KEYEVENTF_KEYUP, (UIntPtr)0); // Tab Release
                Console.WriteLine("release");

            }

            Console.ReadKey();
        }
        private static InputSimulator input = new InputSimulator();

        private static string lastsp = "low";
        static void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (sp.BytesToRead > 0)
            {
                var test = sp.ReadLine();
                if (test == "high\r" && lastsp != "high\r")
                {
                    //input.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_B);
                    keybd_event((byte)WindowsInput.Native.VirtualKeyCode.VK_B, 0x8f, 0, (UIntPtr)0); // Tab Press

                }

                while (test != "low\r")
                {
                    test = sp.ReadLine();
                }
                keybd_event((byte)WindowsInput.Native.VirtualKeyCode.VK_B, 0x8f, KEYEVENTF_KEYUP, (UIntPtr)0); // Tab Release
                //PressKey(VirtualKeyCode.VK_B, true);

                //if (test == "low\r" && lastsp != "low\r")
                //{
                //    PressKey(VirtualKeyCode.VK_B, true);
                //    // input.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_B);
                //}

                lastsp = test;
            }
        }
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        //public static void PressKey(VirtualKeyCode key, bool up)
        //{
        //    const int KEYEVENTF_EXTENDEDKEY = 0x1;
        //    const int KEYEVENTF_KEYUP = 0x2;
        //    if (up)
        //    {
        //        keybd_event((byte)key, 0xh2, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        //    }
        //    else
        //    {
        //        keybd_event((byte)key, 0x00, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
        //    }
        //}

        //void TestProc()
        //{
        //    PressKey(Keys.Tab, false);
        //    Thread.Sleep(1000);
        //    PressKey(Keys.Tab, true);
        //}
    }
}
