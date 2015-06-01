using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using LauncherLight.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LauncherLight
{
    public class Helpers
    {
        public static  List<TrackDesc> GetTracks(string gamePath)
        {
            List<TrackDesc> tracks = new List<TrackDesc>();
            try
            {
                if (System.IO.Directory.Exists(System.IO.Path.Combine(gamePath, "content", "tracks")))
                    foreach (var track in System.IO.Directory.GetDirectories(System.IO.Path.Combine(gamePath, "content", "tracks")))
                    {
                        if (System.IO.Directory.GetDirectories(System.IO.Path.Combine(track, "ui")).Count() > 0)
                        {
                            foreach (var config in System.IO.Directory.GetDirectories(System.IO.Path.Combine(track, "ui")))
                            {
                                try
                                {
                                    tracks.Add(new TrackDesc(gamePath, System.IO.Path.GetFileName(track), System.IO.Path.GetFileName(config)));
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            try
                            {
                                tracks.Add(new TrackDesc(gamePath, System.IO.Path.GetFileName(track), null));
                            }
                            catch { }
                        }
                    }
            }
            catch { }
            return tracks;
        }
        public static List<CarDesc> GetCars(string gamePath)
        {
            List<CarDesc> cars = new List<CarDesc>();
            try
            {
                if (System.IO.Directory.Exists(System.IO.Path.Combine(gamePath, "content", "cars")))
                    foreach (var car in System.IO.Directory.GetDirectories(System.IO.Path.Combine(gamePath, "content", "cars")))
                    {
                        var card = CarDesc.FromModel(gamePath, System.IO.Path.GetFileName(car));
                        cars.Add(card);
                    }
            }
            catch { }
            return cars;
        }

        public static List<ACServer> GetOnlineServer(string steamID) {

            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("http://93.57.10.21/lobby.ashx/list?guid={0}",steamID));
            req.Headers.Clear();
            req.UserAgent = "Assetto Corsa Launcher";
            var response = req.GetResponse();
            StreamReader streamRead = new StreamReader(response.GetResponseStream());
            string res = streamRead.ReadToEnd();
            

            try{
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ACServer>>(res);
            }catch{
                return new List<ACServer>();
            }

        }

        public static List<Assist> GetStabilityAssists()
        {
            List<Assist> Stability = new List<Assist>();
            Stability.Add(new Assist { Text = "Off", Value = "0" });
            Stability.Add(new Assist { Text = "Allowed", Value = "1" });
            return Stability;
        }

        public static List<Assist> GetTCAssits()
        {
            List<Assist> TCassists = new List<Assist>();
            TCassists.Add(new Assist { Text = "Off", Value = "0" });
            TCassists.Add(new Assist { Text = "Factory", Value = "1" });
            TCassists.Add(new Assist { Text = "User defined", Value = "2" });
            return TCassists;
        }
    }
}
