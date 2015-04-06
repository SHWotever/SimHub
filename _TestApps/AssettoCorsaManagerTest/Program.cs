using ACSharedMemory;
using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using System;

namespace AssettoCorsaManagerTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ACManager manager = new ACManager();
            manager.GameStateChanged += manager_GameStateChanged;
            manager.GameStatusChanged += manager_GameStatusChanged;
            manager.SessionRestart += manager_SessionRestart;
            manager.NewLap += manager_NewLap;
            manager.SessionTypeChanged += manager_SessionTypeChanged;
            manager.CarChanged += manager_CarChanged;
            manager.TrackChanged += manager_TrackChanged;
            manager.DataUpdated += manager_DataUpdated;
            manager.Enabled = true;
            Console.ReadLine();
        }

        static void manager_DataUpdated(GameData data, ACManager manager)
        {
            Console.WriteLine("DataUpdated()");
        }

        static void manager_TrackChanged(TrackDesc newTrack, ACManager manager)
        {
            Console.WriteLine("TrackChanged(newTrack=" + (newTrack == null ? "null" : newTrack.TrackCode) + ")");
        }

        private static void manager_CarChanged(CarDesc newCar, ACManager manager)
        {
            Console.WriteLine("CarChanged(newCar=" + (newCar == null ? "null" : newCar.Model) + ")");
        }

        private static void manager_SessionTypeChanged(AC_SESSION_TYPE sessionType, ACManager manager)
        {
            Console.WriteLine("SessionTypeChanged(sessionType=" + sessionType.ToString() + ")");
        }

        private static void manager_SessionRestart(ACManager manager)
        {
            Console.WriteLine("SessionRestart()");
        }

        private static void manager_NewLap(int completedLapNumber, bool testLap, ACManager manager)
        {
            Console.WriteLine("NewLap(completedLapNumber=" + completedLapNumber.ToString() + ", bool testLap=" + testLap.ToString() + ")");
        }

        private static void manager_GameStatusChanged(AC_STATUS status, ACManager manager)
        {
            Console.WriteLine("GameStatusChanged(status=" + status.ToString() + ")");
        }

        private static void manager_GameStateChanged(bool running, ACManager manager)
        {
            Console.WriteLine("GameStateChanged(running=" + running.ToString() + ")");
        }
    }
}