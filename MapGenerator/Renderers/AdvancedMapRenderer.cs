using ACSharedMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapGenerator.Renderers
{
    public class AdvancedMapRenderer : MapRendererBase
    {
        private TelemetryData data;

        public static double GetDistanceBetweenPoints(float[] p, float[] q)
        {
            double a = p[0] - q[0];
            double b = p[2] - q[2];
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        public AdvancedMapRenderer(string logFile, TelemetryData data)
            : base()
        {
            LogFile = logFile;
            this.data = data;
            SimplifyData();
        }

        private void SimplifyData()
        {
            for (int i = 0; i < data.Data.Count - 1; i++)
            {
                if (GetDistanceBetweenPoints(data.Data[i].Graphics.CarCoordinates, data.Data[i + 1].Graphics.CarCoordinates) < 2)
                {
                    data.Data.RemoveAt(i + 1);
                    i--;
                }
            }
        }

        private float ScaleCoord(float i)
        {
            return i / (float)Scale;
        }

        private Point toScaledPoint(float[] i)
        {
            return new Point(i[0] / (float)Scale - MinValueX, i[2] / (float)Scale - MinValueY);
        }

        protected override PngBitmapEncoder GetPNG()
        {
            try
            {
                this.SectorCount = this.data.Data.Max(i => i.Graphics.CurrentSectorIndex);

                PngBitmapEncoder png;
                Margin = 50;

                MinValueX = (int)this.data.Data.Min(i => ScaleCoord(i.Graphics.CarCoordinates[0])) - Margin;
                MinValueY = (int)this.data.Data.Min(i => ScaleCoord(i.Graphics.CarCoordinates[2])) - Margin;

                MaxValueX = (int)this.data.Data.Max(i => ScaleCoord(i.Graphics.CarCoordinates[0])) + Margin;
                MaxValueY = (int)this.data.Data.Max(i => ScaleCoord(i.Graphics.CarCoordinates[2])) + Margin;

                MapWidth = MaxValueX - MinValueX;
                MapHeight = MaxValueY - MinValueY;

                DrawingVisual dv = new DrawingVisual();

                using (var dc = dv.RenderOpen())
                {
                    if (OuterPathWidth > 0)
                    {
                        DrawOuterPath(dc);
                        if (CloseLoopPoints > 0)
                        {
                            DrawOuterLoopClose(dc);
                        }
                    }

                    if (InnerPathWidth > 0)
                    {
                        DrawInnerPath(dc);

                        DrawSectorsSeparators(dc);

                        if (CloseLoopPoints > 0)
                        {
                            DrawInnerLoopClose(dc);
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

        private void DrawInnerLoopClose(DrawingContext dc)
        {
            {
                var drawColor = data.Data[data.Data.Count - CloseLoopPoints - 1].Graphics.CurrentSectorIndex % 2 == 0 ? TrackColor : AlternateSectorColor;
                dc.DrawLine(
                    new Pen(new SolidColorBrush(drawColor), (InnerPathWidth) * 2),
                        toScaledPoint(data.Data[data.Data.Count - CloseLoopPoints - 1].Graphics.CarCoordinates),
                        toScaledPoint(data.Data[0].Graphics.CarCoordinates)
                    );
            }

            {
                var drawColor = data.Data[0].Graphics.CurrentSectorIndex % 2 == 0 ? TrackColor : AlternateSectorColor;
                dc.DrawLine(
                    new Pen(new SolidColorBrush(drawColor), (InnerPathWidth) * 2),
                        toScaledPoint(data.Data[0].Graphics.CarCoordinates),
                        toScaledPoint(data.Data[CloseLoopPoints + 1].Graphics.CarCoordinates)
                    );
            }
            {
                var drawColor = data.Data[0].Graphics.CurrentSectorIndex % 2 == 0 ? TrackColor : AlternateSectorColor;
                dc.DrawEllipse(new SolidColorBrush(drawColor), null, toScaledPoint(data.Data[0].Graphics.CarCoordinates), InnerPathWidth, InnerPathWidth);
            }

            var point1 = data.Data[0].Graphics.CarCoordinates;
            var point2 = data.Data[CloseLoopPoints + 1].Graphics.CarCoordinates;
            DrawSeparator(dc, point1, point2);
        }

        private void DrawSectorsSeparators(DrawingContext dc)
        {
            for (int i = 1 + CloseLoopPoints; i < data.Data.Count - CloseLoopPoints; i++)
            {
                if (i > 0)
                {
                    //Sector separator
                    if (data.Data[i].Graphics.CurrentSectorIndex != data.Data[i - 1].Graphics.CurrentSectorIndex)
                    {
                        var point1 = data.Data[i - 1].Graphics.CarCoordinates;
                        var point2 = data.Data[i].Graphics.CarCoordinates;
                        DrawSeparator(dc, point1, point2);
                    }
                }
            }
        }

        private void DrawSeparator(DrawingContext dc, float[] point1, float[] point2)
        {
            if (SectorSeparators)
            {
                var point = toScaledPoint(point1);
                var prevpoint = toScaledPoint(point2);
                var vector = new Vector(point.X - prevpoint.X, point.Y - prevpoint.Y);
                var angle = Vector.AngleBetween(vector, new Vector(0, 1));

                dc.PushTransform(new TranslateTransform(point.X, point.Y));
                {
                    dc.PushTransform(new RotateTransform(-angle));
                    {
                        dc.DrawLine(new Pen(new SolidColorBrush(SectorSeparatorsColor), SectorSeparatorsHeight), new Point(-SectorSeparatorsWidth / 2, 0), new Point(SectorSeparatorsWidth / 2, 0));
                    }
                    dc.Pop();
                }
                dc.Pop();
            }
        }

        private void DrawInnerPath(DrawingContext dc)
        {
            for (int i = 1 + CloseLoopPoints; i < data.Data.Count - CloseLoopPoints; i++)
            {
                //var drawColor = data.Data[i].Graphics.CurrentSectorIndex % 2 == 0 ? TrackColor : AlternateSectorColor;
                var drawColor = GetColor(i);

                dc.DrawEllipse(new SolidColorBrush(drawColor), null, toScaledPoint(data.Data[i].Graphics.CarCoordinates), InnerPathWidth, InnerPathWidth);
                if (i > 0)
                {
                    dc.DrawLine(
                       new Pen(new SolidColorBrush(drawColor), (InnerPathWidth) * 2),
                           toScaledPoint(data.Data[i].Graphics.CarCoordinates),
                           toScaledPoint(data.Data[i - 1].Graphics.CarCoordinates)
                   );

                    //Sector separator
                    //if (data.Data[i].Graphics.CurrentSectorIndex != data.Data[i - 1].Graphics.CurrentSectorIndex)
                    //{
                    //    var point = toScaledPoint(data.Data[i].Graphics.CarCoordinates);
                    //    var prevpoint = toScaledPoint(data.Data[i - 1].Graphics.CarCoordinates);
                    //    var vector = new Vector(point.X - prevpoint.X, point.Y - prevpoint.Y);
                    //    var angle = Vector.AngleBetween(vector, new Vector(0, 1));

                    //    dc.PushTransform(new TranslateTransform(point.X, point.Y));
                    //    {
                    //        dc.PushTransform(new RotateTransform(-angle));
                    //        {
                    //            dc.DrawLine(new Pen(new SolidColorBrush(Colors.Red), 5), new Point(-20, 0), new Point(20, 0));
                    //        }
                    //        dc.Pop();
                    //    }
                    //    dc.Pop();
                    //}
                }
            }
        }

        private Vector ToVector(TelemetryContainer data1, TelemetryContainer data2)
        {
            return new Vector(data2.Graphics.CarCoordinates[0] - data1.Graphics.CarCoordinates[0], data2.Graphics.CarCoordinates[2] - data1.Graphics.CarCoordinates[2]);
        }

        private Color GetColor(int idx)
        {
            int nbPointsBefore = 20;
            int nbPointsAfter = 20;
            bool alternatesector = data.Data[idx].Graphics.CurrentSectorIndex % 2 != 0;

            List<Vector> vectors = new List<Vector>();

            int startidx = Math.Max(1, idx - nbPointsBefore);
            int endidx = Math.Min(data.Data.Count - 1, idx + nbPointsAfter);

            //for (int i = startidx; i < endidx; i++)
            //{
            //    vectors.Add(ToVector(data.Data[i - 1], data.Data[i]));
            //}

            //var total = 0d;
            //var count = 0d;
            //for (int i = 1; i < vectors.Count; i++)
            //{
            //    total += Vector.AngleBetween(vectors[i - 1], vectors[i]);
            //    count++;
            //}

            //var avgAngle = Math.Abs(total / count);

            var avgAngle = Math.Abs(Vector.AngleBetween(ToVector(data.Data[startidx], data.Data[idx]), ToVector(data.Data[idx], data.Data[endidx])));
            if (avgAngle > TurnAngleThreshold && HighlightTurns)
            {
                return TurnColor;
            }
            else
            {
                return alternatesector ? AlternateSectorColor : TrackColor;
            }
        }

        private void DrawOuterLoopClose(DrawingContext dc)
        {
            {
                dc.DrawLine(
                     new Pen(new SolidColorBrush(TrackBorderColor), (InnerPathWidth + OuterPathWidth) * 2),
                         toScaledPoint(data.Data[data.Data.Count - CloseLoopPoints - 1].Graphics.CarCoordinates),
                         toScaledPoint(data.Data[0].Graphics.CarCoordinates)
                     );
            }

            {
                dc.DrawLine(
                    new Pen(new SolidColorBrush(TrackBorderColor), (InnerPathWidth + OuterPathWidth) * 2),
                        toScaledPoint(data.Data[0].Graphics.CarCoordinates),
                        toScaledPoint(data.Data[CloseLoopPoints + 1].Graphics.CarCoordinates)
                    );
            }
            dc.DrawEllipse(new SolidColorBrush(TrackBorderColor), null, toScaledPoint(data.Data[0].Graphics.CarCoordinates), OuterPathWidth + InnerPathWidth, OuterPathWidth + InnerPathWidth);
        }

        private void DrawOuterPath(DrawingContext dc)
        {
            for (int i = 1 + CloseLoopPoints; i < data.Data.Count - CloseLoopPoints; i++)
            {
                dc.DrawEllipse(new SolidColorBrush(TrackBorderColor), null, toScaledPoint(data.Data[i].Graphics.CarCoordinates), OuterPathWidth + InnerPathWidth, OuterPathWidth + InnerPathWidth);
                if (i > 0)
                {
                    // Line between points
                    dc.DrawLine(
                        new Pen(new SolidColorBrush(TrackBorderColor), (InnerPathWidth + OuterPathWidth) * 2),
                            toScaledPoint(data.Data[i].Graphics.CarCoordinates),
                            toScaledPoint(data.Data[i - 1].Graphics.CarCoordinates)
                    );
                }
            }
        }

        public override int GetTrackLenght()
        {
            return (int)data.Data.Max(i => i.Graphics.DistanceTraveled) - (int)data.Data.Min(i => i.Graphics.DistanceTraveled);
        }
    }
}