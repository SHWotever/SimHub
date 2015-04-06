using IniParser;
using System;
using System.Globalization;
using System.IO;

namespace ACSharedMemory.Models.Track
{
    public class TrackDesc
    {
        public string Track { get; set; }

        public string TrackConfig { get; set; }

        private string tracksPath;

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

        public byte[] getTrackOutline()
        {
            string path = System.IO.Path.Combine(tracksPath, UIPath, "outline.png");
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public byte[] getTrackPreview()
        {
            string path = System.IO.Path.Combine(tracksPath, UIPath, "preview.png");
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        public static TrackDesc GetFromGameSettings(string acpath)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");

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

        public TrackDesc(string acpath, string trackCode, string trackConfig)
        {
            this.Track = trackCode;
            this.TrackConfig = trackConfig;
            this.tracksPath = System.IO.Path.Combine(acpath, "content", "tracks");

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            try
            {
                string MapIniPath = System.IO.Path.Combine(this.tracksPath, this.MapPath, "data", "map.ini");
                string MapImagePath = System.IO.Path.Combine(this.tracksPath, this.MapPath, "map.png");

                if (System.IO.File.Exists(MapImagePath))
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