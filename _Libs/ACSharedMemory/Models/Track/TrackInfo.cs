using System.Collections.Generic;

namespace ACSharedMemory.Models.Track
{
    public class TrackInfo
    {
        public static TrackInfo FromFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TrackInfo>(json);
            }
            else
            {
                return new TrackInfo();
            }
        }

        public string name { get; set; }

        public string description { get; set; }

        public List<string> tags { get; set; }

        public List<string> geotags { get; set; }

        public string country { get; set; }

        public string city { get; set; }

        public string length { get; set; }

        public string width { get; set; }

        public string pitboxes { get; set; }

        public string run { get; set; }
    }
}