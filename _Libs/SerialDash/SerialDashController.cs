using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SerialDash
{
    public class ModuleButton
    {
        public int Screen { get; set; }
        public Buttons PressedButton { get; set; }

    }

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

    class CRC8 : List<byte>
    {

        public List<Byte> getDataWithCrc()
        {
            var result = new List<byte>();
            result.AddRange(this);
            result.Add(ComputeAdditionChecksum(this));
            this.Clear();
            return result;

        }

        public Byte getDataCrc()
        {
            var res = ComputeAdditionChecksum(this);
            this.Clear();

            return res;
        }

        private static byte ComputeAdditionChecksum(IEnumerable<byte> data)
        {
            byte sum = 0;
            unchecked // Let overflow occur without exceptions
            {
                foreach (byte b in data)
                {
                    sum += b;
                }
            }
            return sum;
        }

    }

    public class SerialDashController
    {
        public const int MAXSUPPORTED_SCREENS = 4;
        public const int MAXSUPPORTED_RGBLEDS = 64;

        private const byte PROTOCOL_COMMAND_HELLO = (byte)'1';
        private const byte PROTOCOL_COMMAND_MODULECOUNT = (byte)'2';
        private const byte PROTOCOL_COMMAND_RGBLEDCOUNT = (byte)'4';

        private const byte PROTOCOL_COMMAND_SENDDISPLAY = (byte)'3';

        private const byte PROTOCOL_COMMAND_SENDTEXT = (byte)'5';
        private const byte PROTOCOL_COMMAND_SENDRGB = (byte)'6';
        private const byte PROTOCOL_COMMAND_SETBAUDRATE = (byte)'8';

        private const char MODEL_A = 'a';
        private const char MODEL_B = 'b';

        private int baudRate;
        private List<int> ButtonsState;
        private SerialPort ComPort;
        private char connectedModel;
        private List<List<LedColor>> DashLeds;
        private List<string> DashStrings;

        //[DebuggerNonUserCode]
        private Task DetectComPortTask;

        private List<int> Intensity;
        private List<bool> InvertedLedColor;

        private List<Color> RGBLedColor;

        private List<bool> InvertedScreen;
        private int ModuleCount;
        private int RgbLedCount;
        private string SelectedComPort;

        public bool EnableRGBLeds { get; set; }

        public SerialDashController(string comPort, int baudRate = 19200)
        {
            this.baudRate = baudRate;
            SelectedComPort = comPort;
            this.ModuleCount = MAXSUPPORTED_SCREENS;

            DetectComPort(SelectedComPort, baudRate);

            this.DashStrings = new List<string>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.DashStrings.Add("---------");
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
                this.InvertedLedColor.Add(true);
            }

            this.RGBLedColor = new List<Color>();
            for (int i = 0; i < MAXSUPPORTED_RGBLEDS; i++)
            {
                this.RGBLedColor.Add(Color.Black);
            }

            this.Intensity = new List<int>();
            for (int i = 0; i < MAXSUPPORTED_SCREENS; i++)
            {
                this.Intensity.Add(7);
            }
        }

        public delegate void ButtonEventDelegate(int screen, Buttons buttons);

        public delegate void ButtonsChangedDelegate(List<ModuleButton> currentButtons);

        public event ButtonEventDelegate ButtonPressed;

        public event ButtonEventDelegate ButtonReleased;

        public event ButtonsChangedDelegate ButtonChanged;

        public char ConnectedModel
        {
            get { return connectedModel; }
        }

        public static string Format(int lenght, object data, bool rightToLeft, string overflowText = null)
        {
            return Format(lenght, data.ToString(), rightToLeft, overflowText);
        }

        public static string Format(int lenght, string text, bool rightToLeft, string overflowText = null)
        {
            text = ReplaceChars(text);
            int realLength = text.Count(i => i != '.');
            if (realLength > lenght)
            {
                if (overflowText != null)
                {
                    return Format(lenght, overflowText, rightToLeft);
                }
                else
                {
                    if (!rightToLeft)
                    {
                        while (text.Count(i => i != '.') > lenght)
                        {
                            text = text.Substring(text.Length - 1);
                        }
                    }
                    else
                    {
                        while (text.Count(i => i != '.') > lenght)
                        {
                            text = text.Substring(1);
                        }
                    }
                    return text;
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

        public static byte[] getDataFromDefaultFont(string text, bool invert)
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
                    if (i > 0)
                    {
                        data[i - 1] = (byte)(data[i - 1] | 0x80);
                        i = i - 1;
                    }
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

        public static string ReplaceChars(string text)
        {
            text = text ?? "";
            text = text.Replace(",", ".");
            text = text.Replace(":", ".");
            return text;
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

        public string FormatText(int lenght, object data, bool rightToLeft, string overflowText = null)
        {
            return SerialDashController.Format(lenght, data.ToString(), rightToLeft, overflowText);
        }

        public string FormatText(int lenght, string text, bool rightToLeft, string overflowText = null)
        {
            return SerialDashController.Format(lenght, text, rightToLeft, overflowText);
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

        public Color GetRGBLedColor(int ledNumber)
        {
            return this.RGBLedColor[ledNumber];
        }

        public int GetModuleCount()
        {
            return ModuleCount;
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
            lock (this)
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
                if (ConnectedModel == MODEL_A)
                {
                    Send_MODELA();
                }
                else if (ConnectedModel == MODEL_B)
                {
                    if (!EnableRGBLeds)
                    {
                        RemapRGB();
                    }
                    Send_MODELB();
                }
            }
        }

        private void RemapRGB()
        {
            int ENABLEDMODULES = 2;
            for (int screenIndex = 0; screenIndex < ModuleCount; screenIndex++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var state = GetLedColor(screenIndex, i);
                    int tmppos = (((screenIndex - ENABLEDMODULES + 1) * -1) * 8) + i;
                    int pos = 0;
                    if (tmppos < 3)
                        pos = 0;
                    else if (tmppos > 13)
                        pos = 16;
                    else
                        pos = (int)((tmppos - 3) / 10.0 * 16.0);

                    if (state == LedColor.Red || state == LedColor.Green)
                        SetRGBLedColor(15 - (screenIndex * 8 + (7 - i)), Color.FromArgb(pos, 0, 16 - pos));
                    else
                    {
                        SetRGBLedColor(15 - (screenIndex * 8 + (7 - i)), Color.FromArgb(0, 0, 0));
                    }
                }
            }

        }

        private void Send_MODELA()
        {
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

        private void Send_MODELB()
        {
            try
            {
                CRC8 crc = new CRC8();
                int currentPos = 0;

                List<byte> bytes = new List<byte>();

                // TEXT
                bytes.Add(PROTOCOL_COMMAND_SENDTEXT);
                for (int screenIndex = 0; screenIndex < ModuleCount; screenIndex++)
                {
                    // INTENSITY
                    crc.Add((byte)GetIntensity(screenIndex));

                    // TEXT
                    var data = getDataFromDefaultFont(GetText(screenIndex), GetInvertedScreen(screenIndex));
                    crc.AddRange(data.Take(8));
                    bytes.AddRange(crc.getDataWithCrc());

                    currentPos = Send(bytes, currentPos);
                }


                //Thread.Sleep(10);

                // LEDS
                bytes.Add(PROTOCOL_COMMAND_SENDRGB);
                foreach (var color in this.RGBLedColor.Take(16))
                {
                    crc.Add(color.R);
                    crc.Add(color.G);
                    crc.Add(color.B);

                    bytes.Add(color.R);
                    bytes.Add(color.G);
                    bytes.Add(color.B);

                    // SEND
                    currentPos = Send(bytes, currentPos);
                }
                bytes.Add(crc.getDataCrc());

                // SEND
                currentPos = Send(bytes, currentPos);
            }
            catch { }

        }

        private int Send(List<byte> bytes, int currentPos)
        {

            try
            {
                var buffer = bytes.ToArray();
                ComPort.Write(buffer, currentPos, buffer.Length - currentPos);
                return buffer.Length;
            }
            catch
            {
                this.ComPort = null;
                throw;
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

        public void SetLedsColor(LedColor color)
        {
            for (int i = 0; i < this.DashLeds.Count; i++)
            {
                for (int j = 0; j < DashLeds[i].Count; j++)
                {
                    this.DashLeds[i][j] = color;
                }
            }
        }

        public void SetRGBLedColor(int ledNumber, Color color)
        {
            this.RGBLedColor[ledNumber] = color;
        }

        public void SetRGBLedColor(Color color)
        {
            for (int i = 0; i < this.RGBLedColor.Count; i++)
            {
                this.RGBLedColor[i] = color;
            }
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

        public void SetText(string text)
        {
            for (int i = 0; i < this.DashStrings.Count; i++)
            {
                this.DashStrings[i] = text;
            }
        }

        private static byte InvertDataByte(byte data)
        {
            // Bit shifting Game !
            return (byte)(data & 0xC0 | (data & 0x07) << 3 | (data & 0x38) >> 3);
        }

        private static byte[] InvertTextData(byte[] data)
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

        private void DetectComPort(string comPortName, int baudRate)
        {
            if (DetectComPortTask != null)
            {
                return;
            }
            DetectComPortTask = Task.Factory.StartNew(async delegate
            {
                try
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
                }
                finally
                {
                    DetectComPortTask = null;
                }

            });
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
            //Debug.WriteLine(ComPort.ReadExisting());
            //return;

            while (ComPort.BytesToRead < ModuleCount)
            {
                Thread.Sleep(1);
            }
            List<ModuleButton> buttons = new List<ModuleButton>();

            for (int i = 0; i < ModuleCount; i++)
            {
                var oldvalue = this.ButtonsState[i];
                var newvalue = ComPort.ReadByte();

                this.ButtonsState[i] = newvalue;

                if (GetInvertedScreen(i))
                {
                    oldvalue = ReverseBits(oldvalue, 8);
                    newvalue = ReverseBits(newvalue, 8);
                }

                if (newvalue != oldvalue)
                {
                    foreach (var item in Enum.GetValues(typeof(Buttons)))
                    {
                        if (((Buttons)newvalue).HasFlag((Buttons)item))
                        {
                            buttons.Add(new ModuleButton { Screen = i, PressedButton = (Buttons)item });
                        }
                    }

                    if (ButtonChanged != null)
                    {
                        ButtonChanged(buttons);
                    }

                    if (ButtonPressed != null)
                    {
                        foreach (var item in Enum.GetValues(typeof(Buttons)))
                        {
                            if (!((Buttons)oldvalue).HasFlag((Buttons)item) && ((Buttons)newvalue).HasFlag((Buttons)item))
                            {
                                ButtonPressed(i, (Buttons)item);
                            }
                        }
                    }

                    if (ButtonReleased != null)
                    {
                        foreach (var item in Enum.GetValues(typeof(Buttons)))
                        {
                            if (((Buttons)oldvalue).HasFlag((Buttons)item) && !((Buttons)newvalue).HasFlag((Buttons)item))
                            {
                                ButtonReleased(i, (Buttons)item);
                            }
                        }
                    }
                }
            }
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
                if (s == 'a' || s == 'b')
                {

                    com.WriteTimeout = 100;

                    com.Write(new byte[] { PROTOCOL_COMMAND_MODULECOUNT }, 0, 1);
                    this.ModuleCount = com.ReadByte() - 48;

                    this.connectedModel = s;

                    if (ConnectedModel == MODEL_B)
                    {
                        com.Write(new byte[] { PROTOCOL_COMMAND_RGBLEDCOUNT }, 0, 1);
                        this.RgbLedCount = com.ReadByte();

                        //SetBaudRate(com, 11, 115200);
                        SetBaudRate(com, 13, 250000);
                        //SetBaudRate(com, 14, 1000000);
                        //SetBaudRate(com, 15, 2000000);
                    }

                    com.DataReceived += SerialPort_DataReceived;
                    com.ReceivedBytesThreshold = 2;

                    ComPort = com;

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

        private static void SetBaudRate(SerialPort com, int brCode, int brSpeed)
        {
            com.Write(new byte[] { PROTOCOL_COMMAND_SETBAUDRATE }, 0, 1);
            com.Write(new byte[] { (byte)brCode }, 0, 1);
            Thread.Sleep(500);
            com.DiscardInBuffer();
            com.DiscardOutBuffer();
            com.BaudRate = brSpeed;
            com.DiscardInBuffer();
            com.DiscardOutBuffer();
        }

        private string truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}