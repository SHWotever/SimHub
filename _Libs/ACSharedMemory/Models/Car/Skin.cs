using Newtonsoft.Json;
using PropertyChanged;
using System;

namespace ACSharedMemory.Models.Car
{
    [ImplementPropertyChanged]
    public class Skin
    {
        public string Name { get; set; }

        public string PreviewImage { get; set; }

        public string skinname { get; set; }

        public string drivername { get; set; }

        public string country { get; set; }

        public string team { get; set; }

        // [JsonConverter(typeof(IntegerConverter))]
        public string number { get; set; }
    }

    public class IntegerConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();

            return 0;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}