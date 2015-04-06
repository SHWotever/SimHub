using ACSharedMemory;
using ACToolsUtilities;
using ACToolsUtilities.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TimingClient
{
    public class DashDisplay
    {
        private SerialDash.SerialDash dash;
        private JoystickManager JoystickManager = new JoystickManager();

        private Blinker RPMBlink = new Blinker();
        private Settings settings;
        private DateTime StaticDisplayEnd;

        private string StaticDisplayInitiator;

        public DashDisplay()
        {


            if (System.IO.File.Exists("DashDisplay.json"))
            {
                settings = Settings.FromFile("DashDisplay.json");
            }
            else
            {
                settings = new Settings();
            }
            if (settings == null)
            {
                settings = new Settings();
            }

            dash = new SerialDash.SerialDash("auto");
            dash.SetIntensity(0);
            dash.SetInvertedLedColors(0, true);
            dash.SetInvertedLedColors(1, true);
            dash.SetInvertedScreen(0, true);
            Reset();
        }

        ~DashDisplay()
        {
            Settings.ToFile("DashDisplay.json", settings);
        }

        public void DecrementBlinkLevel()
        {
            if (IsStaticDisplay("blink"))
            {
                settings.RpmBlinkLevel = Math.Max(settings.RpmBlinkLevel - 2, 0);
                StartStaticDisplay("blink", 1000, "RPML-", settings.RpmBlinkLevel.ToString() + " PER");
            }
        }

        public void IncrementBlinkLevel()
        {
            if (IsStaticDisplay("blink"))
            {
                // Loop for single button mapping ;)
                if (settings.RpmBlinkLevel == 100)
                {
                    settings.RpmBlinkLevel = 60;
                    return;
                }
                else
                {
                    settings.RpmBlinkLevel = Math.Min(settings.RpmBlinkLevel + 2, 100);
                }
            }
            StartStaticDisplay("blink", 1000, "RPML", settings.RpmBlinkLevel.ToString() + " PER");
        }

        public void Refresh(DataContainer data)
        {
            lock (this)
            {
                var dashButtons = dash.GetButtonState(0);
                JoystickManager.ReadState();

                if (dashButtons == SerialDash.Buttons.Button1 || TestKeyMapping(settings.TimeDeltaModeSwithInput))
                {
                    SwitchLapMode();
                }

                if (dashButtons == SerialDash.Buttons.Button2 || TestKeyMapping(settings.RpmBlinkLevelIncrementInput))
                {
                    IncrementBlinkLevel();
                }

                if (dashButtons == SerialDash.Buttons.Button3 || TestKeyMapping(settings.RpmBlinkLevelDecrementInput))
                {
                    DecrementBlinkLevel();
                }

                if (dashButtons == SerialDash.Buttons.Button4 || TestKeyMapping(settings.RpmDisplayModeSwithInput))
                {
                    SwitchRpmMode();
                }


                //if (TestKeyMapping(settings.RestartMacroInput))
                //{
                //    RestartRace();
                //}

                if (TestKeyMapping(settings.IncrementGameVolume))
                {
                    var process = Process.GetProcessesByName("acs").FirstOrDefault();
                    if (process != null)
                    {
                        float? volume = 0;
                        volume = VolumeMixer.GetApplicationVolume(process.Id);

                        if (IsStaticDisplay("gamevolume"))
                        {
                            volume = Math.Min(100, volume.GetValueOrDefault(0) + 10);
                            VolumeMixer.SetApplicationVolume(process.Id, volume.GetValueOrDefault(0));
                        }
                        StartStaticDisplay("gamevolume", 1000, "GAME VOL", "INC " + ((int)volume.GetValueOrDefault(0)).ToString());
                    }
                }
                if (TestKeyMapping(settings.DecrementGameVolume))
                {
                    var process = Process.GetProcessesByName("acs").FirstOrDefault();
                    if (process != null)
                    {
                        float? volume = 0;
                        volume = VolumeMixer.GetApplicationVolume(process.Id);

                        if (IsStaticDisplay("gamevolume"))
                        {
                            volume = Math.Max(0, volume.GetValueOrDefault(0) - 10);
                            VolumeMixer.SetApplicationVolume(process.Id, volume.GetValueOrDefault(0));
                        }
                        StartStaticDisplay("gamevolume", 1000, "GAME VOL", "DEC " + ((int)volume.GetValueOrDefault(0)).ToString());
                    }
                }
                if (TestKeyMapping(settings.IncrementGeneralVolume))
                {
                    VolumeMixer.VolumeUp(); VolumeMixer.VolumeUp();
                }
                if (TestKeyMapping(settings.DecrementGeneralVolume))
                {
                    VolumeMixer.VolumeDown(); VolumeMixer.VolumeDown();
                }

                if (IsStaticDisplay())
                {
                    return;
                }

                if (data.GameRunning)
                {
                    SetLapGearSpd(data);

                    if (settings.RpmDisplayMode == 0)
                    {
                        SetRpmLedMode1(data);
                    }
                    if (settings.RpmDisplayMode == 1)
                    {
                        SetRpmLedMode2(data);
                    }
                    if (settings.RpmDisplayMode == 2)
                    {
                        SetRpmLedMode3(data);
                    }
                }

                if (!data.GameRunning)
                {
                    dash.SetText(1, DateTime.Now.ToString("HH.mm.ss"));
                    dash.SetText(0, DateTime.Now.Millisecond.ToString());
                }

                dash.Send();
            }
        }

        public void Reset()
        {
            dash.SetText(0, "ARDUINO");
            dash.SetText(1, "DASH");
            dash.SetIntensity(7);
            dash.SetLedsColor(0, SerialDash.LedColor.None);
            dash.SetLedsColor(1, SerialDash.LedColor.None);
            dash.Send();
        }

        public void SwitchLapMode()
        {
            lock (this)
            {
                if (IsStaticDisplay("lapmode"))
                {
                    settings.TimeDeltaMode = (settings.TimeDeltaMode + 1) % 3;
                }

                switch (settings.TimeDeltaMode)
                {
                    case 0:
                        StartStaticDisplay("lapmode", 1000, "SESS", "BLAP");
                        break;

                    case 1:
                        StartStaticDisplay("lapmode", 1000, "MY", "BLAP");
                        break;

                    case 2:
                        StartStaticDisplay("lapmode", 1000, "ALLTIME", "BLAP");
                        break;
                }
            }
        }

        public void SwitchRpmMode()
        {
            lock (this)
            {
                if (IsStaticDisplay("rpmmode"))
                {
                    settings.RpmDisplayMode = (settings.RpmDisplayMode + 1) % 3;
                }
                switch (settings.RpmDisplayMode)
                {
                    case 0:
                        StartStaticDisplay("rpmmode", 1000, "RPM", "SINGLE");
                        break;

                    case 1:
                        StartStaticDisplay("rpmmode", 1000, "RPM", "DOUBLE");
                        break;

                    case 2:
                        StartStaticDisplay("rpmmode", 1000, "RPM", "JOYPOS");
                        break;
                }
            }
        }

        public bool TestKeyMapping(string mapping)
        {
            foreach (var str in (mapping ?? "").Split('|'))
            {
                if (JoystickManager.ButtonPressed().Contains(mapping))
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearScreen()
        {
            dash.SetText(0, "");
            dash.SetText(1, "");
            dash.SetLedsColor(0, SerialDash.LedColor.None);
            dash.SetLedsColor(1, SerialDash.LedColor.None);
        }

        private bool IsStaticDisplay(string initiator = null)
        {
            return DateTime.Now <= StaticDisplayEnd && (string.IsNullOrEmpty(initiator) || initiator == StaticDisplayInitiator);
        }



        private void SetLapGearSpd(DataContainer data)
        {
            // LAP / GEAR / SPD
            dash.SetText(0,
                dash.Format(2, data.Graphics.CompletedLaps + 1, false) +
                dash.Format(2, ACHelper.GetGear(data.Physics.Gear), true) +
                dash.Format(4, (int)Math.Round(data.Physics.SpeedKmh), true)
                );

            // CURRENT TIME - BL DELTA
            var TimeDelta = data.AllTimeDelta;
            if (settings.TimeDeltaMode == 0)
            {
                TimeDelta = data.SessionTimeDelta;
            }
            else if (settings.TimeDeltaMode == 1)
            {
                TimeDelta = data.MyTimeDelta;
            }
            else if (settings.TimeDeltaMode == 2)
            {
                TimeDelta = data.AllTimeDelta;
            }

            var text = dash.Format(4, TimeSpan.FromMilliseconds(data.Graphics.iCurrentTime).ToString(@"mm\.ss"), true);
            if (Math.Abs(TimeDelta.TotalSeconds) <= 99)
            {
                text += dash.Format(4, (int)TimeDelta.TotalSeconds + "." + (int)(Math.Abs(TimeDelta.Milliseconds) / 100), true);
            }
            else
            {
                text += dash.Format(4, (int)TimeDelta.TotalSeconds >= 0 ? "P" : "N", true);
            }
            dash.SetText(1, text);
        }

        private void SetRpmLedMode1(DataContainer data)
        {
            // RPM
            var rpm = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)16);
            dash.SetLedsColor(0, SerialDash.LedColor.None);
            dash.SetLedsColor(1, SerialDash.LedColor.None);

            RPMBlink.Started = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)100) >= settings.RpmBlinkLevel;

            bool blink = RPMBlink.Blink;
            for (int i = 0; i < 8 && i < rpm; i++)
            {
                dash.SetLedColor(1, i, blink ? SerialDash.LedColor.Red : SerialDash.LedColor.Green);
            }

            for (int i = 0; i < 5 && i + 8 < rpm; i++)
            {
                dash.SetLedColor(0, i, blink ? SerialDash.LedColor.Red : SerialDash.LedColor.Green);
            }

            for (int i = 5; i < 8 && i + 8 < rpm; i++)
            {
                dash.SetLedColor(0, i, SerialDash.LedColor.Red);
            }
        }

        private void SetRpmLedMode2(DataContainer data)
        {
            // RPM
            var rpm = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)64);

            dash.SetLedsColor(0, SerialDash.LedColor.None);
            dash.SetLedsColor(1, SerialDash.LedColor.None);

            RPMBlink.Started = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)100) >= settings.RpmBlinkLevel;

            bool blink = RPMBlink.Blink;

            for (int i = 0; i < 8 && i < rpm / 8; i++)
            {
                dash.SetLedColor(0, i, blink ? SerialDash.LedColor.Red : SerialDash.LedColor.Green);
            }

            dash.SetLedColor(1, (int)(rpm % 8), blink ? SerialDash.LedColor.Red : SerialDash.LedColor.Green);
        }

        private void SetRpmLedMode3(DataContainer data)
        {
            // RPM
            var rpm = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)64);

            dash.SetLedsColor(0, SerialDash.LedColor.None);
            dash.SetLedsColor(1, SerialDash.LedColor.None);

            RPMBlink.Started = Math.Round((double)Math.Max(data.Physics.Rpms, 0) / (double)data.StaticInfo.MaxRpm * (double)100) >= settings.RpmBlinkLevel;

            bool blink = RPMBlink.Blink;

            for (int i = 0; i < 8 && i < rpm / 8; i++)
            {
                dash.SetLedColor(0, i, blink ? SerialDash.LedColor.Red : SerialDash.LedColor.Green);
            }
            var x = data.Physics.SteerAngle;
            var pos = ((x + 1f) * 4f);
            var roundedpos = Math.Round(pos);
            int idx1 = (int)roundedpos;
            int idx2 = 0;
            if (roundedpos > pos)
            {
                idx2 = idx1 - 1;
            }
            else
            {
                idx2 = idx1 + 1;
            }

            dash.SetLedsColor(1, SerialDash.LedColor.Green);
            if (idx1 >= 0 && idx1 < 8)
                dash.SetLedColor(1, idx1, SerialDash.LedColor.Red);
        }

        private void StartStaticDisplay(string initiator, int milliseconds = 1000, string row1 = null, string row2 = null)
        {
            StaticDisplayInitiator = initiator;
            StaticDisplayEnd = DateTime.Now.AddMilliseconds(milliseconds);
            dash.SetText(0, row1);
            dash.SetText(1, row2);
            dash.Send();
        }
    }



    public class Settings
    {
        public Settings()
        {
            RpmBlinkLevel = 80;
            TimeDeltaModeSwithInput = "J0B6";
            RpmBlinkLevelIncrementInput = "J0B7";
            RpmBlinkLevelIncrementInput = "J0B3";
            RpmDisplayModeSwithInput = "J0B2";
            IncrementGameVolume = "J0HATUp";
            DecrementGameVolume = "J0HATDown";
            IncrementGeneralVolume = "J0HATRight";
            DecrementGeneralVolume = "J0HATLeft";
            RestartMacroInput = "J0B15";
        }

        public string DecrementGameVolume { get; set; }

        public string DecrementGeneralVolume { get; set; }

        public string IncrementGameVolume { get; set; }

        public string IncrementGeneralVolume { get; set; }

        public string RestartMacro { get; set; }

        public string RestartMacroInput { get; set; }

        public int RpmBlinkLevel { get; set; }

        public string RpmBlinkLevelDecrementInput { get; set; }

        public string RpmBlinkLevelIncrementInput { get; set; }

        public int RpmDisplayMode { get; set; }

        public string RpmDisplayModeSwithInput { get; set; }

        public int TimeDeltaMode { get; set; }

        public string TimeDeltaModeSwithInput { get; set; }
        public static Settings FromFile(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(json);
        }

        public static void ToFile(string path, Settings data)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }
    }
}