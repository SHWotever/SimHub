using System;

namespace ACSharedMemory.Reader
{
    [Serializable]
    public class ACData 
    {
        public Graphics Graphics { get; set; }

        public Physics Physics { get; set; }

        public StaticInfo StaticInfo { get; set; }

        public TimeSpan CurrentLapTime{ get; set; }

        public TimeSpan LastLapTime { get; set; }

        public TimeSpan BestLapTime { get; set; }

        public TimeSpan SessionTimeLeft { get; set; }

        public TimeSpan LastSectorTime { get; set; }

        public string Gear { get; internal set; }

        public float SpeedMph { get; internal set; }

        public string SessionTypeName { get; internal set; }
    }
}