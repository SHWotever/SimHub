Assetto Corsa Shared Memory Library
===================================

Assetto Corsa Shared Memory library written in C# to access live game data

The code was built around the shared memory structures described here on the official Assetto Corsa forum:
http://www.assettocorsa.net/forum/index.php?threads/shared-memory-reference.3352/


class: AssettoCorsa
-------------------

This is the centerpiece of the library. Using this you can add your own event listeners to trigger for updates.

There are three events to listen for:

* AssettoCorsa.StaticInfoUpdated
* AssettoCorsa.GraphicsUpdated
* AssettoCorsa.PhysicsUpdated

These events have individual timers and their respective update intervals can be changed to fit your own needs. Timers will not be running if an event has no listeners.

The default update intervals are:

```
AssettoCorsa.SharedInfoInterval: 3000 ms
AssettoCorsa.GraphicsInterval: 10000 ms
AssettoCorsa.PhysicsInterval: 10 ms
```

The `AssettoCorsa.Start()` and `AssettoCorsa.Stop()` functions are to connect and disconnect from the shared memory and also to start and stop the timers for the events. After you've executed `Start()` you can use `IsRunning` to check if it successfully connected to the shared memory.

Usage Example
-------------

In Visual Studio you have two easy options:

1. Add the AssettoCorsaSharedMemory.dll as a reference to your project
2. Add the complete AssettoCorsaSharedMemory project to your solution and then add it as a reference

Here is some example code to read the StaticInfo and output it to the console:

```c#
using AssettoCorsaSharedMemory;
using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AssettoCorsa ac = new AssettoCorsa();
            ac.StaticInfoInterval = 5000; // Get StaticInfo updates ever 5 seconds
            ac.StaticInfoUpdated += ac_StaticInfoUpdated; // Add event listener for StaticInfo
            ac.Start(); // Connect to shared memory and start interval timers 

            Console.ReadKey();
        }

        static void ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {
            // Print out some data from StaticInfo
            Console.WriteLine("StaticInfo");
            Console.WriteLine("  Car Model: " + e.StaticInfo.CarModel);
            Console.WriteLine("  Track:     " + e.StaticInfo.Track);
            Console.WriteLine("  Max RPM:   " + e.StaticInfo.MaxRpm);
        }
    }
}
```
