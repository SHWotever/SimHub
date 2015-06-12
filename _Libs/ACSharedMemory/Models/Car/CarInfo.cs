using Newtonsoft.Json;
using PropertyChanged;
using System.Collections.Generic;
using System.IO;

namespace ACSharedMemory.Models.Car
{
    [ImplementPropertyChanged]
    public class CarInfo
    {
        public static CarInfo FromModel(string ACPAth, string model)
        {
            return CarInfo.FromFile(System.IO.Path.Combine(ACPAth, "content", "cars", model, "ui", "ui_car.json"));
        }

        public static CarInfo FromFile(string path)
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
                        return serializer.Deserialize<CarInfo>(reader);
                    }

                    //return  (new System.Text.Json.JsonParser()).Parse<CarInfo>(s);
                }

                //    var json = System.IO.File.ReadAllText(path);
                //    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CarInfo>();
                //}
                //return result;
            }
            return new CarInfo();
        }

        public string name { get; set; }

        public string parent { get; set; }

        public string brand { get; set; }

        public string description { get; set; }

        public List<string> tags { get; set; }

        public string @class { get; set; }

        public Specs specs { get; set; }

        public List<List<string>> torqueCurve { get; set; }

        public List<List<string>> powerCurve { get; set; }
    }
}