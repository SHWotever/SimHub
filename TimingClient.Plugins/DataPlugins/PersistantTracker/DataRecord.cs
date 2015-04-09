﻿using System;
using System.Collections.Generic;

namespace TimingClient.Plugins.DataPlugins.PersistantTracker
{
    public class DataRecord
    {
        public DataRecord()
        {
            this.CarPositions = new List<KeyValuePair<TimeSpan, float>>();
            this.CarPositions.Add(new KeyValuePair<TimeSpan, float>(TimeSpan.FromSeconds(0), 0));
            this.LapId = Guid.NewGuid();
            this.SessionId = Guid.NewGuid();
        }

        public DateTime RecordDate { get; set; }

        public TimeSpan LapTime { get; set; }

        public int LapNumber { get; set; }

        public Guid LapId { get; set; }

        public Guid SessionId { get; set; }

        public List<KeyValuePair<TimeSpan, float>> CarPositions { get; set; }
    }
}