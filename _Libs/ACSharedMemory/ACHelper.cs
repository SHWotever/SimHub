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