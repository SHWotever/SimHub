using ACToolsUtilities.Serialisation;
using PropertyChanged;
using System;
using System.Globalization;
using System.IO;

namespace ACSharedMemory.Models.Track
{
    [ImplementPropertyChanged]
    public class TrackDesc
    {
        private static IniFileReader parser = new IniFileReader();

        static TrackDesc()
        {
            parser.Parser.Configuration.AssigmentSpacer = "";
        }

        public string Track { get; set; }

        public string TrackConfig { get; set; }

        private string trackPath;

        public string TrackPath
        {
            get { return trackPath; }
        }

        public string TrackCode
        {
            get
            {
                if (string.IsNullOrEmpty(TrackConfig))
                {
                    return Track;
                }
                else
                {
                    return Track + "-" + TrackConfig;
                }
            }
        }

        public string UIPath
        {
            get
            {
                if (string.IsNullOrEmpty(TrackConfig))
                {
                    return Track + "\\ui";
                }
                else
                {
                    return Track + "\\ui\\" + TrackConfig;
                }
            }
        }

        public string MapPath
        {
            get
            {
                if (trackPath == null) return null;
                if (string.IsNullOrEmpty(TrackConfig))
                {
                    return Track;
                }
                else
                {
                    return Track + "\\" + TrackConfig;
                }
            }
        }

        public string TrackOutlinePath
        {
            get
            {
                if (trackPath == null) return null;
                string path = System.IO.Path.Combine(trackPath, UIPath, "outline.png");
                if (ACToolsUtilities.FileOP.Exists(path))
                {
                    return path;
                }
                return null;
            }
        }

        public byte[] getTrackOutline()
        {
            string path = TrackOutlinePath;
            if (ACToolsUtilities.FileOP.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public byte[] getTrackPreview()
        {
            string path = TrackPreviewPath;

            if (ACToolsUtilities.FileOP.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public string TrackPreviewPath
        {
            get
            {
                if (trackPath == null) return null;
                string path = System.IO.Path.Combine(trackPath, UIPath, "preview.png");
                if (ACToolsUtilities.FileOP.Exists(path))
                {
                    return path;
                }
                return null;
            }
        }

        public static TrackDesc GetFromGameSettings(string acpath)
        {
            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");

            if (!ACToolsUtilities.FileOP.Exists(raceIniPath))
            {
                return null;
            }

            var data = parser.ReadFile(raceIniPath);

            string Track = data["RACE"]["TRACK"];
            string TrackConfig = null;
            if (data["RACE"].ContainsKey("CONFIG_TRACK"))
            {
                TrackConfig = data["RACE"]["CONFIG_TRACK"];
            }

            return new TrackDesc(acpath, Track, TrackConfig);
        }

        private double GetDoubleValue(IniParser.Model.KeyDataCollection data, string key, double defaultValue = 0)
        {
            if (data.ContainsKey(key))
            {
                return double.Parse(data[key], CultureInfo.InvariantCulture);
            }
            return defaultValue;
        }

        public static TrackDesc FromName(string acpath, string trackCode, string trackConfig)
        {
            return new TrackDesc(acpath, trackCode, trackConfig);
        }

        public bool IsMissing { get; set; }

        public TrackDesc()
        {
        }

        public TrackDesc(string acpath, string trackCode, string trackConfig)
        {
            this.Track = trackCode;
            this.TrackConfig = trackConfig;
            this.trackPath = System.IO.Path.Combine(acpath, "content", "tracks");

            var parser = new IniFileReader();
            parser.Parser.Configuration.AssigmentSpacer = "";

            try
            {
                string MapIniPath = System.IO.Path.Combine(this.trackPath, this.MapPath, "data", "map.ini");
                string MapImagePath = System.IO.Path.Combine(this.trackPath, this.MapPath, "map.png");

                if (ACToolsUtilities.FileOP.Exists(MapIniPath))
                {
                    var mapdata = parser.ReadFile(MapIniPath);

                    this.MapSettings = new MapSettings
                    {
                        DRAWING_SIZE = GetDoubleValue(mapdata.Sections["PARAMETERS"], "DRAWING_SIZE"),
                        WIDTH = GetDoubleValue(mapdata.Sections["PARAMETERS"], "WIDTH"),
                        HEIGHT = GetDoubleValue(mapdata.Sections["PARAMETERS"], "HEIGHT"),
                        MARGIN = GetDoubleValue(mapdata.Sections["PARAMETERS"], "MARGIN"),
                        SCALE_FACTOR = GetDoubleValue(mapdata.Sections["PARAMETERS"], "SCALE_FACTOR"),
                        X_OFFSET = GetDoubleValue(mapdata.Sections["PARAMETERS"], "X_OFFSET"),
                        Y_OFFSET = GetDoubleValue(mapdata.Sections["PARAMETERS"], "Y_OFFSET"),
                        Z_OFFSET = GetDoubleValue(mapdata.Sections["PARAMETERS"], "Z_OFFSET"),
                        MapImagePath = MapImagePath,
                    };
                }
            }
            catch { }

            try
            {
                this.TrackInfo = TrackInfo.FromFile(System.IO.Path.Combine(acpath, "content", "tracks", this.UIPath, "ui_track.json"));
            }
            catch
            {
                this.TrackInfo = new TrackInfo();
            }
        }

        public TrackInfo TrackInfo { get; set; }

        public MapSettings MapSettings { get; set; }
    }
}