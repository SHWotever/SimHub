using Newtonsoft.Json;
using PropertyChanged;
using System.Collections.Generic;
using System.IO;

namespace ACSharedMemory.Models.Track
{
    [ImplementPropertyChanged]
    public class TrackInfo
    {
        public static TrackInfo FromFile(string path)
        {
            if (ACToolsUtilities.FileOP.Exists(path))
            {
                using (var s = new StreamReader(path))
                {
                    using (JsonReader reader = new JsonTextReader(s))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        // read the json from a stream
                        // json size doesn't matter because only a small piece is read at a time from the HTTP request
                        return serializer.Deserialize<TrackInfo>(reader);
                    }

                    //return (new System.Text.Json.JsonParser()).Parse<TrackInfo>(s);
                }
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