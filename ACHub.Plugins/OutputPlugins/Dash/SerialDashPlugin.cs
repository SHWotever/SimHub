using ACToolsUtilities.Input;
using ACToolsUtilities.Serialisation;
using ACToolsUtilities.UI;
using SerialDash;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Plugin for LM16XX
    /// </summary>
    public class SerialDashPlugin : IOutputPlugin, IEventPlugin
    {
        private Blinker RpmBlink = new Blinker();
        private Blinker FuelLapsBlink = new Blinker() { BlinkTime = 500 };
        private AlertManager FuelLapsAlertManager = new AlertManager { AlertRecurency = 60 };

        private Blinker ScreenBlinker = new Blinker();

        private const string SettingsPath = "PluginsData\\SerialDashPlugin.json";

        private global::SerialDash.SerialDashController dash = new global::SerialDash.SerialDashController("auto");
        private PluginManager pluginManager;

        private int ScreenIndex = 0;

        private int ScreenIndexIdle = 0;

        private SerialDashPluginSettings settings;

        private SerialDashSettingsControl settingsControl = null;

        private class StaticScreen
        {
            public Screen screen;
            public double duration;
            public DateTime DateEnd;
        }

        private Queue<StaticScreen> _StaticScreens = new Queue<StaticScreen>();

        private void EnqueueStaticScreen(Screen screen, double time)
        {
            lock (_StaticScreens)
            {
                if (_StaticScreens.Count > 0)
                {
                    if (_StaticScreens.Peek().screen == screen)
                    {
                        _StaticScreens.Peek().DateEnd = DateTime.Now.AddSeconds(time);
                        return;
                    }
                }
                _StaticScreens.Enqueue(new StaticScreen { duration = time, DateEnd = DateTime.MinValue, screen = screen });
            }
        }

        private void ClearStaticScreens()
        {
            _StaticScreens.Clear();
        }

        private Screen GetCurrentStaticScreen()
        {
            if (_StaticScreens.Count > 0)
            {
                var currentScreen = _StaticScreens.Peek();
                if (currentScreen.DateEnd != DateTime.MinValue && DateTime.Now > currentScreen.DateEnd)
                {
                    _StaticScreens.Dequeue();
                }
            }
            if (_StaticScreens.Count > 0)
            {
                var currentScreen = _StaticScreens.Peek();
                if (_StaticScreens.Count > 0 && currentScreen.DateEnd == DateTime.MinValue)
                {
                    currentScreen.DateEnd = DateTime.Now.AddSeconds(currentScreen.duration);
                }
            }

            if (_StaticScreens.Count > 0)
            {
                return _StaticScreens.Peek().screen;
            }
            return null;
        }

        private Screen staticScreen { get { return GetCurrentStaticScreen(); } }

        private DateTime staticScreenEnd;

        /// <summary>
        /// Dash controller
        /// </summary>
        public global::SerialDash.SerialDashController Dash
        {
            get { return dash; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Name
        {
            get { return "TM1638 Plugin"; }
        }

        /// <summary>
        /// PLugin manager
        /// </summary>
        public PluginManager PluginManager
        {
            get { return pluginManager; }
        }

        /// <summary>
        /// Current settings
        /// </summary>
        public SerialDashPluginSettings Settings
        {
            get { return settings; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }

        /// <summary>
        /// Apply settings
        /// </summary>
        public void ApplySettings()
        {
            this.dash.SetInvertedScreen(0, Settings.ReverseScreen0);
            this.dash.SetInvertedScreen(1, Settings.ReverseScreen1);
            this.dash.SetInvertedScreen(2, Settings.ReverseScreen2);
            this.dash.SetInvertedScreen(3, Settings.ReverseScreen3);
            this.dash.SetIntensity(settings.Intensity);
            this.FuelLapsAlertManager.AlertRecurency = Math.Max(1, (int)settings.LowFuelLapsAlertInterval);
            CreateActions(pluginManager);
            SaveSettings();
        }

        /// <summary>
        /// Called when data is updating
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data"></param>
        public void DataUpdate(PluginManager pluginManager, ACSharedMemory.GameData data)
        {
            lock (settings)
            {
                if (data.GameRunning)
                {
                    var fuelLapsRemaining = Convert.ToDouble(pluginManager.GetPropertyValue("DataCorePlugin.Computed.Fuel_RemainingLaps") ?? "-1");
                    FuelLapsAlertManager.Started = fuelLapsRemaining <= settings.LowFuelLapsLevel;

                    if (FuelLapsAlertManager.Elapsed && fuelLapsRemaining != -1 && data.NewData.Graphics.IsInPit == 0)
                    {
                        FuelLapsAlertManager.Reset();
                        pluginManager.TriggerEvent("LowFuelLapAlert", typeof(SerialDashPlugin));
                    }
                }

                if (data.GameRunning)
                {
                    var RunningScreens = this.settings.Screens.Where(i => i.GameRunningScreen).ToList();
                    ScreenIndex = DisplayScreen(pluginManager, RunningScreens, ScreenIndex);
                }
                else
                {
                    var NotRunningScreens = this.settings.Screens.Where(i => i.GameNotRunningScreen).ToList();
                    ScreenIndexIdle = DisplayScreen(pluginManager, NotRunningScreens, ScreenIndexIdle);
                }

                var currentStaticScreen = GetCurrentStaticScreen();

                if (currentStaticScreen != null)
                {
                    DisplayScreen(pluginManager, currentStaticScreen);
                }
                else
                {
                    currentStaticScreen = null;
                }

                for (int i = 0; i < 4; i++)
                {
                    dash.SetLedsColor(i, SerialDash.LedColor.None);
                }

                if (data.GameRunning)
                {
                    double RPM = (double)data.NewData.Physics.Rpms;
                    double MaxRPM = data.NewData.StaticInfo.MaxRpm;
                    if (MaxRPM > 0)
                    {
                        var currentRpm = Math.Min(100.0, RPM / MaxRPM * 100);
                        RpmBlink.Started = currentRpm >= (double)this.settings.RpmBlinkingLevel;

                        if (settings.RPMStartOffset > 0)
                        {
                            RPM = RPM - (settings.RPMStartOffset / 100.0 * MaxRPM);
                            MaxRPM = ((100.0 - settings.RPMStartOffset) / 100.0) * MaxRPM;
                        }

                        currentRpm = Math.Min(100.0, RPM / MaxRPM * 100);

                        var fuelLapsRemaining = Convert.ToDouble(pluginManager.GetPropertyValue("DataCorePlugin.Computed.Fuel_RemainingLaps") ?? "0");
                        FuelLapsBlink.Started = fuelLapsRemaining <= settings.LowFuelLapsLevel;

                        var fuelPercent = Convert.ToDouble(pluginManager.GetPropertyValue("DataCorePlugin.Computed.Fuel_Percent") ?? "100");

                        int ledidx = 0;

                        bool blink = RpmBlink.Blink;

                        if (settings.LedSettings != null)
                        {
                            foreach (var led in this.settings.LedSettings)
                            {
                                var currentValue = currentRpm;

                                if (led.DataSource == "Rpms")
                                {
                                    currentValue = currentRpm;
                                    blink = RpmBlink.Blink;
                                }
                                else if (led.DataSource == "FuelLaps")
                                {
                                    currentValue = fuelLapsRemaining;
                                    blink = FuelLapsBlink.Blink;
                                }
                                else if (led.DataSource == "FuelPercent")
                                {
                                    currentValue = fuelPercent;
                                    blink = FuelLapsBlink.Blink;
                                }

                                bool ledon = (currentValue > (double)led.OnRangeStart && currentValue <= (double)led.OnRangeEnd);
                                var baseColor = ledon ? led.OnColor : led.OffColor;
                                if (ledon && blink)
                                {
                                    baseColor = led.BlinkColor;
                                }

                                dash.SetLedColor(ledidx / 8, ledidx % 8, ToLedColor(baseColor));

                                ledidx++;
                            }
                        }
                    }
                }

                dash.Send();
            }
        }

        private int DisplayScreen(PluginManager pluginManager, System.Collections.Generic.List<Screen> RunningScreens, int CurrentScreenIndex)
        {
            dash.SetText(CurrentScreenIndex, "");

            if (RunningScreens.Count > 0)
            {
                CurrentScreenIndex = Math.Max(0, CurrentScreenIndex);
                CurrentScreenIndex = CurrentScreenIndex % RunningScreens.Count;
                var screen = RunningScreens[CurrentScreenIndex];

                DisplayScreen(pluginManager, RunningScreens.First());
                if (CurrentScreenIndex > 0)
                {
                    DisplayScreen(pluginManager, screen);
                }
            }
            return CurrentScreenIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            SaveSettings();
            // Cleaning
            lock (settings)
            {
                for (int i = 0; i < 4; i++)
                {
                    dash.SetText(i, "");
                    dash.SetLedsColor(i, SerialDash.LedColor.None);
                    dash.Send();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventName"></param>
        public void EventTriggered(string eventName)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Forms.Control GetSettingsControl(PluginManager pluginManager)
        {
            if (settingsControl == null)
            {
                settingsControl = new SerialDashSettingsControl(this);
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

            LoadSettings();

            this.dash.ButtonReleased += dash_ButtonPressed;
            this.dash.ButtonChanged += dash_ButtonChanged;

            for (int screen = 0; screen < global::SerialDash.SerialDashController.MAXSUPPORTED_SCREENS; screen++)
            {
                foreach (var item in Enum.GetValues(typeof(global::SerialDash.Buttons)))
                {
                    pluginManager.AddInput("SCREEN" + screen.ToString() + "_" + ((global::SerialDash.Buttons)item).ToString().ToUpper(), typeof(SerialDashPlugin));
                }
            }

            pluginManager.AddProperty("RPMStartOffset", typeof(SerialDashPlugin), typeof(double));
            pluginManager.AddEvent("RPMStartOffsetChanged", typeof(SerialDashPlugin));

            pluginManager.AddProperty("BlinkTriggerRatio", typeof(SerialDashPlugin), typeof(double));
            pluginManager.AddEvent("BlinkTriggerRatioChanged", typeof(SerialDashPlugin));

            pluginManager.AddProperty("LowFuelLapLevel", typeof(SerialDashPlugin), typeof(double));
            pluginManager.AddEvent("LowFuelLapLevelChanged", typeof(SerialDashPlugin));

            pluginManager.AddEvent("LowFuelLapAlert", typeof(SerialDashPlugin));

            pluginManager.AddProperty("DisplayIntensity", typeof(SerialDashPlugin), typeof(int));
            pluginManager.AddEvent("DisplayIntensityChanged", typeof(SerialDashPlugin));

            RefreshProperties();

            this.dash.SetIntensity(settings.Intensity);

            im.LongPress += im_LongPress;
        }

        private void im_LongPress(string input)
        {
            PluginManager.TriggerInput(input, typeof(SerialDashPlugin), PressType.LongPress);
        }

        private InputManager im = new InputManager();

        private void dash_ButtonChanged(System.Collections.Generic.List<ModuleButton> currentButtons)
        {
            im.SetCurrentInputs(currentButtons.Where(i => i.PressedButton != Buttons.ButtonNone)
                .Select(screen => "SCREEN" + screen.Screen.ToString() + "_" + screen.PressedButton.ToString().ToUpper()).ToList());

            //Logging.Current.Info("-- New inputs--");
            //foreach (var i in im.GetNewInputs())
            //{
            //    Logging.Current.Info(i + " pressed");
            //}

            //Logging.Current.Info("-- Released inputs--");
            //foreach (var i in im.GetReleasedInputs())
            //{
            //    Logging.Current.InfoFormat("{0} released (after {1}ms)", i.Key, i.Value.TotalMilliseconds);
            //}

            foreach (var i in im.GetShortInputs())
            {
                pluginManager.TriggerInput(i, typeof(SerialDashPlugin), PressType.ShortPress);
            }

            foreach (var i in im.GetNewInputs())
            {
                pluginManager.TriggerInputPress(i, typeof(SerialDashPlugin));
            }

            foreach (var i in im.GetReleasedInputs())
            {
                pluginManager.TriggerInputRelease(i, typeof(SerialDashPlugin));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputName"></param>
        public void InputTriggered(string inputName)
        {
        }

        private double ChangeValue(double currentvalue, double min, double max, double delta)
        {
            currentvalue = currentvalue + delta;
            if (currentvalue < min)
            {
                currentvalue = currentvalue + max - min;
            }
            if (currentvalue > max)
            {
                currentvalue = currentvalue - max + min;
            }
            return currentvalue;
        }

        private int ChangeValue(int currentvalue, int min, int max, int delta)
        {
            currentvalue = currentvalue + delta;
            if (currentvalue < min)
            {
                currentvalue = currentvalue + max - min;
            }
            if (currentvalue > max)
            {
                currentvalue = currentvalue - max + min;
            }
            return currentvalue;
        }

        private void CreateActions(PluginManager pluginManager)
        {
            pluginManager.ClearActions(typeof(SerialDashPlugin));

            #region RPMStartOffset

            pluginManager.AddAction("IncrementRPMStartOffset", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    if (manager.Status.GameRunning)
                    {
                        this.settings.RPMStartOffset = ChangeValue(this.settings.RPMStartOffset, 0, 100, 2); RefreshProperties();
                        pluginManager.TriggerEvent("RPMStartOffsetChanged", typeof(SerialDashPlugin));
                        ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                    }
                });
            pluginManager.AddAction("DecrementRPMStartOffset", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    if (manager.Status.GameRunning)
                    {
                        this.settings.RPMStartOffset = ChangeValue(this.settings.RPMStartOffset, 0, 100, -2); RefreshProperties();
                        pluginManager.TriggerEvent("RPMStartOffsetChanged", typeof(SerialDashPlugin));
                        ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                    }
                });

            #endregion RPMStartOffset

            #region LowFuelLapLevel

            pluginManager.AddAction("IncrementLowFuelLapLevel", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    if (manager.Status.GameRunning)
                    {
                        this.settings.RPMStartOffset = ChangeValue(this.settings.LowFuelLapsLevel, 0, 100, 2); RefreshProperties();
                        pluginManager.TriggerEvent("LowFuelLapLevelChanged", typeof(SerialDashPlugin));
                        ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                    }
                });
            pluginManager.AddAction("DecrementLowFuelLapLevel", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    if (manager.Status.GameRunning)
                    {
                        this.settings.RPMStartOffset = ChangeValue(this.settings.LowFuelLapsLevel, 0, 100, -2); RefreshProperties();
                        pluginManager.TriggerEvent("LowFuelLapLevelChanged", typeof(SerialDashPlugin));
                        ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                    }
                });

            #endregion LowFuelLapLevel

            #region DisplayIntensity

            pluginManager.AddAction("IncrementDisplayIntensity", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    dash.IncrementIntensity();
                    this.settings.Intensity = this.dash.GetIntensity(0);
                    SaveSettings();
                    RefreshProperties();
                    pluginManager.TriggerEvent("DisplayIntensityChanged", typeof(SerialDashPlugin));
                    ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                });
            pluginManager.AddAction("DecrementDisplayIntensity", typeof(SerialDashPlugin),
                (manager, actionname) =>
                {
                    dash.DecrementIntensity(); RefreshProperties();
                    this.settings.Intensity = this.dash.GetIntensity(0);
                    SaveSettings();
                    pluginManager.TriggerEvent("DisplayIntensityChanged", typeof(SerialDashPlugin));
                    ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                });

            #endregion DisplayIntensity

            #region BlinkTriggerRatio

            pluginManager.AddAction("IncrementBlinkTriggerRatio", typeof(SerialDashPlugin), (manager, actionname) =>
            {
                if (manager.Status.GameRunning)
                {
                    this.settings.RpmBlinkingLevel = ChangeValue(this.settings.RpmBlinkingLevel, 60, 100, 2); RefreshProperties();
                    pluginManager.TriggerEvent("BlinkTriggerRatioChanged", typeof(SerialDashPlugin));
                    ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                }
            });
            pluginManager.AddAction("DecrementBlinkTriggerRatio", typeof(SerialDashPlugin), (manager, actionname) =>
            {
                if (manager.Status.GameRunning)
                {
                    this.settings.RpmBlinkingLevel = ChangeValue(this.settings.RpmBlinkingLevel, 60, 100, -2); RefreshProperties();
                    pluginManager.TriggerEvent("BlinkTriggerRatioChanged", typeof(SerialDashPlugin));
                    ((SerialDashSettingsControl)GetSettingsControl(pluginManager)).LoadSettings();
                }
            });

            #endregion BlinkTriggerRatio

            #region ScreenSwitching

            pluginManager.AddAction("NextScreen", typeof(SerialDashPlugin), (manager, actionname) => { NextScreen(); });
            pluginManager.AddAction("PreviousScreen", typeof(SerialDashPlugin), (manager, actionname) => { PreviousScreen(); });
            pluginManager.AddAction("GoToFirstScreen", typeof(SerialDashPlugin), (manager, actionname) => { GoToFirstScreen(); });

            foreach (var screen in this.settings.Screens)
            {
                pluginManager.AddAction(
                    "DisplayScreen_" + screen.ScreenName, typeof(SerialDashPlugin),
                    (manager, actionname) =>
                    {
                        GoToScreen(screen);
                    },
                    (manager, actionname) =>
                    {
                        ReleaseScreen(screen);
                    });

                pluginManager.AddAction("DisplayScreenFor1s_" + screen.ScreenName, typeof(SerialDashPlugin), (manager, actionname) => { ShowStaticScreen(screen, 1000); });
                pluginManager.AddAction("DisplayScreenFor3s_" + screen.ScreenName, typeof(SerialDashPlugin), (manager, actionname) => { ShowStaticScreen(screen, 3000); });
            }

            #endregion ScreenSwitching
        }

        private void dash_ButtonPressed(int screen, global::SerialDash.Buttons buttons)
        {
            //Logging.Current.Info("SCREEN" + screen.ToString() + "_" + buttons.ToString().ToUpper());
            //PluginManager.TriggerInput("SCREEN" + screen.ToString() + "_" + buttons.ToString().ToUpper(), typeof(SerialDashPlugin));
        }

        private void DisplayScreen(Screen screen)
        {
            this.ScreenIndex = settings.Screens.FindIndex(i => i.ScreenName == screen.ScreenName);
        }

        private void DisplayScreen(PluginManager pluginManager, Screen screen)
        {
            int idx = 0;
            foreach (var parts in screen.ScrenParts)
            {
                if (screen.BlinkFrequency > 0)
                {
                    ScreenBlinker.BlinkTime = screen.BlinkFrequency;
                    ScreenBlinker.Started = true;
                }

                if (parts.Count > 0)
                {
                    dash.SetText(idx, "");

                    if (screen.BlinkFrequency > 0)
                    {
                        if (ScreenBlinker.Blink)
                        {
                            dash.SetText(idx, TextGenerator.GetText(pluginManager, parts));
                        }
                    }
                    else
                    {
                        dash.SetText(idx, TextGenerator.GetText(pluginManager, parts));
                    }
                }
                idx++;
            }
        }

        private void GoToScreen(Screen screen)
        {
            //this.staticScreen = null;
            //this.ScreenIndex = settings.Screens.FindIndex(i => i.ScreenName == screen.ScreenName);

            //this.staticScreen = screen;
            ClearStaticScreens();
            EnqueueStaticScreen(screen, 999999);

            this.staticScreenEnd = DateTime.MaxValue;

            ScreenBlinker.Started = false;
        }

        private void ReleaseScreen(Screen screen)
        {
            //this.staticScreen = null;
            //this.ScreenIndex = settings.Screens.FindIndex(i => i.ScreenName == screen.ScreenName);
            if (this.staticScreen == screen)
            {
                ClearStaticScreens();
                ScreenBlinker.Started = false;
            }
        }

        private void GoToFirstScreen()
        {
            //this.staticScreen = null;
            //this.ScreenIndex = settings.Screens.FindIndex(i => i.ScreenName == screen.ScreenName);

            ClearStaticScreens();
            ScreenBlinker.Started = false;
            if (pluginManager.Status.GameRunning)
            {
                ScreenIndex = 0;
            }
            else
            {
                ScreenIndexIdle = 0;
            }
        }

        private void LoadSettings()
        {
            this.settings = JsonExtensions.FromJsonFile<SerialDashPluginSettings>(SettingsPath);

            if (this.settings == null)
                settings = new SerialDashPluginSettings();

            ApplySettings();
        }

        private void NextScreen()
        {
            if (staticScreen != null) { ClearStaticScreens(); return; }
            ScreenBlinker.Started = false;
            if (pluginManager.Status.GameRunning)
            {
                ScreenIndex++;
            }
            else
            {
                ScreenIndexIdle++;
            }
        }

        private void PreviousScreen()
        {
            if (staticScreen != null) { ClearStaticScreens(); return; }
            ScreenBlinker.Started = false;
            if (pluginManager.Status.GameRunning)
            {
                ScreenIndex--;
            }
            else
            {
                ScreenIndexIdle--;
            }
        }

        private void RefreshProperties()
        {

            pluginManager.SetPropertyValue("DisplayIntensity", typeof(SerialDashPlugin), this.dash.GetIntensity(0));
            pluginManager.SetPropertyValue("BlinkTriggerRatio", typeof(SerialDashPlugin), this.settings.RpmBlinkingLevel);
            pluginManager.SetPropertyValue("RPMStartOffset", typeof(SerialDashPlugin), this.Settings.RPMStartOffset);
            pluginManager.SetPropertyValue("LowFuelLapLevel", typeof(SerialDashPlugin), this.Settings.LowFuelLapsLevel);
        }

        private void SaveSettings()
        {
            this.settings.ToJsonFile(SettingsPath);
        }

        private void ShowStaticScreen(Screen screen, int milliseconds)
        {
            //staticScreen = screen;
            //staticScreenEnd = DateTime.Now.AddMilliseconds(milliseconds);
            EnqueueStaticScreen(screen, (double)milliseconds / 1000d);
        }

        private SerialDash.LedColor ToLedColor(string color)
        {
            if (color != null && color.StartsWith("G")) return SerialDash.LedColor.Green;
            if (color != null && color.StartsWith("R")) return SerialDash.LedColor.Red;
            return SerialDash.LedColor.None;
        }
    }
}