using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Led definition
    /// </summary>
    public class LedDefinition
    {
        /// <summary>
        /// CTOr
        /// </summary>
        public LedDefinition()
        {
            DataSource = "Rpms";
        }

        /// <summary>
        /// Color when blinking
        /// </summary>
        public string BlinkColor { get; set; }

        /// <summary>
        /// Color when not in range
        /// </summary>
        public string OffColor { get; set; }

        /// <summary>
        /// Data origin
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Color when  in range
        /// </summary>
        public string OnColor { get; set; }

        /// <summary>
        /// Range end
        /// </summary>
        public decimal OnRangeEnd { get; set; }

        /// <summary>
        /// Range start
        /// </summary>
        public decimal OnRangeStart { get; set; }
    }

    /// <summary>
    /// Screen
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Description
        /// </summary>
        [JsonIgnore]
        public string Description
        {
            get
            {
                return string.Format("[{0}][{1}] {2}", GameRunningScreen ? "ACRUNNING" : "         ", GameNotRunningScreen ? "ACIDLE" : "      ", ScreenName);
            }
        }

        /// <summary>
        /// When game is not running ?
        /// </summary>
        public bool GameNotRunningScreen { get; set; }

        /// <summary>
        /// When game is running ?
        /// </summary>
        public bool GameRunningScreen { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// Parts
        /// </summary>
        public List<ScreenItem> ScrenParts { get; set; }

        public List<ScreenAnnouncePart> ScrenAnnounce { get; set; }

        /// <summary>
        /// Blink Frequency
        /// </summary>
        public int BlinkFrequency { get; set; }
    }

    public class ScreenAnnouncePart
    {
        public string AnnounceText { get; set; }
    }

    public class ScreenItem : List<ScreenPart>
    {
        /// <summary>
        /// Default CTOR
        /// </summary>
        public ScreenItem()
        {
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="collection"></param>
        public ScreenItem(IEnumerable<ScreenPart> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Text shown when screen is activated
        /// </summary>
        public string AnnounceText { get; set; }
    }

    /// <summary>
    /// Screen part
    /// </summary>
    public class ScreenPart : ICloneable
    {
        /// <summary>
        /// Description
        /// </summary>
        [JsonIgnore]
        public string Description
        {
            get
            {
                return (this.Expression == null ? "TEXT : \"" + this.Text : "EXPR : \"" + Expression) + "\"";
            }
        }

        /// <summary>
        /// Expression
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Length
        /// </summary>
        public int FixedLength { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Right align
        /// </summary>
        public bool RightAlign { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    /// <summary>
    /// Settings for screens
    /// </summary>
    public class SerialDashPluginSettings
    {
        /// <summary>
        /// CTor
        /// </summary>
        public SerialDashPluginSettings()
        {
            this.Screens = new List<Screen>();
            this.RpmBlinkingLevel = 96;
            this.LowFuelLapsLevel = 2;
            this.LowFuelLapsAlertInterval = 30;
            this.AnnounceTime = 0.5;
           // this.ModuleLogicalMap = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        public List<int> ModuleLogicalMap { get; set; }

        /// <summary>
        /// Announce screen display time (seconds)
        /// </summary>
        public double AnnounceTime { get; set; }

        /// <summary>
        /// High RPM Binking level
        /// </summary>
        public double RpmBlinkingLevel { get; set; }

        /// <summary>
        /// Low Lap Blinking level
        /// </summary>
        public double LowFuelLapsLevel { get; set; }

        /// <summary>
        /// Laox fuel alert interval
        /// </summary>
        public int LowFuelLapsAlertInterval { get; set; }

        /// <summary>
        /// Current screen
        /// </summary>
        public int currentScreenIndex { get; set; }

        /// <summary>
        /// Led definition
        /// </summary>
        public List<LedDefinition> LedSettings { get; set; }

        /// <summary>
        /// reverse screen
        /// </summary>
        public bool ReverseScreen0 { get; set; }

        /// <summary>
        /// reverse screen
        /// </summary>
        public bool ReverseScreen1 { get; set; }

        /// <summary>
        /// reverse screen
        /// </summary>
        public bool ReverseScreen2 { get; set; }

        /// <summary>
        /// reverse screen
        /// </summary>
        public bool ReverseScreen3 { get; set; }

        /// <summary>
        /// Available screens
        /// </summary>
        public List<Screen> Screens { get; set; }

        /// <summary>
        /// Start offset in %
        /// </summary>
        public double RPMStartOffset { get; set; }

        public int Intensity { get; set; }
    }
}