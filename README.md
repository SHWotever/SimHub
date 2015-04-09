# AssettoCorsaTools
Tools, MapGenerator, Arduino Dash

## ACManager

### How to use ACManager 

```csharp
  ACManager = new ACSharedMemory.ACManager();
  ACManager.SynchronizingObject = this;
  ACManager.Start();
```

### Exposed events 

```csharp
// Event when game is started or stopped
GameRunningChanged(bool running, ACManager manager);

// Event when session status is changed (ie, LIVE/PAUSE ...)
GameStatusChanged(AC_STATUS status, ACManager manager);

// Event when a new lap start, testlap notifies if previous lap has been taken by the game
NewLap(int completedLapNumber, bool testLap, ACManager manager);

// Event when session type change (ie PRACTIVE/HOTLAP ...)
SessionTypeChanged(AC_SESSION_TYPE sessionType, ACManager manager);

// Event when a session restart is detected
SessionRestart(ACManager manager);

// Event when car change
CarChanged(CarDesc newCar, ACManager manager);

// Event when track change
TrackChanged(TrackDesc newTrack, ACManager manager);

// Event when data is updated
DataUpdated(GameData data, ACManager manager);
```
