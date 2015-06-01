using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherLight.Models
{
    public class ACServerJson
    {

        public string ip { get; set; }
        public int port { get; set; }
        public int cport { get; set; }
        public int tport { get; set; }
        public string name { get; set; }
        public int clients { get; set; }
        public int maxclients { get; set; }
        public string track { get; set; }
        public List<string> cars { get; set; }
        public int timeofday { get; set; }
        public int session { get; set; }
        public List<string> sessiontypes { get; set; }
        public List<string> durations { get; set; }
        public long timeleft { get; set; }
        public List<string> country { get; set; }
        public bool pass { get; set; }
        public bool pickup { get; set; }
        public int timestamp { get; set; }
        public int lastupdate { get; set; }
        public bool l { get; set; }

    }


    public class ACServer : ACServerJson
    {
        private List<CarDesc> _cars = null;

        private TrackDesc _track = null;
        public TrackDesc TrackDesc
        {
            get
            {
                if (_track == null)
                {
                    try
                    {
                        if (!track.Contains("-") || Directory.Exists(Path.Combine(Properties.Settings.Default.GamePath, "content", "tracks", track)))
                        {
                            _track = TrackDesc.FromName(Properties.Settings.Default.GamePath, track, null);
                        }
                        else
                        {
                            var trackparts = track.Split('-');
                            _track = TrackDesc.FromName(Properties.Settings.Default.GamePath, trackparts[0], trackparts[1]);
                        }
                    }
                    catch
                    {
                        _track = new TrackDesc() { Track = track, TrackInfo = new TrackInfo() };
                    }
                }
                return _track;
            }

        }

        public List<CarDesc> CarDescs
        {
            get
            {
                if (_cars == null)
                {
                    _cars = cars.Select(i => CarDesc.FromModel(Properties.Settings.Default.GamePath, i)).ToList();
                }
                return _cars;
            }

        }


    }
}

