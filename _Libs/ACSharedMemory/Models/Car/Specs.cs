using PropertyChanged;

namespace ACSharedMemory.Models.Car
{
    [ImplementPropertyChanged]
    public class Specs
    {
        public string bhp { get; set; }

        public string torque { get; set; }

        public string weight { get; set; }

        public string topspeed { get; set; }

        public string acceleration { get; set; }

        public string pwratio { get; set; }
    }
}