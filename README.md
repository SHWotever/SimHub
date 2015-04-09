# AssettoCorsaTools
Tools, MapGenerator, Arduino Dash

## ACManager

ACManager is an event oriented class for reading Assetto caorsa chared memory, Additional data is added by reading content files of the game

In addition of shared memory data you can access to :
  - Track desctiption
    - Min map data
    - Current track + configuration ....
  - Car description
    - badge, preview image ... 

### How to use ACManager 

```csharp
  var acManager = new ACSharedMemory.ACManager();
  ACManager.SynchronizingObject = this;
  ACManager.Start();
```

### Exposed events 

```csharp
// Event when game is started or stopped
acManager.GameRunningChanged(bool running, ACManager manager);

// Event when session status is changed (ie, LIVE/PAUSE ...)
acManager.GameStatusChanged(AC_STATUS status, ACManager manager);

// Event when a new lap start, testlap notifies if previous lap has been taken by the game
acManager.NewLap(int completedLapNumber, bool testLap, ACManager manager);

// Event when session type change (ie PRACTIVE/HOTLAP ...)
acManager.SessionTypeChanged(AC_SESSION_TYPE sessionType, ACManager manager);

// Event when a session restart is detected
acManager.SessionRestart(ACManager manager);

// Event when car change
acManager.CarChanged(CarDesc newCar, ACManager manager);

// Event when track change
acManager.TrackChanged(TrackDesc newTrack, ACManager manager);

// Event when data is updated
acManager.DataUpdated(GameData data, ACManager manager);
```
