using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ACSharedMemory
{
    public class ACHelper
    {
        public static CarDesc GetCarData(StaticInfo info)
        {
            return CarDesc.FromModel(GetAsettoCorsaPath(), info.CarModel);
        }

        public static TrackDesc GetCurrentTrack(StaticInfo info)
        {
            return TrackDesc.GetFromGameSettings(GetAsettoCorsaPath());
        }

        public static CarDesc GetCarData(string ACPath, StaticInfo info)
        {
            return CarDesc.FromModel(ACPath, info.CarModel);
        }

        public static TrackDesc GetCurrentTrack(string ACPath, StaticInfo info)
        {
            return TrackDesc.GetFromGameSettings(ACPath);
        }

        public static bool IsProcessRunning()
        {
            return GetAsettoCorsaPath() != null;
        }

        [DebuggerNonUserCode]
        public static string GetAsettoCorsaPath()
        {
            try
            {
                var p = GetAsettoCorsaProcess();
                if (p != null)
                {
                    return System.IO.Path.GetDirectoryName(p.MainModule.FileName);
                }
            }
            catch { }
            return null;
        }

        [DebuggerNonUserCode]
        public static Process GetAsettoCorsaProcess()
        {
            return Process.GetProcessesByName("acs").FirstOrDefault();
        }

        public static TimeSpan GetLapDelta(Graphics g, List<KeyValuePair<TimeSpan, float>> lapData)
        {
            TimeSpan timedeltaResult = new TimeSpan();
            if (lapData != null && lapData.Count > 2)
            {
                var idx = 0;
                var currentTime = ToTimeSpan(g.iCurrentTime);
                for (int i = 0; i < lapData.Count; i++)
                {
                    if (lapData[i].Value >= g.NormalizedCarPosition)
                    {
                        idx = i;
                        break;
                    }
                }
                var bestLapTime = lapData[idx];

                // Interpolate
                if (idx > 0 && idx < lapData.Count - 1)
                {
                    var nextBestLapTime = lapData[idx - 1];
                    var estimatedSpeed = ((float)(nextBestLapTime.Value - bestLapTime.Value)) / ((float)((nextBestLapTime.Key - bestLapTime.Key).Milliseconds));
                    int additionalTime = (int)((g.NormalizedCarPosition - nextBestLapTime.Value) / estimatedSpeed);
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - (bestLapTime.Key - ToTimeSpan(additionalTime)));
                    timedeltaResult = timeDelta;
                }
                // Direct
                else
                {
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - bestLapTime.Key);
                    timedeltaResult = timeDelta;
                }
            }
            return timedeltaResult;
        }

        private static Double Distance(float[] a, float[] b)
        {
            return (double)Math.Sqrt(Math.Pow(Convert.ToDouble(b[0]) - Convert.ToDouble(a[0]), 2) + Math.Pow(Convert.ToDouble(b[1]) - Convert.ToDouble(a[1]), 2) + Math.Pow(Convert.ToDouble(b[2]) - Convert.ToDouble(a[2]), 2));
        }

        public static TimeSpan GetLapDeltaAlternative(Graphics g, List<KeyValuePair<TimeSpan, float[]>> lapData)
        {
            TimeSpan timedeltaResult = new TimeSpan();

            if (lapData != null && lapData.Count > 2)
            {
                var bestidx = 0;
                double bestidxdelta = 99999;

                var currentTime = ToTimeSpan(g.iCurrentTime);
                if (currentTime == TimeSpan.Zero)
                {
                    return TimeSpan.Zero;
                }

                for (int i = lapData.Count - 2; i >= 0; i--)
                {
                    var delta = Distance(lapData[i].Value, g.CarCoordinates);
                    delta += Distance(lapData[i + 1].Value, g.CarCoordinates);

                    if (delta <= bestidxdelta)
                    {
                        bestidx = i;
                        bestidxdelta = delta;
                    }
                }

                var bestLapTime = lapData[bestidx];
                var idx = bestidx;


                if (bestidx < lapData.Count - 2 && bestidx > 1)
                {

                    var pointA = lapData[bestidx];
                    var pointB = lapData[bestidx + 1];

                    var pointADistance = Distance(pointA.Value, g.CarCoordinates);
                    var pointBDistance = Distance(pointB.Value, g.CarCoordinates);




                    var referenceTimeMs = pointA.Key.TotalMilliseconds
                        + ((pointB.Key.TotalMilliseconds - pointA.Key.TotalMilliseconds) * pointADistance) / (pointADistance + pointBDistance);

                    var tmp = TimeSpan.FromMilliseconds(referenceTimeMs);

                    return (currentTime - TimeSpan.FromMilliseconds(referenceTimeMs));
                }
                else
                {
                    return (currentTime - lapData[bestidx].Key);
                }

            }
            return timedeltaResult;
        }

        public static TimeSpan GetLapDeltaAlternative(Graphics g, List<KeyValuePair<TimeSpan, float>> lapData)
        {
            TimeSpan timedeltaResult = new TimeSpan();
            if (lapData != null && lapData.Count > 2)
            {
                var bestidx = 0;
                float bestidxdelta = 999;

                var currentTime = ToTimeSpan(g.iCurrentTime);
                for (int i = 0; i < lapData.Count; i++)
                {
                    var delta = lapData[i].Value - g.NormalizedCarPosition;
                    if (delta > 0 && lapData[i].Value - g.NormalizedCarPosition < bestidxdelta)
                    {
                        bestidx = i;
                        bestidxdelta = delta;
                    }
                }
                var bestLapTime = lapData[bestidx];
                var idx = bestidx;


                // Interpolate
                if (idx > 0 && idx < lapData.Count - 1)
                {
                    var nextBestLapTime = lapData[idx - 1];
                    var estimatedSpeed = ((float)(nextBestLapTime.Value - bestLapTime.Value)) / ((float)((nextBestLapTime.Key - bestLapTime.Key).Milliseconds));
                    int additionalTime = (int)((g.NormalizedCarPosition - nextBestLapTime.Value) / estimatedSpeed);
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - (bestLapTime.Key - ToTimeSpan(additionalTime)));
                    timedeltaResult = timeDelta;
                }
                // Direct
                else
                {
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - bestLapTime.Key);
                    timedeltaResult = timeDelta;
                }
            }
            return timedeltaResult;
        }

        public static TimeSpan GetLapDeltaReverse(Graphics g, List<KeyValuePair<TimeSpan, float>> lapData)
        {
            TimeSpan timedeltaResult = new TimeSpan();
            if (lapData != null && lapData.Count > 2)
            {
                var idx = 0;
                var currentTime = ToTimeSpan(g.iCurrentTime);
                for (int i = 0; i <= lapData.Count; i++)
                {
                    if (lapData[i].Value <= g.NormalizedCarPosition)
                    {
                        idx = i;
                        break;
                    }
                }
                var bestLapTime = lapData[idx];

                // Interpolate
                if (idx > 0 && idx < lapData.Count - 1)
                {
                    var nextBestLapTime = lapData[idx - 1];
                    var estimatedSpeed = ((float)(nextBestLapTime.Value - bestLapTime.Value)) / ((float)((nextBestLapTime.Key - bestLapTime.Key).Milliseconds));
                    int additionalTime = (int)((g.NormalizedCarPosition - nextBestLapTime.Value) / estimatedSpeed);
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - (bestLapTime.Key + ToTimeSpan(additionalTime)));
                    timedeltaResult = timeDelta;
                }
                // Direct
                else
                {
                    var timeDelta = (ToTimeSpan(g.iCurrentTime) - bestLapTime.Key);
                    timedeltaResult = timeDelta;
                }
            }
            return timedeltaResult;
        }

        public static TimeSpan ToTimeSpan(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static string GetGear(int gear)
        {
            gear = gear - 1;
            if (gear == 0)

                return "N";
            else if (gear == -1)

                return "R";
            else

                return gear.ToString();
        }
    }
}