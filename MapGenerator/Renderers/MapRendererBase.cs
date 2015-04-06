using IniParser;
using IniParser.Model;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapGenerator.Renderers
{
    public class MapException : Exception
    {
        public MapException(string message)
            : base(message)
        {
        }
    }

    public abstract class MapRendererBase
    {
        public MapRendererBase()
        {
            Scale = 1;
            Margin = 50;
            InnerPathWidth = 3;
            OuterPathWidth = 5;
        }

        protected abstract PngBitmapEncoder GetPNG();

        public decimal Scale { get; set; }

        public int Margin { get; protected set; }

        public int MinValueX { get; protected set; }

        public int MinValueY { get; protected set; }

        public int MaxValueX { get; protected set; }

        public int MaxValueY { get; protected set; }

        public int MapWidth { get; protected set; }

        public int MapHeight { get; protected set; }

        public int SectorCount { get; protected set; }

        public string LogFile { get; protected set; }

        public Color TrackColor { get; set; }

        public Color TrackBorderColor { get; set; }

        public Color AlternateSectorColor { get; set; }

        public int InnerPathWidth { get; set; }

        public int OuterPathWidth { get; set; }

        public int CloseLoopPoints { get; set; }

        private void WriteIniFile(string outputBaseDir)
        {
            try
            {
                var parser = new FileIniDataParser();
                parser.Parser.Configuration.AssigmentSpacer = "";
                IniData entryData = new IniData();
                entryData.Configuration.AssigmentSpacer = "";
                entryData.Sections.AddSection("PARAMETERS");
                entryData["PARAMETERS"].AddKey("WIDTH", MapWidth.ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("HEIGHT", MapHeight.ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("MARGIN", Margin.ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("SCALE_FACTOR", (Scale).ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("MAX_SIZE", (1600).ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("X_OFFSET", (-1 * MinValueX * Scale).ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("Z_OFFSET", (-1 * MinValueY * Scale).ToString(CultureInfo.InvariantCulture));
                entryData["PARAMETERS"].AddKey("DRAWING_SIZE", (10).ToString(CultureInfo.InvariantCulture));
                parser.WriteFile(Path.Combine(outputBaseDir, "data", "map.ini"), entryData);
            }
            catch
            {
                throw new MapException("An error occured while writing map ini file");
            }
        }

        public System.Drawing.Image GetMap()
        {
            PngBitmapEncoder png = GetPNG();
            MemoryStream myStream = new MemoryStream();
            png.Save(myStream);
            myStream.Position = 0;
            return System.Drawing.Bitmap.FromStream(myStream);
        }

        public void ExportMap(string path)
        {
            string outputBaseDir = System.IO.Path.Combine(path, System.IO.Path.GetFileNameWithoutExtension(LogFile));

            // Create data folder
            if (!System.IO.Directory.Exists(outputBaseDir + "\\data\\"))
            {
                System.IO.Directory.CreateDirectory(outputBaseDir + "\\data\\");
            }

            // Copy data file to output
            System.IO.File.Copy(this.LogFile, System.IO.Path.Combine(outputBaseDir, System.IO.Path.GetFileName(LogFile)), true);

            // Create map
            PngBitmapEncoder png = GetPNG();
            using (Stream stm = File.Create(Path.Combine(outputBaseDir, "map.png")))
            {
                png.Save(stm);
            }

            WriteIniFile(outputBaseDir);
        }
    }
}