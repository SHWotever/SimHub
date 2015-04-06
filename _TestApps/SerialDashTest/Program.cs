using System;
using System.IO.Ports;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ports = SerialPort.GetPortNames();
            var dash = new SerialDash.SerialDash("auto");

            dash.SetIntensity(0);
            dash.SetIntensity(1, 7);
            dash.SetInvertedScreen(0, true);

            dash.SetInvertedLedColors(0, true);

            dash.SetInvertedLedColors(1, true);

            dash.SetText(0, "  DASH  ");
            dash.SetText(1, "- - - - ");

            int led = 0;
            while (1 == 1)
            {
                dash.SetLedsColor(0, SerialDash.LedColor.Green);
                dash.SetLedsColor(1, SerialDash.LedColor.Green);

                dash.SetText(1, DateTime.Now.Millisecond.ToString());

                //Thread.Sleep(50);
                dash.Send();

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.PageUp:
                            dash.IncrementIntensity();
                            break;

                        case ConsoleKey.PageDown:
                            dash.DecrementIntensity();
                            break;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}