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

        public float TyreWearAvg { get; internal set; }

        public float TyreWearMin { get; internal set; }

        public float TyreWearMax { get; internal set; }

        public float TyreDirtyLevel1 { get; internal set; }

        public float TyreDirtyLevel2 { get; internal set; }

        public float TyreDirtyLevel3 { get; internal set; }

        public float TyreDirtyLevel4 { get; internal set; }

        public float TyreDirtyLevelAvg { get; internal set; }

        public float TyreDirtyLevelMin { get; internal set; }

        public float TyreDirtyLevelMax { get; internal set; }

        public float TyreCoreTemperature1 { get; internal set; }

        public float TyreCoreTemperature2 { get; internal set; }

        public float TyreCoreTemperature3 { get; internal set; }

        public float TyreCoreTemperature4 { get; internal set; }

        public float TyreCoreTemperatureAvg { get; internal set; }

        public float TyreCoreTemperatureMin { get; internal set; }

        public float TyreCoreTemperatureMax { get; internal set; }

        public float CarDamage1 { get; internal set; }

        public float CarDamage2 { get; internal set; }

        public float CarDamage3 { get; internal set; }

        public float CarDamage4 { get; internal set; }

        public float CarDamage5 { get; internal set; }

        public float CarDamageAvg { get; internal set; }

        public float CarDamageMin { get; internal set; }

        public float CarDamageMax { get; internal set; }
    }
}