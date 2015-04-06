using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Timers;

namespace ACSharedMemory.Reader
{
    //public delegate void PhysicsUpdatedHandler(object sender, PhysicsEventArgs e);
    //public delegate void GraphicsUpdatedHandler(object sender, GraphicsEventArgs e);
    //public delegate void StaticInfoUpdatedHandler(object sender, StaticInfoEventArgs e);
    //public delegate void GameStatusChangedHandler(object sender, GameStatusEventArgs e);

    internal class AssettoCorsaNotStartedException : Exception
    {
        public AssettoCorsaNotStartedException()
            : base("Shared Memory not connected, is Assetto Corsa running and have you run assettoCorsa.Start()?")
        {
        }
    }

    internal enum AC_MEMORY_STATUS { DISCONNECTED, CONNECTING, CONNECTED }

    public class ACReader
    {
        private Timer sharedMemoryRetryTimer;
        private AC_MEMORY_STATUS memoryStatus = AC_MEMORY_STATUS.DISCONNECTED;

        public bool IsRunning { get { return (memoryStatus == AC_MEMORY_STATUS.CONNECTED); } }

        public ACReader()
        {
            sharedMemoryRetryTimer = new Timer(1000);
            sharedMemoryRetryTimer.AutoReset = true;
            sharedMemoryRetryTimer.Elapsed += sharedMemoryRetryTimer_Elapsed;
            Stop();
        }

        /// <summary>
        /// Connect to the shared memory and start the update timers
        /// </summary>
        public void Start()
        {
            sharedMemoryRetryTimer.Start();
        }

        private void sharedMemoryRetryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ConnectToSharedMemory();
        }

        [DebuggerNonUserCode]
        private bool ConnectToSharedMemory()
        {
            try
            {
                memoryStatus = AC_MEMORY_STATUS.CONNECTING;
                // Connect to shared memory
                physicsMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_physics");
                graphicsMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_graphics");
                staticInfoMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_static");

                // Stop retry timer
                sharedMemoryRetryTimer.Stop();
                memoryStatus = AC_MEMORY_STATUS.CONNECTED;
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Stop the timers and dispose of the shared memory handles
        /// </summary>
        public void Stop()
        {
            memoryStatus = AC_MEMORY_STATUS.DISCONNECTED;
            sharedMemoryRetryTimer.Stop();
        }

        private MemoryMappedFile physicsMMF;
        private MemoryMappedFile graphicsMMF;
        private MemoryMappedFile staticInfoMMF;

        /// <summary>
        /// Read the current physics data from shared memory
        /// </summary>
        /// <returns>A Physics object representing the current status, or null if not available</returns>
        public Physics ReadPhysics()
        {
            return ReadMemoryMappedFile<Physics>(physicsMMF);
        }

        public Graphics ReadGraphics()
        {
            return ReadMemoryMappedFile<Graphics>(graphicsMMF);
        }

        public StaticInfo ReadStaticInfo()
        {
            return ReadMemoryMappedFile<StaticInfo>(staticInfoMMF);
        }

        public ACData GetData()
        {
            return new ACData { StaticInfo = ReadStaticInfo(), Physics = ReadPhysics(), Graphics = ReadGraphics() };
        }

        private T ReadMemoryMappedFile<T>(MemoryMappedFile MMF)
        {
            if (memoryStatus == AC_MEMORY_STATUS.DISCONNECTED || MMF == null)
                throw new AssettoCorsaNotStartedException();

            using (var stream = MMF.CreateViewStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    var size = Marshal.SizeOf(typeof(T));
                    var bytes = reader.ReadBytes(size);
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    var data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                    handle.Free();
                    return data;
                }
            }
        }
    }
}

;