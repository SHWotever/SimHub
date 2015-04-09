using System.Collections.Generic;

namespace ACSharedMemory.Models.Car
{
    public class CarInfo
    {
        public static CarInfo FromModel(string ACPAth, string model)
        {
            return CarInfo.FromFile(System.IO.Path.Combine(ACPAth, "content", "cars", model, "ui", "ui_car.json"));
        }

        public static CarInfo FromFile(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CarInfo>(json);
            return result;
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