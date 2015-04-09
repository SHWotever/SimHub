using System.Collections.Generic;

namespace TimingClient.Plugins
{
    public class Mapping
    {
        public string Trigger;
        public string Target;

        public override string ToString()
        {
            return string.Format("When [{0}] then [{1}]", Trigger, Target);
        }
    }

    public class PluginManagerSettings
    {
        public PluginManagerSettings()
        {
            this.InputActionMapping = new List<Mapping>();
            this.EventActionMapping = new List<Mapping>();
        }

        public List<Mapping> InputActionMapping { get; set; }

        public List<Mapping> EventActionMapping { get; set; }
    }
}