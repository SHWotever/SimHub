using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ACSharedMemory
{
    public static class TelemetryReader
    {
        public static TelemetryData Read(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (var outputStream = new GZipStream(fileStream, CompressionMode.Decompress, false))
                {
                    using (var reader = new StreamReader(outputStream))
                    {
                        var serializer = new JsonSerializer();
                        var result = new TelemetryData();

                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            jsonReader.SupportMultipleContent = true;

                            jsonReader.Read();
                            result.StaticInfo = serializer.Deserialize<StaticInfo>(jsonReader);

                            result.Data = new List<TelemetryContainer>();
                            while (jsonReader.Read())
                            {
                                result.Data.Add(serializer.Deserialize<TelemetryContainer>(jsonReader));
                            }
                        }
                        return result;
                    }
                }
            }
        }
    }

    public class TelemetryWriter
    {
        private FileStream fileStream;
        private GZipStream outputStream;
        private StreamWriter writer;
        private bool KeepFile = false;
        private string FileName;

        public TelemetryWriter(string fileName)
        {
            this.FileName = fileName;
            fileStream = new FileStream(fileName, FileMode.Create);
            outputStream = new GZipStream(fileStream, CompressionMode.Compress, false);
            writer = new StreamWriter(outputStream);
        }

        public void Write(TelemetryContainer container, StaticInfo s)
        {
            lock (this)
            {
                if (!KeepFile)
                {
                    writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(s));
                }
                KeepFile = true;
                writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(container));
            }
        }

        public void close()
        {
            lock (this)
            {
                writer.Flush();
                outputStream.Flush();
                fileStream.Flush();

                writer.Close();

                if (!KeepFile)
                {
                    try
                    {
                        System.IO.File.Delete(FileName);
                    }
                    catch { }
                }
            }
        }
    }

    public class TelemetryData
    {
        public List<TelemetryContainer> Data { get; set; }

        public StaticInfo StaticInfo { get; set; }
    }

    public class TelemetryContainer
    {
        public Graphics Graphics { get; set; }

        public Physics Physics { get; set; }
    }
}