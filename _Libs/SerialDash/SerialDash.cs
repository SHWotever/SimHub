using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SerialDash
{
    [Flags]
    public enum Buttons
    {
        ButtonNone = 0,
        Button1 = 1,
        Button2 = 2,
        Button3 = 4,
        Button4 = 8,
        Button5 = 16,
        Button6 = 32,
        Button7 = 64,
        Button8 = 128,
    }

    public enum LedColor
    {
        Green,
        Red,
        None
    }

    public class SerialDash
    {
        private const byte PROTOCOL_COMMAND_HELLO = (byte)'1';
        private const byte PROTOCOL_COMMAND_MODULECOUNT = (byte)'2';
        private const byte PROTOCOL_COMMAND_SENDDISPLAY = (byte)'3';
        private const int MAXSUPPORTED_SCREENS = 4;

        private List<int> ButtonsState;
        private SerialPort ComPort;
        private List<List<LedColor>> DashLeds;
        private List<string> DashStrings;
        private List<int> Intensity;
        private List<bool> InvertedLedColor;
        private List<bool> InvertedScreen;
        private int ModuleCount;
        private string SelectedComPort;
        private int baudRate;

        public int GetModuleCount()
        {
            return ModuleCount;
        }

        private string truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public SerialDash(string comPort, int baudRate = 19200)
        {
            this.baudRate = baudRate;
            SelectedComPort = comPort;
            this.ModuleCount = MAXSUPPORTED_SCREENS;

            DetectComPort(SelectedComPort, baudRate);

            this.DashStrings = new List<string>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.DashStrings.Add("");
            }

            this.ButtonsState = new List<int>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.ButtonsState.Add(0);
            }

            this.DashLeds = new List<List<LedColor>>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.DashLeds.Add(new List<LedColor>(
                    new LedColor[] { LedColor.None, LedColor.None, LedColor.None, LedColor.None, LedColor.None, LedColor.None, LedColor.None, LedColor.None })
                );
            }

            this.InvertedScreen = new List<bool>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.InvertedScreen.Add(false);
            }

            this.InvertedLedColor = new List<bool>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.InvertedLedColor.Add(false);
            }

            this.Intensity = new List<int>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.Intensity.Add(7);
            }
        }

        public void AppendFormat(int screenIndex, int lenght, object data, bool rightToLeft, string overflowText = null)
        {
            this.AppendText(screenIndex, Format(lenght, data.ToString(), rightToLeft, overflowText));
        }

        public void AppendText(int screenIndex, string text)
        {
            this.DashStrings[screenIndex] += text;
        }

        public void DecrementIntensity()
        {
            for (int i = 0; i < ModuleCount; i++)
            {
                this.Intensity[i] = Math.Max(0, this.Intensity[i] - 1);
            }
        }

        public string Format(int lenght, object data, bool rightToLeft, string overflowText = null)
        {
            return Format(lenght, data.ToString(), rightToLeft, overflowText);
        }

        public string Format(int lenght, string text, bool rightToLeft, string overflowText = null)
        {
            int realLength = text.Count(i => i != '.');
            if (realLength > lenght)
            {
                if (overflowText != null)
                {
                    return Format(lenght, overflowText, rightToLeft);
                }
                else
                {
                    return text.Substring(0, lenght);
                }
            }
            else
            {
                int missingchars = lenght - realLength;
                if (rightToLeft)
                {
                    return new string(' ', missingchars) + text;
                }
                else
                {
                    return text + new string(' ', missingchars);
                }
            }
        }

        public Buttons GetButtonState(int screenIndex)
        {
            int result = this.ButtonsState[screenIndex];

            if (GetInvertedScreen(screenIndex))
            {
                result = ReverseBits(result, 8);
            }

            this.ButtonsState[screenIndex] = 0;
            return (Buttons)result;
        }

        public byte GetByteColor(int screenIndex, int ledNumber)
        {
            return GetLedColor(this.DashLeds[screenIndex][ledNumber], this.GetInvertedLedColors(screenIndex));
        }

        public byte[] getDataFromDefaultFont(string text, bool invert)
        {
            byte[] data = new byte[8];

            if (string.IsNullOrEmpty(text))
            {
                return data;
            }

            int textpos = 0;
            for (int i = 0; textpos < text.Length && i < 8; i++)
            {
                if (text[textpos] == '.')
                {
                    data[i - 1] = (byte)(data[i - 1] | 0x80);
                    i = i - 1;
                }
                else
                {
                    data[i] = Fonts.FONT_DEFAULT[(int)text[textpos] - 32];
                }
                textpos++;
            }
            if (invert)
            {
                return InvertTextData(data);
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Get if the led colors must be inverted
        /// </summary>
        /// <param name="screenIndex"></param>
        /// <returns></returns>
        public int GetIntensity(int screenIndex)
        {
            return this.Intensity[screenIndex];
        }

        /// <summary>
        /// Get if the led colors must be inverted
        /// </summary>
        /// <param name="screenIndex"></param>
        /// <returns></returns>
        public bool GetInvertedLedColors(int screenIndex)
        {
            return this.InvertedLedColor[screenIndex];
        }

        /// <summary>
        /// Get if the screen is upside down
        /// </summary>
        /// <param name="screenIndex"></param>
        /// <returns></returns>
        public bool GetInvertedScreen(int screenIndex)
        {
            return this.InvertedScreen[screenIndex];
        }

        public LedColor GetLedColor(int screenIndex, int ledNumber)
        {
            return this.DashLeds[screenIndex][ledNumber];
        }

        public string GetText(int screenIndex)
        {
            return this.DashStrings[screenIndex];
        }

        public void IncrementIntensity()
        {
            for (int i = 0; i < ModuleCount; i++)
            {
                this.Intensity[i] = Math.Min(7, this.Intensity[i] + 1);
            }
        }

        public void Send()
        {
            if (ComPort == null)
            {
                DetectComPort(SelectedComPort, baudRate);
            }

            if (ComPort == null)
            {
                return;
            }

            if (!ComPort.IsOpen)
            {
                try
                {
                    ComPort.Open();
                }
                catch
                {
                    ComPort.Close();
                    DetectComPort(SelectedComPort, baudRate);
                    return;
                }
            }

            List<byte> bytes = new List<byte>();

            bytes.Add(PROTOCOL_COMMAND_SENDDISPLAY);
            for (int screenIndex = 0; screenIndex < ModuleCount; screenIndex++)
            {
                // INTENSITY
                bytes.Add((byte)GetIntensity(screenIndex));

                // TEXT
                var data = getDataFromDefaultFont(GetText(screenIndex), GetInvertedScreen(screenIndex));
                bytes.AddRange(data.Take(8));

                // LEDS
                if (!GetInvertedScreen(screenIndex))
                    for (int i = 0; i < 8; i++)
                        bytes.Add(GetByteColor(screenIndex, i));
                else
                    for (int i = 0; i < 8; i++)
                        bytes.Add(GetByteColor(screenIndex, 7 - i));
            }
            try
            {
                var buffer = bytes.ToArray();
                ComPort.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                this.ComPort = null;
            }
        }

        public void SetIntensity(int screenIndex, int intensity)
        {
            this.Intensity[screenIndex] = intensity;
        }

        public void SetIntensity(int intensity)
        {
            for (int i = 0; i < ModuleCount; i++)
            {
                this.Intensity[i] = intensity;
            }
        }

        /// <summary>
        /// Det if the led colors must be inverted
        /// </summary>
        /// <param name="screenIndex"></param>
        /// <param name="inverted"></param>
        public void SetInvertedLedColors(int screenIndex, bool inverted)
        {
            this.InvertedLedColor[screenIndex] = inverted;
        }

        /// <summary>
        /// Set if the screen is upside down
        /// </summary>
        /// <param name="screenIndex"></param>
        /// <param name="inverted"></param>
        public void SetInvertedScreen(int screenIndex, bool inverted)
        {
            this.InvertedScreen[screenIndex] = inverted;
        }

        public void SetLedColor(int screenIndex, int ledNumber, LedColor color)
        {
            this.DashLeds[screenIndex][ledNumber] = color;
        }

        public void SetLedsColor(int screenIndex, LedColor color)
        {
            for (int i = 0; i < this.DashLeds[screenIndex].Count; i++)
            {
                this.DashLeds[screenIndex][i] = color;
            }
        }

        public void SetText(int screenIndex, string text)
        {
            this.DashStrings[screenIndex] = text;
        }

        //[DebuggerNonUserCode]
        private Task DetectComPortTask;

        private void DetectComPort(string comPortName, int baudRate)
        {
            if (DetectComPortTask != null && DetectComPortTask.Status == TaskStatus.Running)
            {
                return;
            }
            DetectComPortTask = Task.Factory.StartNew(async delegate
            {
                if (ComPort != null && ComPort.IsOpen)
                {
                    ComPort.Close();
                    this.ComPort = null;
                }
                if (comPortName == "auto")
                {
                    foreach (var port in SerialPort.GetPortNames().Reverse())
                    {
                        TestSerialPort(port, baudRate);
                        if (this.ComPort != null) return;
                    }
                }
                else
                {
                    TestSerialPort(comPortName, baudRate);
                }
                await Task.Delay(5000);
            });
        }

        private bool TestSerialPort(string port, int baudRate)
        {
            //comPort = port;
            SerialPort com = new SerialPort(port, baudRate);
            //  return port;
            try
            {
                //open serial port
                com.DtrEnable = true;
                com.BaudRate = baudRate;
                com.WriteTimeout = 10;
                com.ReadTimeout = 500;

                com.Open();
                Thread.Sleep(2000);
                //com.ReadExisting();
                com.Write(new byte[] { PROTOCOL_COMMAND_HELLO }, 0, 1);

                char s = (char)com.ReadChar();
                if (s == 'a')
                {
                    ComPort = com;
                    com.WriteTimeout = 100;

                    com.Write(new byte[] { PROTOCOL_COMMAND_MODULECOUNT }, 0, 1);
                    this.ModuleCount = com.ReadByte() - 48;

                    this.ComPort.DataReceived += SerialPort_DataReceived;
                    this.ComPort.ReceivedBytesThreshold = 2;

                    return true;
                }
                else
                {
                    com.Close();
                }
            }
            catch
            {
                com.Close();
                com.Dispose();
            }
            return false;
        }

        private byte GetLedColor(LedColor color, bool inverted)
        {
            char value = 'N';
            if (color == LedColor.Green)
            {
                value = inverted ? 'R' : 'G';
            }
            else if (color == LedColor.Red)
            {
                value = inverted ? 'G' : 'R';
            }
            return (byte)value;
        }

        private byte InvertDataByte(byte data)
        {
            // Bit shifting Game !
            return (byte)(data & 0xC0 | (data & 0x07) << 3 | (data & 0x38) >> 3);
        }

        private byte[] InvertTextData(byte[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[result.Length - 1 - i] = InvertDataByte(data[i]);
            }

            // Move dots
            for (int i = 1; i < result.Length; i++)
            {
                if ((result[i] & 0x80) > 0)
                {
                    result[i - 1] = (byte)(result[i - 1] | 0x80);
                    result[i] = (byte)(result[i] ^ 0x80);
                }
            }

            return result;
        }

        private int ReverseBits(int value, int nbbytes)
        {
            int result = 0;
            for (int i = 0; i < nbbytes; i++)
            {
                result = result * 2;
                result = result + (value & 1);
                value = value / 2;
            }
            return result;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (ComPort.BytesToRead >= ModuleCount)
            {
                Thread.Sleep(1);
            }

            for (int i = 0; i < ModuleCount; i++)
            {
                this.ButtonsState[i] = ComPort.ReadByte();
            }
        }
    }
}