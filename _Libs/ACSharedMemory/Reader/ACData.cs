using System;

namespace ACSharedMemory.Reader
{
    [Serializable]
    public class ACData
    {
        public Graphics Graphics { get; set; }

        public Physics Physics { get; set; }

        public StaticInfo StaticInfo { get; set; }

        public TimeSpan CurrentLapTime { get; set; }

        public TimeSpan LastLapTime { get; set; }

        public TimeSpan BestLapTime { get; set; }

        public TimeSpan SessionTimeLeft { get; set; }

        public TimeSpan LastSectorTime { get; set; }

        public string Gear { get; internal set; }

        public float SpeedMph { get; internal set; }

        public string SessionTypeName { get; internal set; }

        public string StatusName { get; internal set; }

        public float TyreWear1 { get; internal set; }

        public float TyreWear2 { get; internal set; }

        public float TyreWear3 { get; internal set; }

        public float TyreWear4 { get; internal set; }

        public float TyreDirtyLevel1 { get; internal set; }

        public float TyreDirtyLevel2 { get; internal set; }

        public float TyreDirtyLevel3 { get; internal set; }

        public float TyreDirtyLevel4 { get; internal set; }
    }
}