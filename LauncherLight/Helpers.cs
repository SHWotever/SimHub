using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using IniParser;
using IniParser.Model;
using LauncherLight.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LauncherLight
{
    public class Helpers
    {
        public static string CutSuffix(string name, string suffixe)
        {
            return name.Substring(0, name.Length - ("_" + suffixe).Length);
        }

        public static bool ContainsSuffix(string name, string suffixe)
        {
            return name.EndsWith("_" + suffixe, StringComparison.InvariantCultureIgnoreCase);
        }

        public static void MergeIniFiles(string src, string target)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var srcdata = parser.ReadFile(src);
            var targetdata = parser.ReadFile(target);

            foreach (var section in srcdata.Sections)
            {
                // Remove
                if (ContainsSuffix(section.SectionName, "REMOVE"))
                {
                    var sectionName = CutSuffix(section.SectionName, "REMOVE");
                    if (targetdata.Sections.ContainsSection(sectionName))
                    {
                        targetdata.Sections.RemoveSection(sectionName);
                    }
                }

                // Replace
                else if (ContainsSuffix(section.SectionName, "REPLACE"))
                {
                    var sectionName = CutSuffix(section.SectionName, "REPLACE");

                    // Delete
                    if (targetdata.Sections.ContainsSection(sectionName))
                    {
                        targetdata.Sections.RemoveSection(sectionName);
                    }

                    // Copy
                    targetdata.Sections.AddSection(sectionName);
                    foreach (var key in section.Keys)
                    {
                        targetdata.Sections[sectionName].AddKey(key.KeyName, key.Value);
                    }
                }

                // Merge
                else //if (containssuffix(section.SectionName, "MERGE"))
                {
                    string sectionName = string.Empty;
                    if (ContainsSuffix(section.SectionName, "MERGE"))
                    {
                        sectionName = CutSuffix(section.SectionName, "MERGE");
                    }
                    else
                    {
                        sectionName = section.SectionName;
                    }

                    // Create of missing
                    if (!targetdata.Sections.ContainsSection(sectionName))
                    {
                        targetdata.Sections.AddSection(sectionName);
                    }

                    foreach (var key in section.Keys)
                    {
                        if (!targetdata.Sections[sectionName].ContainsKey(key.KeyName))
                        {
                            targetdata.Sections[sectionName].AddKey(key.KeyName, key.Value);
                        }
                        else
                        {
                            targetdata.Sections[sectionName][key.KeyName] = key.Value;
                        }
                    }
                }
            }

            CleanKeys(targetdata);

#pragma warning disable CS0618 // Type or member is obsolete
            parser.SaveFile(target, targetdata);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private static void CleanKeys(IniData targetdata)
        {
            foreach (var section in targetdata.Sections)
            {
                foreach (var key in section.Keys.ToList())
                {
                    if ((key.Value ?? "").ToLower() == "REMOVE")
                    {
                        section.Keys.RemoveKey(key.KeyName);
                    }
                }
            }
        }

        public static List<LLTrackDesc> GetTracks(string gamePath)
        {
            List<LLTrackDesc> tracks = new List<LLTrackDesc>();
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
                                    tracks.Add(new LLTrackDesc(gamePath, System.IO.Path.GetFileName(track), System.IO.Path.GetFileName(config)));
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            try
                            {
                                tracks.Add(new LLTrackDesc(gamePath, System.IO.Path.GetFileName(track), null));
                            }
                            catch { }
                        }
                    }
            }
            catch { }
            return tracks;
        }

        public static List<LLCarDesc> GetCars(string gamePath)
        {
            List<LLCarDesc> cars = new List<LLCarDesc>();
            try
            {
                if (System.IO.Directory.Exists(System.IO.Path.Combine(gamePath, "content", "cars")))
                    foreach (var car in System.IO.Directory.GetDirectories(System.IO.Path.Combine(gamePath, "content", "cars")))
                    {
                        var card = new LLCarDesc(gamePath, System.IO.Path.GetFileName(car));
                        cars.Add(card);
                    }
            }
            catch { }
            return cars;
        }

        private static Thread RefreshServerThread = null;

        public static void AbortRefresh()
        {
            if (RefreshServerThread != null)
            {
                RefreshServerThread.Abort();
            }
            RefreshServerThread = null;
        }

        public static void RefreshServers(IEnumerable<ACServer> server, string steamID)
        {
            AbortRefresh();

            RefreshServerThread = new Thread(delegate()
              {
                  Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                  var tmpservers = server.OrderByDescending(i => i.cars.Count > 1 && i.pickup).Where(i => i.pickup);
                  Parallel.ForEach(tmpservers, new ParallelOptions { MaxDegreeOfParallelism = 3 }, s =>
                  {
                      RefreshServer(s, steamID, false);
                  });

                  //foreach (var s in tmpservers)
                  //{
                  //    RefreshServer(s, steamID);
                  //}
              });
            RefreshServerThread.Start();
        }

        public static void RefreshServer(ACServer server, string steamID, bool force)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(
                    string.Format("http://{0}:{1}/INFO", server.ip, server.cport));
                req.Timeout = 10000;
                req.Headers.Clear();
                req.UserAgent = "Assetto Corsa Launcher";
                var response = req.GetResponse();
                StreamReader streamRead = new StreamReader(response.GetResponseStream());
                string res = streamRead.ReadToEnd();
                var infos = Newtonsoft.Json.JsonConvert.DeserializeObject<ACServer>(res);

                if (infos.pickup && server.ServerCars != null)
                {
                    var entries = GetServerEntries(server, steamID);

                    foreach (var car in server.ServerCars)
                    {
                        car.TotalEntries = entries.Count(i => i.Model == car.CarDesc.Model);
                        car.ConnectedEntries = entries.Count(i => i.Model == car.CarDesc.Model && i.IsConnected == true);
                        car.IsPickup = true;
                        car.IsFull = car.ConnectedEntries == car.TotalEntries;
                    }
                }
                else if (server.ServerCars != null)
                {
                    foreach (var car in server.ServerCars)
                    {
                        car.IsPickup = false;
                        car.IsFull = false;
                    }
                }
                //server.ip                =infos.ip         ;
                server.port = infos.port;
                server.cport = infos.cport;
                server.tport = infos.tport;
                server.name = infos.name;
                server.clients = infos.clients;
                server.maxclients = infos.maxclients;
                server.track = infos.track;
                server.cars = infos.cars;
                server.timeofday = infos.timeofday;
                server.session = infos.session;
                server.sessiontypes = infos.sessiontypes;
                server.durations = infos.durations;
                server.timeleft = infos.timeleft;
                server.country = infos.country;
                server.pass = infos.pass;
                server.pickup = infos.pickup;
                server.timestamp = infos.timestamp;
                server.lastupdate = infos.lastupdate;
                server.l = infos.l;
                server.LastRefresh = DateTime.Now;
                server.Unreachable = false;
                server.fill(force);
            }
            catch
            {
                server.Unreachable = true;
            }
        }

        private static List<EntryItem> GetServerEntries(ACServer server, string steamID)
        {
            var req = (HttpWebRequest)WebRequest.Create(
                string.Format("http://{0}:{1}/JSON|12345678", server.ip, server.cport));
            req.Timeout = 10000;
            req.Headers.Clear();
            req.UserAgent = "Assetto Corsa Launcher";
            var response = req.GetResponse();
            StreamReader streamRead = new StreamReader(response.GetResponseStream());
            string res = streamRead.ReadToEnd();
            var infos = Newtonsoft.Json.JsonConvert.DeserializeObject<EntryList>(res);
            return infos.Cars;
        }

        public static List<ACServer> GetAllServers(string steamID)
        {
            List<ACServer> onlineservers = new List<ACServer>();
            List<ACServer> lanservers = new List<ACServer>();

            var a = Task.Factory.StartNew(() =>
            {
                onlineservers = GetOnlineServers(steamID);
            });

            var b = Task.Factory.StartNew(() =>
            {
                lanservers = GetLanServers(steamID);
            });

            Task.WaitAll(new Task[] { a, b });

            return onlineservers.Concat(lanservers).OrderBy(i => i.name).ToList();
        }

        public static List<ACServer> GetOnlineServers(string steamID)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(
                    string.Format("http://93.57.10.21/lobby.ashx/list?guid={0}", steamID));
                req.Headers.Clear();
                req.UserAgent = "Assetto Corsa Launcher";
                var response = req.GetResponse();
                StreamReader streamRead = new StreamReader(response.GetResponseStream());
                string res = streamRead.ReadToEnd();

                try
                {
                    var servers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ACServer>>(res);
                    foreach (var i in servers)
                        i.fill(false);

                    return servers;
                }
                catch
                {
                    return new List<ACServer>();
                }
            }
            catch
            {
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

        private static IPAddress GetSubnetMask(IPAddress address)
        {
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < allNetworkInterfaces.Length; i++)
            {
                NetworkInterface networkInterface = allNetworkInterfaces[i];
                foreach (UnicastIPAddressInformation current in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (current.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(current.Address))
                        {
                            return current.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

        // AC.Launcher.ACRequestHandler
        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] addressBytes = address.GetAddressBytes();
            byte[] addressBytes2 = subnetMask.GetAddressBytes();
            if (addressBytes.Length != addressBytes2.Length)
            {
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
            }
            byte[] array = new byte[addressBytes.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(addressBytes[i] | (addressBytes2[i] ^ 255));
            }
            return new IPAddress(array);
        }

        // AC.Launcher.ACRequestHandler
        private static string BCastPing(IPAddress bcastaddress, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint remoteEP = new IPEndPoint(bcastaddress, port);
            socket.Blocking = false;
            socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.Debug, true);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint endPoint = iPEndPoint;
            byte[] bytes = BitConverter.GetBytes(200);
            byte[] array = new byte[256];
            try
            {
                socket.SendTo(bytes, SocketFlags.DontRoute, remoteEP);
                if (socket.Poll(50000, SelectMode.SelectRead))
                {
                    socket.ReceiveFrom(array, ref endPoint);
                }
            }
            catch (SocketException)
            {
            }
            socket.Close();
            int[] array2 = new int[] { -1, -1 };
            byte value = array[0];
            byte b = array[1];
            byte b2 = array[2];
            if (Convert.ToInt32(value) == 200 && b + b2 > 0)
            {
                array2[0] = port;
                array2[1] = (int)BitConverter.ToInt16(array, 1);
            }
            return endPoint.ToString() + ":" + array2[1];
        }

        public static List<ACServer> GetLanServers(string steamID)
        {
            List<ACServer> servers = new List<ACServer>();
            int num = 9000;
            int num2 = 10000;

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addressList = hostEntry.AddressList;
            for (int j = 0; j < addressList.Length; j++)
            {
                IPAddress ip = addressList[j];
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddress subnetMask = GetSubnetMask(ip);
                    IPAddress bcast = GetBroadcastAddress(ip, subnetMask);

                    Parallel.For(num, num2, delegate(int i, ParallelLoopState state)
                    {
                        string text = BCastPing(bcast, i);
                        if (!text.EndsWith("-1"))
                        {
                            string[] array = text.Split(new char[] { ':' });

                            var Server = new ACServer { ip = ip.ToString(), cport = int.Parse(array[2]), Lan = true };
                            servers.Add(Server);
                        }
                    });
                }
            }
            foreach (var s in servers)
            {
                RefreshServer(s, steamID, false);
            }
            return servers;
        }
    }
}