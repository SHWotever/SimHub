using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapGenerator.Renderers
{
    public class SimpleMapRenderer : MapRendererBase
    {
        private List<float[]> data;

        public SimpleMapRenderer(string logFile)
            : base()
        {
            Scale = 1;
            Margin = 50;
            InnerPathWidth = 3;
            OuterPathWidth = 5;
            LogFile = logFile;
            data = GetData(LogFile);
        }

        protected override PngBitmapEncoder GetPNG()
        {
            try
            {
                var scaledData = this.data.Select(i => (float[])(i.Clone())).ToList();

                foreach (var item in scaledData)
                {
                    for (int i = 0; i < item.Length; i++)
                    {
                        item[i] = item[i] / (float)Scale;
                    }
                }

                PngBitmapEncoder png;
                Margin = 50;

                MinValueX = (int)scaledData.Min(i => i[0]) - Margin;
                MinValueY = (int)scaledData.Min(i => i[2]) - Margin;

                MaxValueX = (int)scaledData.Max(i => i[0]) + Margin;
                MaxValueY = (int)scaledData.Max(i => i[2]) + Margin;

                MapWidth = MaxValueX - MinValueX;
                MapHeight = MaxValueY - MinValueY;

                var dataPoints = scaledData.Select(i => new Point(i[0] - MinValueX, i[2] - MinValueY)).ToList();

                // Clean datapoints
                for (int i = dataPoints.Count - 1; i > 0; i--)
                {
                    if (dataPoints[i] == dataPoints[i - 1])
                    {
                        dataPoints.RemoveAt(i);
                    }
                }

                DrawingVisual dv = new DrawingVisual();

                using (var dc = dv.RenderOpen())
                {
                    if (OuterPathWidth > 0)
                    {
                        for (int i = 1; i < dataPoints.Count; i++)
                        {
                            dc.DrawEllipse(new SolidColorBrush(TrackBorderColor), null, dataPoints[i], OuterPathWidth + InnerPathWidth, OuterPathWidth + InnerPathWidth);
                        }
                    }

                    if (InnerPathWidth > 0)
                    {
                        for (int i = 1; i < dataPoints.Count; i++)
                        {
                            dc.DrawEllipse(new SolidColorBrush(TrackColor), null, dataPoints[i], InnerPathWidth, InnerPathWidth);
                        }
                    }

                    dc.Close();
                    RenderTargetBitmap rtb = new RenderTargetBitmap(MapWidth, MapHeight, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(dv);

                    png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(rtb));
                }

                return png;
            }
            catch
            {
                throw new MapException("An error occured while generating map");
            }
        }

        private List<float[]> GetData(string dataFile)
        {
            try
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<float[]>>(File.ReadAllText(dataFile));

                if (data == null || data.Count < 10)
                {
                    throw new MapException("Data file seems empty");
                }

                return data;
            }
            catch
            {
                throw new MapException("Data file unreadable");
            }
        }
    }
}