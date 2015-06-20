using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

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

        public static string ToJson(this object item)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
        }

        public static T JsonClone<T>(this T objectInstance)
        {
            var tmp = Newtonsoft.Json.JsonConvert.SerializeObject(objectInstance);
            return (T)(T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(tmp);
        }

        public static T FromJsonFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
        }

        public static T FromJsonGZipFile<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var outputStream = new GZipStream(fileStream, CompressionMode.Decompress, false))
                {
                    using (var reader = new StreamReader(outputStream))
                    {
                        var serializer = new JsonSerializer();

                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            jsonReader.Read();
                            return serializer.Deserialize<T>(jsonReader);
                        }
                    }
                }
            }
        }

        public static void ToJsonGZipFile<T>(this T objectInstance, string path)
        {
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(path)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (var outputStream = new GZipStream(fileStream, CompressionMode.Compress, false))
                {
                    using (var writer = new StreamWriter(outputStream))
                    {
                        writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objectInstance));
                    }
                }
            }
        }

        public static T FromJsonFile<T>(string path, Func<string, string> textHandler)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(textHandler(System.IO.File.ReadAllText(path)));
        }

        public static void CleanFolder(string directory)
        {
            foreach (var file in GetFiles(directory, "*"))
            {
                File.Delete(file);
            }
        }

        public static IEnumerable<string> GetFiles(string directory, string pattern)
        {
            if (Directory.Exists(directory))
            {
                return System.IO.Directory.GetFiles(directory, pattern);
            }
            else
            {
                return new string[0];
            }
        }
    }
}