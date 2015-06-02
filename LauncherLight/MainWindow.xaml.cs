using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using IniParser;
using IniParser.Model;
using LauncherLight.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LauncherLight
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        //private string GamePath = @"D:\Program Files (x86)\Assetto Corsa";
        //private string ServerPath = @"D:\Program Files (x86)\Assetto Corsa\Server";
        public MainWindow()
        {
            InitializeComponent();
            this.lstCars.SelectionChanged += lstCars_SelectionChanged;
            this.lstServers.SelectionChanged += lstServer_SelectionChanged;
            Task.Factory.StartNew(() =>
            {
                LoadData();
            //    this.Dispatcher.Invoke(() =>
            //                        {
            //                            for (var i = 0; i < tab.Items.Count; i++)
            //                            {

            //                                tab.SelectedIndex = i;

            //                                tab.UpdateLayout();


            //                            }
            //                            tab.SelectedIndex = 0;
            //                        });
            });
        }

        void lstServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstServers.SelectedItem != null)
            {
                lstOnlineCars.DataContext = (lstServers.SelectedItem as ACServer).CarDescs.ToList();
            }
        }
        void lstCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCars.SelectedItem != null)
            {
                lstSkins.DataContext = (lstCars.SelectedItem as CarDesc).Skins.ToList();
                lstSkins.SelectedValue = (lstCars.SelectedItem as CarDesc).Skins.First();
            }
        }

        private string GamePath { get { return Properties.Settings.Default.GamePath; } }

        private string ServerPath { get { return Properties.Settings.Default.ServerPath; } }

        private void LoadData()
        {
            List<Assist> TCassists = Helpers.GetTCAssits();

            List<Assist> Stability = Helpers.GetStabilityAssists();
            List<CarDesc> cars = Helpers.GetCars(GamePath);
            List<TrackDesc> tracks = Helpers.GetTracks(GamePath);

            cars = cars.OrderBy(i => i.CarInfo.brand).ThenBy(i => i.CarInfo.name).ToList();
            tracks = tracks.OrderBy(i => i.TrackInfo.name).ToList();
            var servers = Helpers.GetOnlineServer("76561197987605830");


            Dispatcher.Invoke(() =>
            {
                var parser = new FileIniDataParser();
                parser.Parser.Configuration.AssigmentSpacer = "";
                string cfgFile = System.IO.Path.Combine(ServerPath, "cfg\\server_cfg.ini");
                IniData data = null;
                if (System.IO.File.Exists(cfgFile))
                {
                    data = parser.ReadFile(cfgFile);
                    grpServer.IsEnabled = true;
                }
                else
                {
                    grpServer.IsEnabled = false;
                }

                try
                {
                    if (data != null)
                    {
                        numPractive.Value = short.Parse(data["PRACTICE"]["TIME"]);
                        numQualify.Value = short.Parse(data["QUALIFY"]["TIME"]);
                        numLaps.Value = short.Parse(data["RACE"]["LAPS"]);
                    }
                }
                catch { }
                try
                {
                    cbTC.ItemsSource = TCassists;
                    cbTC.SelectedIndex = 0;
                    if (data != null)
                        cbTC.SelectedItem = TCassists.FirstOrDefault(i => i.Value == data["SERVER"]["TC_ALLOWED"]);
                }
                catch { }
                try
                {
                    cbABS.ItemsSource = TCassists;
                    cbABS.SelectedIndex = 0;
                    if (data != null)
                        cbABS.SelectedItem = TCassists.FirstOrDefault(i => i.Value == data["SERVER"]["ABS_ALLOWED"]);
                }
                catch { }
                try
                {
                    cbStability.ItemsSource = Stability;
                    cbStability.SelectedIndex = 0;
                    if (data != null)
                        cbStability.SelectedItem = Stability.FirstOrDefault(i => i.Value == data["SERVER"]["STABILITY_ALLOWED"]);
                }
                catch { }

                this.lstCars.DataContext = cars;

                this.lstTracks.DataContext = tracks;
                try
                {
                    var currenttrack = TrackDesc.GetFromGameSettings(GamePath);
                    if (currenttrack != null)
                        this.lstTracks.SelectedItem = tracks.FirstOrDefault(i => i.TrackCode == currenttrack.TrackCode);
                    this.lstTracks.ScrollIntoView(this.lstTracks.SelectedItem);
                }
                catch { }
                try
                {
                    var currentcar = CarDesc.GetFromGameSettings(GamePath);
                    if (currentcar != null)
                        this.lstCars.SelectedItem = cars.FirstOrDefault(i => i.Model == currentcar.Model);
                    this.lstCars.ScrollIntoView(this.lstCars.SelectedItem);
                }
                catch { }
                try
                {
                    this.lstPreset.DataContext = System.IO.Directory.GetDirectories("Presets").Select(i => System.IO.Path.GetFileName(i)).ToList();
                    this.lstPreset.SelectedIndex = 0;
                    this.lstPreset.SelectedItem = Properties.Settings.Default.LastPreset;
                }
                catch { }
                Dispatcher.Invoke(() => { lstServers.DataContext = servers; });
            });

            //


        }





        public void MergeIniFiles(string src, string target)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var srcdata = parser.ReadFile(src);
            var targetdata = parser.ReadFile(target);

            foreach (var section in srcdata.Sections)
            {
                // Remove
                if (containssuffix(section.SectionName, "REMOVE"))
                {
                    var sectionName = cutSuffix(section.SectionName, "REMOVE");
                    if (targetdata.Sections.ContainsSection(sectionName))
                    {
                        targetdata.Sections.RemoveSection(sectionName);
                    }
                }

                // Replace
                else if (containssuffix(section.SectionName, "REPLACE"))
                {
                    var sectionName = cutSuffix(section.SectionName, "REPLACE");

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
                    if (containssuffix(section.SectionName, "MERGE"))
                    {
                        sectionName = cutSuffix(section.SectionName, "MERGE");
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

            parser.SaveFile(target, targetdata);
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

        public string cutSuffix(string name, string suffixe)
        {
            return name.Substring(0, name.Length - ("_" + suffixe).Length);
        }

        public bool containssuffix(string name, string suffixe)
        {
            return name.EndsWith("_" + suffixe, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Start race
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            foreach (var process in Process.GetProcessesByName("acs"))
            {
                process.Kill();
            }

            foreach (var process in Process.GetProcessesByName("AssettoCorsa"))
            {
                process.Kill();
            }

            try
            {
                foreach (var file in System.IO.Directory.GetFiles("Presets\\" + lstPreset.SelectedItem.ToString()))
                {
                    if (!file.ToLower().EndsWith(".merge"))
                    {
                        string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileName(file));
                        System.IO.File.Copy(file, newpath, true);
                    }
                }
                foreach (var file in System.IO.Directory.GetFiles("Presets\\" + lstPreset.SelectedItem.ToString()))
                {
                    if (file.EndsWith(".merge"))
                    {
                        string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileNameWithoutExtension(file));
                        //System.IO.File.Copy(file, newpath, true);
                        MergeIniFiles(file, newpath);
                    }
                }
            }
            catch { }

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");
            var data = parser.ReadFile(raceIniPath);

            var track = lstTracks.SelectedItem as TrackDesc;
            data["RACE"]["TRACK"] = track.Track;
            data["RACE"].RemoveKey("CONFIG_TRACK");
            if (!string.IsNullOrEmpty(track.TrackConfig))
            {
                data["RACE"].AddKey("CONFIG_TRACK", track.TrackConfig);
            }

            var car = lstCars.SelectedItem as CarDesc;
            data["RACE"]["MODEL"] = car.Model;
            try
            {
                data["CAR_0"]["SKIN"] = //System.IO.Path.GetFileName(
                    //System.IO.Directory.GetDirectories(System.IO.Path.Combine(GamePath, "content\\cars", car.Model, "skins")).FirstOrDefault()); ;
                    (lstSkins.SelectedItem as Skin).Name;
            }
            catch { }

            parser.WriteFile(raceIniPath, data, new System.Text.UTF8Encoding(false));

            Properties.Settings.Default.LastPreset = lstPreset.SelectedItem.ToString();
            Properties.Settings.Default.Save();
#if DEBUG
            if (Properties.Settings.Default.UseRename)
#endif
            {
                System.IO.File.Move(System.IO.Path.Combine(GamePath, "AssettoCorsa.exe"), System.IO.Path.Combine(GamePath, "AssettoCorsa.exe.original"));
                System.IO.File.Move(System.IO.Path.Combine(GamePath, "acs.exe"), System.IO.Path.Combine(GamePath, "AssettoCorsa.exe"));

                await Task.Delay(TimeSpan.FromSeconds(1));

                Process.Start(
               new ProcessStartInfo(System.IO.Path.Combine(GamePath, "AssettoCorsa.exe")) { WorkingDirectory = GamePath });
                DateTime startTime = DateTime.Now;

                await Task.Delay(TimeSpan.FromSeconds(3));

                while (Process.GetProcessesByName("AssettoCorsa").Count() == 0 && Process.GetProcessesByName("acs").Count() == 0 && (DateTime.Now - startTime).TotalSeconds < 30)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                System.IO.File.Move(System.IO.Path.Combine(GamePath, "AssettoCorsa.exe"), System.IO.Path.Combine(GamePath, "acs.exe"));
                System.IO.File.Move(System.IO.Path.Combine(GamePath, "AssettoCorsa.exe.original"), System.IO.Path.Combine(GamePath, "AssettoCorsa.exe"));
            }
#if DEBUG
            else
            {
                Process.Start(
                    new ProcessStartInfo(System.IO.Path.Combine(GamePath, "acs.exe")) { WorkingDirectory = GamePath });
            }
#endif


            if (Properties.Settings.Default.UseRename)
            {

            }

            btnStart.IsEnabled = true;
        }

        private Process p;

        /// <summary>
        /// Start Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            if (txtServerRunning.Visibility != System.Windows.Visibility.Collapsed)
            {
                txtServerRunning.Visibility = System.Windows.Visibility.Collapsed;
                KillServers();
                return;
            }
            KillServers();

            var car = lstCars.SelectedItem as CarDesc;
            var track = lstTracks.SelectedItem as TrackDesc;

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";
            string cfgFile = System.IO.Path.Combine(ServerPath, "cfg\\server_cfg.ini");
            string entryFile = System.IO.Path.Combine(ServerPath, "cfg\\entry_list.ini");

            IniData data = parser.ReadFile(cfgFile);

            data["SERVER"]["CARS"] = (lstCars.SelectedItem as CarDesc).Model;

            if (data["SERVER"].ContainsKey("CONFIG_TRACK"))
            {
                data["SERVER"].RemoveKey("CONFIG_TRACK");
            }

            if (!string.IsNullOrWhiteSpace(track.TrackConfig))
            {
                data["SERVER"].AddKey("CONFIG_TRACK", track.TrackConfig);
            }
            data["SERVER"]["TRACK"] = track.Track;

            data["SERVER"]["TC_ALLOWED"] = (cbTC.SelectedItem as Assist).Value;
            data["SERVER"]["ABS_ALLOWED"] = (cbABS.SelectedItem as Assist).Value;
            data["SERVER"]["STABILITY_ALLOWED"] = (cbStability.SelectedItem as Assist).Value;

            data["PRACTICE"]["TIME"] = numPractive.Value.ToString();
            data["QUALIFY"]["TIME"] = numQualify.Value.ToString();
            data["RACE"]["LAPS"] = numLaps.Value.ToString();

            parser.WriteFile(cfgFile, data);

            IniData entryData = new IniData();
            entryData.Configuration.AssigmentSpacer = "";

            //            string dummyCar = listBox2.Items[0].ToString();

            //for (int i = 2; i < 4; i++)
            //{
            //    entryData.Sections.AddSection("CAR_" + i);
            //    entryData["CAR_" + i].AddKey("DRIVERNAME", "");
            //    entryData["CAR_" + i].AddKey("TEAM", "");
            //    entryData["CAR_" + i].AddKey("MODEL", dummyCar);
            //    entryData["CAR_" + i].AddKey("SKIN", System.IO.Path.GetFileName(
            //        System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\cars", dummyCar, "skins")).FirstOrDefault()));
            //    entryData["CAR_" + i].AddKey("GUID", "");
            //    entryData["CAR_" + i].AddKey("SPECTATOR_MODE", "0");
            //}

            for (int i = 0; i < 2; i++)
            {
                entryData.Sections.AddSection("CAR_" + i);
                entryData["CAR_" + i].AddKey("DRIVERNAME", "");
                entryData["CAR_" + i].AddKey("TEAM", "");
                entryData["CAR_" + i].AddKey("MODEL", car.Model);
                entryData["CAR_" + i].AddKey("SKIN", System.IO.Path.GetFileName(
                    System.IO.Directory.GetDirectories(System.IO.Path.Combine(GamePath, "content\\cars", car.Model, "skins")).FirstOrDefault()));
                entryData["CAR_" + i].AddKey("GUID", "");
                entryData["CAR_" + i].AddKey("SPECTATOR_MODE", "0");
            }

            parser.WriteFile(entryFile, entryData);
            string log = "";
            p = new Process();
            p.StartInfo = new ProcessStartInfo() { UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true, RedirectStandardOutput = true, WindowStyle = ProcessWindowStyle.Hidden, FileName = System.IO.Path.Combine(ServerPath, "acServer.exe"), WorkingDirectory = ServerPath };
            p.OutputDataReceived += p_OutputDataReceived;
            p.ErrorDataReceived += p_OutputDataReceived;
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            txtServerRunning.Visibility = System.Windows.Visibility.Visible;
        }

        private static void KillServers()
        {
            foreach (var process in Process.GetProcessesByName("acServer"))
            {
                process.Kill();
            }
        }

        private string log = "";

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            log += "\r\n" + e.Data;
            this.Dispatcher.Invoke(() => { tbServerLogs.Text = log; });
        }

        private void menuReleadData_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                LoadData();
            });
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuOptions_Click(object sender, RoutedEventArgs e)
        {
            (new Settings()).ShowDialog();
            LoadData();
        }

        private void txtCarSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Findcar(false);
            }
        }

        private void txtTrackSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Findtrack(false);
            }
        }

        private void Findcar(bool fromStart)
        {
            // txtCarSearch.Foreground = new SolidColorBrush(Colors.Black);
            if (!string.IsNullOrEmpty(txtCarSearch.Text))
            {
                int baseindex = fromStart ? 0 : lstCars.SelectedIndex;
                for (int i = 1; i < lstCars.Items.Count; i++)
                {
                    var tmpcar = (lstCars.Items[(baseindex + i) % lstCars.Items.Count] as CarDesc);
                    if ((tmpcar.CarInfo.name + " " + tmpcar.CarInfo.brand).ToLower().Contains(txtCarSearch.Text.ToLower()))
                    {
                        lstCars.SelectedIndex = (baseindex + i) % lstCars.Items.Count;
                        this.lstCars.ScrollIntoView(this.lstCars.SelectedItem);
                        return;
                    }
                }
                //      txtCarSearch.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Findtrack(bool fromStart)
        {
            //txtTrackSearch.Foreground = new SolidColorBrush(Colors.Black);
            if (!string.IsNullOrEmpty(txtTrackSearch.Text))
            {
                int baseindex = fromStart ? 0 : lstTracks.SelectedIndex;
                for (int i = 1; i < lstTracks.Items.Count; i++)
                {
                    var tmptrack = (lstTracks.Items[(baseindex + i) % lstTracks.Items.Count] as TrackDesc);
                    if ((tmptrack.TrackInfo.name + " " + tmptrack.TrackInfo.country).ToLower().Contains(txtTrackSearch.Text.ToLower()))
                    {
                        lstTracks.SelectedIndex = (baseindex + i) % lstTracks.Items.Count;
                        this.lstTracks.ScrollIntoView(this.lstTracks.SelectedItem);
                        return;
                    }
                }
                //txtTrackSearch.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void txtCarSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Findcar(true);
        }

        private void txtTrackSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Findtrack(true);
        }

        private void txtServerSearch_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtServerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtServerSearch_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}