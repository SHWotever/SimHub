using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(item));
        }

        public static T FromJsonFile<T>(string path)
        {
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
        }

    }
}
