using System;
using System.Runtime.InteropServices;

namespace ACSharedMemory
{
    public class StaticInfoEventArgs : EventArgs
    {
        public StaticInfoEventArgs(StaticInfo staticInfo)
        {
            this.StaticInfo = staticInfo;
        }

        public StaticInfo StaticInfo { get; private set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    [Serializable]
    public struct StaticInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String SMVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String ACVersion;

        // session static info
        public int NumberOfSessions;

        public int NumCars;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String CarModel;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String Track;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerSurname;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerNick;

        public int SectorCount;

        // car static info
        public float MaxTorque;

        public float MaxPower;
        public int MaxRpm;
        public float MaxFuel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] SuspensionMaxTravel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] TyreRadius;
    }
}