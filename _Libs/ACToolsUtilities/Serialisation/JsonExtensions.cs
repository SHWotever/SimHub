using System.IO;

namespace ACToolsUtilities.Serialisation
{
    public static class JsonExtensions
    {
        public static void ToJsonFile(this object item, string path)
        {
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(path)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented));
        }

        public static T FromJsonFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
        }
    }
}