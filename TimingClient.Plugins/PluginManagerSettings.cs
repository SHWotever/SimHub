using System;
using System.Collections.Generic;

namespace TimingClient.Plugins
{
    /// <summary>
    /// Mapping class
    /// </summary>
    [Serializable]
    public class Mapping
    {
        /// <summary>
        /// Mapping trigger
        /// </summary>
        public string Trigger { get; set; }

        /// <summary>
        /// Mapping Target
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Retrurs description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("When [{0}] then [{1}]", Trigger, Target);
        }
    }

    public enum PressType
    {
        ShortPress = 1,
        LongPress = 2,
        During = 3, 
    }

    public class InputMapping : Mapping
    {

        public InputMapping()
        {
            PressType = PressType.ShortPress;
        }

        public PressType PressType { get; set; }

        /// <summary>
        /// Retrurs description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (PressType)
            {
                case Plugins.PressType.ShortPress:
                    return string.Format("When [{0}] pressed then [{1}]", Trigger, Target);
                case Plugins.PressType.LongPress:
                    return string.Format("When [{0}] long pressed then [{1}]", Trigger, Target);
                case Plugins.PressType.During:
                    return string.Format("While [{0}] is pressed then [{1}]", Trigger, Target);
                default:
                    return "";
            }
        }

    }

    /// <summary>
    /// Settings class
    /// </summary>
    [Serializable]
    public class PluginManagerSettings
    {
        /// <summary>
        /// CTor
        /// </summary>
        public PluginManagerSettings()
        {
            this.InputActionMapping = new List<InputMapping>();
            this.EventActionMapping = new List<Mapping>();
        }

        /// <summary>
        /// Input to action mappings
        /// </summary>
        public List<InputMapping> InputActionMapping { get; set; }

        /// <summary>
        /// Events to action mappings
        /// </summary>
        public List<Mapping> EventActionMapping { get; set; }
    }
}