using System.Collections.Generic;
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

        public static void CleanFolder(string directory) {
            foreach (var file in GetFiles(directory, "*")) {
                File.Delete(file);
            }
        }

        public static  IEnumerable<string> GetFiles(string directory, string pattern)
        {
            if (Directory.Exists(directory))
            {
                return System.IO.Directory.GetFiles(directory, pattern);
            }
            else { 
                return new string[0];
            }

        }
    }
}