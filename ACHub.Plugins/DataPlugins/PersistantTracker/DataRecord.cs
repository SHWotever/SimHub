using System;
using System.Collections.Generic;

namespace ACHub.Plugins.DataPlugins.PersistantTracker
{
    /// <summary>
    /// Lap record 
    /// </summary>
    public class DataRecord
    {
        /// <summary>
        /// CTOr
        /// </summary>
        public DataRecord()
        {
            this.CarPositions = new List<KeyValuePair<TimeSpan, float>>();
            this.CarPositions.Add(new KeyValuePair<TimeSpan, float>(TimeSpan.FromSeconds(0), 0));
            this.LapId = Guid.NewGuid();
            this.SessionId = Guid.NewGuid();
            this.SectorsTime = new Dictionary<int, TimeSpan>();
            this.CarCoordinates = new List<KeyValuePair<TimeSpan, float[]>>();
        }

        /// <summary>
        /// Record date
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Lap time 
        /// </summary>
        public TimeSpan LapTime { get; set; }

        /// <summary>
        /// Lap number
        /// </summary>
        public int LapNumber { get; set; }

        /// <summary>
        /// Lap Id
        /// </summary>
        public Guid LapId { get; set; }

        /// <summary>
        /// Session Id
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Positions 
        /// </summary>
        public List<KeyValuePair<TimeSpan, float>> CarPositions { get; set; }

        /// <summary>
        /// Car coordinates
        /// </summary>
        public List<KeyValuePair<TimeSpan, float[]>> CarCoordinates { get; set; }

        /// <summary>
        /// Sectors time
        /// </summary>
        public Dictionary<int, TimeSpan> SectorsTime{ get; set; }
    }
}