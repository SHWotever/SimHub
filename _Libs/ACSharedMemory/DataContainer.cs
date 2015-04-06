using ACSharedMemory.Models.Track;
using System;

namespace ACSharedMemory
{
    public class DataContainer
    {
        public Graphics Graphics { get; set; }

        public Physics Physics { get; set; }

        public StaticInfo StaticInfo { get; set; }

        public TimeSpan AllTimeDelta { get; set; }

        public TimeSpan MyTimeDelta { get; set; }

        public TimeSpan SessionTimeDelta { get; set; }

        public TimeSpan AllTimeBest { get; set; }

        public TimeSpan MyTimeBest { get; set; }

        public TimeSpan SessionTimeBest { get; set; }

        public bool GameRunning { get; set; }

        public TrackDesc TrackDesc { get; set; }
    }
}