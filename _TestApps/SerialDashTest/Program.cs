using System;
using System.Drawing;
using System.IO.Ports;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static int delta = 0;

        private static void Main(string[] args)
        {
            var ports = SerialPort.GetPortNames();
            var dash = new SerialDash.SerialDashController("auto");

            dash.SetIntensity(0);
            dash.SetIntensity(1, 7);
            //dash.SetInvertedScreen(0, true);

            dash.SetInvertedLedColors(0, true);

            dash.SetInvertedLedColors(1, true);

            //dash.EnableRGBLeds = true;

            dash.SetText(0, "  DASH  ");
            dash.SetText(1, "- - - - ");

            while (1 == 1)
            {
                dash.SetLedsColor(0, SerialDash.LedColor.Red);
                dash.SetLedsColor(1, SerialDash.LedColor.Red);

                dash.SetRGBLedColor(Color.Red);
                //dash.SetRGBLedColor(Color.FromArgb(10, 0, 0));

                dash.SetText(1, DateTime.Now.Millisecond.ToString());
                dash.SetText(0, "--------");
                //Thread.Sleep(10);
                dash.Send();

                //for (double i = 0; i < 16; i += 1)
                //{
                //    Color c = HSL2RGB(i / 16d, 0.5, 0.5).ToColor(0.05);
                //    dash.SetRGBLedColor((int)i + (delta% 32)/2, c);
                //    //do something with the color
                //}
                delta++;

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
        }

        public struct ColorRGB
        {
            public byte R;
            public byte G;
            public byte B;

            public ColorRGB(Color value)
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }

            public static implicit operator Color(ColorRGB rgb)
            {
                Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
                return c;
            }

            public Color ToColor(double intensity)
            {
                Color c = Color.FromArgb((int)(R * intensity), (int)(G * intensity), (int)(B * intensity));
                return c;
            }

            public static explicit operator ColorRGB(Color c)
            {
                return new ColorRGB(c);
            }
        }

        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }
    }
}