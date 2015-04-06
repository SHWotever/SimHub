using System;
using System.Windows.Forms;

//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

namespace MapGenerator
{
    internal class Program
    {
        //public static double GetDistanceBetweenPoints(Point p, Point q)
        //{
        //    double a = p.X - q.X;
        //    double b = p.Y - q.Y;
        //    double distance = Math.Sqrt(a * a + b * b);
        //    return distance;
        //}
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            Application.Run(new MapPreview());
            return;

            //string dataFile = args[0];
            //Console.WriteLine("Map Scale ? ie 1 or 10");
            //int scale = int.Parse(Console.ReadLine());

            //string basedir = System.IO.Path.Combine(Path.GetDirectoryName(dataFile), "Output\\" + System.IO.Path.GetFileNameWithoutExtension(dataFile));
            //if (!System.IO.Directory.Exists(basedir + "\\data\\"))
            //{
            //    System.IO.Directory.CreateDirectory(basedir + "\\data\\");
            //}

            //var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<float[]>>(System.IO.File.ReadAllText(dataFile));

            //foreach (var item in data)
            //{
            //    for (int i = 0; i < item.Length; i++)
            //    {
            //        item[i] = item[i] / (float)scale;
            //    }
            //}

            //if (data != null && data.Count > 5)
            //{
            //    int margin = 50;

            //    int minvalueX = (int)data.Min(i => i[0]) - margin;
            //    int minvalueY = (int)data.Min(i => i[2]) - margin;

            //    int maxvalueX = (int)data.Max(i => i[0]) + margin;
            //    int maxvalueY = (int)data.Max(i => i[2]) + margin;

            //    int width = maxvalueX - minvalueX;
            //    int height = maxvalueY - minvalueY;

            //    var dataPoints = data.Select(i => new Point(i[0] - minvalueX, i[2] - minvalueY)).ToList();

            //    // Clean datapoints
            //    for (int i = dataPoints.Count - 1; i > 0; i--)
            //    {
            //        if (dataPoints[i] == dataPoints[i - 1])
            //        {
            //            dataPoints.RemoveAt(i);
            //        }
            //    }

            //    DrawingVisual dv = new AliasedDrawingVisual();

            //    using (var dc = dv.RenderOpen())
            //    {
            //        int LastRenderedIdx = 0;
            //        int distanceThreshold = 5;

            //        int innerPathWidth = 3;
            //        int outerPathWidth = 5;

            //        LastRenderedIdx = 0;
            //        for (int i = 1; i < dataPoints.Count; i++)
            //        {
            //            var blackPen = new Pen(Brushes.Black, 1);
            //            dc.DrawEllipse(Brushes.Black, null, dataPoints[i], outerPathWidth, outerPathWidth);

            //            //if (GetDistanceBetweenPoints(dataPoints[i], dataPoints[LastRenderedIdx]) > distanceThreshold)
            //            //{
            //            //    dc.DrawLine(blackPen, dataPoints[LastRenderedIdx], dataPoints[i]);
            //            //    LastRenderedIdx = i;
            //            //}
            //        }

            //        LastRenderedIdx = 0;
            //        for (int i = 1; i < dataPoints.Count; i++)
            //        {
            //            var whitePen = new Pen(Brushes.White, 1);
            //            dc.DrawEllipse(Brushes.White, null, dataPoints[i], innerPathWidth, innerPathWidth);

            //            //if (GetDistanceBetweenPoints(dataPoints[i], dataPoints[LastRenderedIdx]) > distanceThreshold)
            //            //{
            //            //    var whitePen = new Pen(Brushes.White, 10);
            //            //    dc.DrawLine(whitePen, dataPoints[LastRenderedIdx], dataPoints[i]);
            //            //    LastRenderedIdx = i;
            //            //}
            //        }

            //        dc.Close();
            //        // The BitmapSource that is rendered with a Visual.
            //        RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            //        rtb.Render(dv);

            //        // Encoding the RenderBitmapTarget as a PNG file.
            //        PngBitmapEncoder png = new PngBitmapEncoder();
            //        png.Frames.Add(BitmapFrame.Create(rtb));
            //        using (Stream stm = File.Create(Path.Combine(basedir, "map.png")))
            //        {
            //            png.Save(stm);
            //        }

            //        var parser = new FileIniDataParser();
            //        parser.Parser.Configuration.AssigmentSpacer = "";

            //        IniData entryData = new IniData();
            //        entryData.Configuration.AssigmentSpacer = "";
            //        entryData.Sections.AddSection("PARAMETERS");
            //        entryData["PARAMETERS"].AddKey("WIDTH", width.ToString());
            //        entryData["PARAMETERS"].AddKey("HEIGHT", height.ToString());
            //        entryData["PARAMETERS"].AddKey("MARGIN", margin.ToString());
            //        entryData["PARAMETERS"].AddKey("SCALE_FACTOR", (scale).ToString());
            //        entryData["PARAMETERS"].AddKey("MAX_SIZE", (1600).ToString());

            //        entryData["PARAMETERS"].AddKey("X_OFFSET", (-1 * minvalueX).ToString());
            //        entryData["PARAMETERS"].AddKey("Z_OFFSET", (-1 * minvalueY).ToString());
            //        entryData["PARAMETERS"].AddKey("DRAWING_SIZE", (outerPathWidth * 2).ToString());

            //        parser.WriteFile(Path.Combine(basedir, "data", "map.ini"), entryData);

            //    }
            //}
        }

        //public class AliasedDrawingVisual : DrawingVisual
        //{
        //    public AliasedDrawingVisual()
        //    {
        //        //this.VisualEdgeMode = EdgeMode.Aliased;
        //    }
        //}
    }
}