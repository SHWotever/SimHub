using System.Collections.Generic;

namespace ACHub.Plugins.DataPlugins.DataCore
{
    /// <summary>
    /// Lap fuel statistics object
    /// </summary>
    public class FuelStatistics
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public FuelStatistics()
        {
            Consumption = new List<float>();
        }

        /// <summary>
        /// Consuption history
        /// </summary>
        public List<float> Consumption { get; set; }
    }
}