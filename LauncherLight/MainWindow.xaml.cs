using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ACSharedMemory.Models.Car;
using ACSharedMemory.Models.Track;
using IniParser;
using IniParser.Model;
using LauncherLight.Models;
using LauncherLight.UserControls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace LauncherLight
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //private string GamePath = @"D:\Program Files (x86)\Assetto Corsa";
        //private string ServerPath = @"D:\Program Files (x86)\Assetto Corsa\Server";

        private OnlineTab onlineContext = new OnlineTab();

        public MainWindow()
        {
            this.Closing += MainWindow_Closing;

            InitializeComponent();

            this.onlineTab.DataContext = onlineContext;

            this.lstCars.SelectionChanged += lstCars_SelectionChanged;
            btnSettings.Click += btnSettings_Click;
            btnChangeServer.Click += btnChangeServer_Click;
            btnOpenInfoServer.Click += btnOpenInfoServer_Click;
            btnGameSettingsPesets.Click += BtnGameSettingsPesets_Click;
            Task.Factory.StartNew(() =>
            {
                LoadData();
            });
        }

        private void BtnGameSettingsPesets_Click(object sender, RoutedEventArgs e)
        {
            SettingsPresets control = new SettingsPresets();
            DialogHost dialog = GetDialog(control);

            control.Cancel = () =>
            {
                this.HideMetroDialogAsync(dialog);
            };
            this.ShowMetroDialogAsync(dialog);
        }

        private void btnOpenInfoServer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(onlineContext.CurrentServer.AdressInfo);
        }

        private void btnChangeServer_Click(object sender, RoutedEventArgs e)
        {
            SelectServer();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Helpers.AbortRefresh();
            Properties.Settings.Default.LastServer = Newtonsoft.Json.JsonConvert.SerializeObject(onlineContext.CurrentServer);
            Properties.Settings.Default.Save();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            (new Settings()).ShowDialog();
            LoadData();
        }

        private void lstCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (lstCars.SelectedItem != null)
            //{
            //    var items = (lstCars.SelectedItem as CarDesc).GetSkins().ToList();
            //    lstSkins.DataContext = items;
            //    lstSkins.SelectedValue = items.FirstOrDefault();
            //}
        }

        private string GamePath { get { return Properties.Settings.Default.GamePath; } }

        private string ServerPath { get { return Properties.Settings.Default.ServerPath; } }

        private List<LLCarDesc> cars;
        private List<LLTrackDesc> tracks;

        private void LoadData()
        {
            Dispatcher.Invoke(() =>
        {
            this.IsEnabled = false;
            this.grpServer.Visibility = Properties.Settings.Default.EnableServerFeature ? Visibility.Visible : System.Windows.Visibility.Collapsed;
            //Grid.SetRowSpan(this.grdCars, Properties.Settings.Default.EnableServerFeature ? 1 : 2);
        });

            string GamePath = this.GamePath;

            List<Assist> TCassists = Helpers.GetTCAssits();

            List<Assist> Stability = Helpers.GetStabilityAssists();
            cars = Helpers.GetCars(GamePath);
            tracks = Helpers.GetTracks(GamePath);

            AvailableRessources.Tracks = tracks.ToDictionary(i => i.TrackCode);
            AvailableRessources.Cars = cars.ToDictionary(i => i.Model);

            cars = cars.OrderBy(i => i.CarInfo.brand).ThenBy(i => i.CarInfo.name).ToList();
            tracks = tracks.OrderBy(i => i.TrackInfo.name).ToList();

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
                    else
                        this.lstTracks.SelectedItem = tracks.First();
                    this.lstTracks.ScrollIntoView(this.lstTracks.SelectedItem);
                }
                catch { }
                try
                {
                    var currentcar = CarDesc.GetFromGameSettings(GamePath);
                    if (currentcar != null)
                        this.lstCars.SelectedItem = cars.FirstOrDefault(i => i.Model == currentcar.Model);
                    else
                        this.lstCars.SelectedItem = cars.First();

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

                try
                {
                    var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<ACServer>(Properties.Settings.Default.LastServer);
                    if (tmp != null)
                    {
                        tmp.fill(true);
                    }
                    this.onlineContext.CurrentServer = tmp;
                }
                catch { }
                this.IsEnabled = true;
            });
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
            if (Properties.Settings.Default.UseRename)
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
                        Helpers.MergeIniFiles(file, newpath);
                    }
                }
            }
            catch { }

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");
            var data = parser.ReadFile(raceIniPath);

            var track = lstTracks.SelectedItem as LLTrackDesc;
            if (cars == null || track == null)
            {
                btnStart.IsEnabled = true;
                return;
            }
            data["RACE"]["TRACK"] = track.Track;
            data["RACE"].RemoveKey("CONFIG_TRACK");
            if (!string.IsNullOrEmpty(track.TrackConfig))
            {
                data["RACE"].AddKey("CONFIG_TRACK", track.TrackConfig);
            }

            var car = lstCars.SelectedItem as LLCarDesc;
            data["RACE"]["MODEL"] = car.Model;
            try
            {
                data["CAR_0"]["SKIN"] = //System.IO.Path.GetFileName(
                    //System.IO.Directory.GetDirectories(System.IO.Path.Combine(GamePath, "content\\cars", car.Model, "skins")).FirstOrDefault()); ;
                    car.CurrentSkin.Name;
            }
            catch { }

            parser.WriteFile(raceIniPath, data, new System.Text.UTF8Encoding(false));

            await StartGame();

            btnStart.IsEnabled = true;
        }

        private async Task StartGame()
        {
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
                logsTab.Visibility = Visibility.Collapsed;
                txtServerRunning.Visibility = System.Windows.Visibility.Collapsed;
                KillServers();
                return;
            }
            KillServers();
            logsTab.Visibility = Visibility.Visible;
            var car = lstCars.SelectedItem as LLCarDesc;
            var track = lstTracks.SelectedItem as LLTrackDesc;

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";
            string cfgFile = System.IO.Path.Combine(ServerPath, "cfg\\server_cfg.ini");
            string entryFile = System.IO.Path.Combine(ServerPath, "cfg\\entry_list.ini");

            IniData data = parser.ReadFile(cfgFile);

            data["SERVER"]["CARS"] = (lstCars.SelectedItem as LLCarDesc).Model;

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
                    var tmpcar = (lstCars.Items[(baseindex + i) % lstCars.Items.Count] as LLCarDesc);
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
                    var tmptrack = (lstTracks.Items[(baseindex + i) % lstTracks.Items.Count] as LLTrackDesc);
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

        private ACServer currentServer = null;

        private DialogHost GetDialog(FrameworkElement control)
        {
            DialogHost dialog = new DialogHost();
            dialog.ContentControl = control;
            return dialog;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectServer();
        }

        private bool closed = false;

        private void SelectServer(Action oncloseDelegate = null)
        {
            closed = false;
            ServerList serverListControl = new ServerList();
            DialogHost dialog = GetDialog(serverListControl);

            serverListControl.ServerSelected = (a) =>
            {
                closed = true;
                //Task.Factory.StartNew(() =>
                //Helpers.RefreshServer(a, Properties.Settings.Default.SteamID, false));
                onlineContext.CurrentServer = a;

                Properties.Settings.Default.LastServer = Newtonsoft.Json.JsonConvert.SerializeObject(a);
                Properties.Settings.Default.Save();
                this.HideMetroDialogAsync(dialog);
                if (oncloseDelegate != null)
                {
                    oncloseDelegate();
                }
                Helpers.AbortRefresh();
            };

            serverListControl.Cancel = () =>
            {
                this.HideMetroDialogAsync(dialog);
                closed = true;
                if (oncloseDelegate != null)
                {
                    oncloseDelegate();
                }
                Helpers.AbortRefresh();
            };

            this.ShowMetroDialogAsync(dialog).ContinueWith((a) =>
            {
                var servers = Helpers.GetOnlineServers(Properties.Settings.Default.SteamID);
                foreach (var server in servers)
                {
                    server.fill(false);
                }

                Helpers.RefreshServers(servers, Properties.Settings.Default.SteamID);
                serverListControl.SetSevers(servers);
                var LANservers = Helpers.GetLanServers(Properties.Settings.Default.SteamID);

                foreach (var server in LANservers)
                {
                    server.fill(true);
                    Helpers.RefreshServer(server, Properties.Settings.Default.SteamID, false);
                }
                //Helpers.RefreshServers(LANservers, Properties.Settings.Default.SteamID);
                serverListControl.SetLanServers(LANservers);
            });

            //while (!closed)
            //{
            //    try
            //    {
            //        DoEvents();
            //    }
            //    catch { }

            //}
        }

        public void DoEvents()
        {
            try
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                    new DispatcherOperationCallback(ExitFrame), frame);
                Dispatcher.PushFrame(frame);
            }
            catch { }
        }

        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

        private void btnSkinChoice_Click(object sender, RoutedEventArgs e)
        {
            SkinChoice skinChoiceControl = new SkinChoice();
            DialogHost dialog = GetDialog(skinChoiceControl);

            var currentCar = this.lstCars.SelectedItem as LLCarDesc;

            skinChoiceControl.SkinSelected = (a) =>
            {
                currentCar.CurrentSkin = a;
                this.HideMetroDialogAsync(dialog);
            };
            skinChoiceControl.Cancel = () =>
            {
                this.HideMetroDialogAsync(dialog);
            };

            this.ShowMetroDialogAsync(dialog).ContinueWith((a) =>
            {
                skinChoiceControl.SetSkins(currentCar.Skins);
            });
        }

        private void btnOnlineSkinChoice_Click(object sender, RoutedEventArgs e)
        {
            SkinChoice skinChoiceControl = new SkinChoice();
            DialogHost dialog = GetDialog(skinChoiceControl);

            if (onlineContext.CurrentServer != null && onlineContext.CurrentServer.SelectedCar != null)
            {
                var currentCar = this.onlineContext.CurrentServer.SelectedCar.CarDesc;

                skinChoiceControl.SkinSelected = (a) =>
                {
                    currentCar.CurrentSkin = a;
                    this.HideMetroDialogAsync(dialog);
                };
                skinChoiceControl.Cancel = () =>
                {
                    this.HideMetroDialogAsync(dialog);
                };

                this.ShowMetroDialogAsync(dialog).ContinueWith((a) =>
                {
                    skinChoiceControl.SetSkins(currentCar.Skins);
                });
            }
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedItem == onlineTab)
            {
                if (onlineContext.CurrentServer == null)
                {
                    e.Handled = true;
                    SelectServer(() =>
                    {
                        if (onlineContext.CurrentServer == null)
                        {
                            tab.SelectedIndex = 0;
                        }
                    });
                }
            }
        }

        private async void btnStartOnline_Click(object sender, RoutedEventArgs e)
        {
            foreach (var file in System.IO.Directory.GetFiles("OnlineTemplate\\"))
            {
                if (!file.ToLower().EndsWith(".merge"))
                {
                    string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileName(file));
                    System.IO.File.Copy(file, newpath, true);
                }
            }
            foreach (var file in System.IO.Directory.GetFiles("OnlineTemplate\\"))
            {
                if (file.EndsWith(".merge"))
                {
                    string newpath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", System.IO.Path.GetFileNameWithoutExtension(file));
                    //System.IO.File.Copy(file, newpath, true);
                    Helpers.MergeIniFiles(file, newpath);
                }
            }

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");
            var data = parser.ReadFile(raceIniPath);

            data["REMOTE"]["SERVER_IP"] = onlineContext.CurrentServer.ip;
            data["REMOTE"]["SERVER_PORT"] = onlineContext.CurrentServer.tport.ToString();
            data["REMOTE"]["REQUESTED_CAR"] = onlineContext.CurrentServer.SelectedCar.CarDesc.Model;
            data["REMOTE"]["NAME"] = Properties.Settings.Default.PlayerName;
            data["REMOTE"]["PASSWORD"] = onlineContext.CurrentServer.pass ? onlineContext.CurrentServer.CurrentPassword : "";
            data["REMOTE"]["GUID"] = Properties.Settings.Default.SteamID;
            data["REMOTE"]["GUID"] = Properties.Settings.Default.SteamID;

            data["CAR_0"]["SKIN"] = onlineContext.CurrentServer.SelectedCar.CarDesc.CurrentSkin.Name;
            data["CAR_0"]["DRIVER_NAME"] = Properties.Settings.Default.PlayerName;

            parser.WriteFile(raceIniPath, data, new System.Text.UTF8Encoding(false));

            await StartGame();
        }
    }
}