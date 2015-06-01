using ACToolsUtilities.Serialisation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Settings control for serial dash
    /// </summary>
    public partial class SerialDashSettingsControl : UserControl
    {
        private SerialDashPlugin plugin;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="plugin"></param>
        public SerialDashSettingsControl(SerialDashPlugin plugin)
            : this()
        {
            this.plugin = plugin;
            LoadSettings(false);
        }

        /// <summary>
        /// CTor
        /// </summary>
        public SerialDashSettingsControl()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.Invoke((MethodInvoker)delegate
            {
                LoadSettings(true);
            });
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="light"></param>
        private void LoadSettings(bool light)
        {
            this.numLowFuelLaps.ValueChanged -= Settings_numValueChanged;
            this.numRPMBlink.ValueChanged -= Settings_numValueChanged;
            this.numRPMOffset.ValueChanged -= Settings_numValueChanged;
            this.numLowFuelRepeatInterval.ValueChanged -= Settings_numValueChanged;
            this.numIntensity.ValueChanged -= Settings_numValueChanged;
            this.numAnnounceTime.ValueChanged -= Settings_numValueChanged;

            this.cbReverseModule1.CheckedChanged -= this.Settings_cbCheckedChanged;
            this.cbReverseModule2.CheckedChanged -= this.Settings_cbCheckedChanged;
            this.cbReverseModule3.CheckedChanged -= this.Settings_cbCheckedChanged;
            this.cbReverseModule4.CheckedChanged -= this.Settings_cbCheckedChanged;

            this.cbReverseModule1.Checked = plugin.Settings.ReverseScreen0;
            this.cbReverseModule2.Checked = plugin.Settings.ReverseScreen1;
            this.cbReverseModule3.Checked = plugin.Settings.ReverseScreen2;
            this.cbReverseModule4.Checked = plugin.Settings.ReverseScreen3;

            this.numRPMOffset.Value = (decimal)plugin.Settings.RPMStartOffset;
            this.numLowFuelLaps.Value = (decimal)plugin.Settings.LowFuelLapsLevel;
            this.numRPMBlink.Value = (decimal)plugin.Settings.RpmBlinkingLevel;
            this.numLowFuelRepeatInterval.Value = (decimal)plugin.Settings.LowFuelLapsAlertInterval;
            this.numIntensity.Value = (decimal)plugin.Settings.Intensity;
            this.numAnnounceTime.Value = (decimal)plugin.Settings.AnnounceTime;


            if (!light)
            {
                if (plugin.Settings.Screens == null)
                {
                    plugin.Settings.Screens = new List<Screen>();
                }

                this.lstScreens.DataSource = plugin.Settings.Screens;

                this.ledEditor2.LoadLeds(plugin.Settings.LedSettings, plugin);
            }

            this.cbReverseModule1.CheckedChanged += this.Settings_cbCheckedChanged;
            this.cbReverseModule2.CheckedChanged += this.Settings_cbCheckedChanged;
            this.cbReverseModule3.CheckedChanged += this.Settings_cbCheckedChanged;
            this.cbReverseModule4.CheckedChanged += this.Settings_cbCheckedChanged;

            this.numLowFuelLaps.ValueChanged += Settings_numValueChanged;
            this.numRPMBlink.ValueChanged += Settings_numValueChanged;
            this.numRPMOffset.ValueChanged += Settings_numValueChanged;
            this.numLowFuelRepeatInterval.ValueChanged += Settings_numValueChanged;
            this.numIntensity.ValueChanged += Settings_numValueChanged;
            this.numAnnounceTime.ValueChanged += Settings_numValueChanged;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lstScreens.SelectedItem != null)
            {
                lock (plugin.Settings)
                {
                    if (MessageBox.Show("Are you sure to want to remove this screen ?", "Remove screen", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Screen exp = this.lstScreens.SelectedItem as Screen;
                        plugin.Settings.Screens.Remove(exp);
                    }
                }
            }
            try
            {
                this.lstScreens.SelectedItem = null;
                this.lstScreens.RefreshItems();
            }
            catch { }
            plugin.ApplySettings();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (this.lstScreens.SelectedItem != null)
            {
                lock (plugin.Settings)
                {
                    Screen exp = this.lstScreens.SelectedItem as Screen;
                    var oldidx = plugin.Settings.Screens.IndexOf(exp);
                    if (oldidx < plugin.Settings.Screens.Count - 1)
                    {
                        plugin.Settings.Screens.Remove(exp);
                        plugin.Settings.Screens.Insert(oldidx + 1, exp);
                        this.lstScreens.RefreshItems();
                        this.lstScreens.SelectedItem = exp;
                        this.lstScreens.RefreshItems();
                    }
                }
            }
            try
            {
            }
            catch { }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.lstScreens.SelectedItem != null)
            {
                Screen exp = this.lstScreens.SelectedItem as Screen;
                var result = EditScreen(exp);
                if (result != null)
                {
                    lock (plugin.Settings)
                    {
                        var idx = plugin.Settings.Screens.IndexOf(exp);
                        plugin.Settings.Screens.RemoveAt(idx);
                        plugin.Settings.Screens.Insert(idx, result);
                    }
                }
            }
            this.lstScreens.RefreshItems();
            plugin.ApplySettings();
        }

        private void btnNewScreen_Click(object sender, EventArgs e)
        {
            Screen exp = new Screen();
            var editor = new ScreenEditor();
            var result = EditScreen(null);

            if (result != null)
            {
                plugin.Settings.Screens.Add(result);
            }

            this.lstScreens.RefreshItems();
            plugin.ApplySettings();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (this.lstScreens.SelectedItem != null)
            {
                lock (plugin.Settings)
                {
                    Screen exp = this.lstScreens.SelectedItem as Screen;
                    var oldidx = plugin.Settings.Screens.IndexOf(exp);
                    if (oldidx > 0)
                    {
                        plugin.Settings.Screens.Remove(exp);
                        plugin.Settings.Screens.Insert(oldidx - 1, exp);
                        this.lstScreens.RefreshItems();
                        this.lstScreens.SelectedItem = exp;
                        this.lstScreens.RefreshItems();
                    }
                }
            }
            try
            {
            }
            catch { }
        }

        private void Settings_cbCheckedChanged(object sender, EventArgs e)
        {
            plugin.Settings.ReverseScreen0 = cbReverseModule1.Checked;
            plugin.Settings.ReverseScreen1 = cbReverseModule2.Checked;
            plugin.Settings.ReverseScreen2 = cbReverseModule3.Checked;
            plugin.Settings.ReverseScreen3 = cbReverseModule4.Checked;
            plugin.ApplySettings();
        }

        private void detectTimer_Tick(object sender, EventArgs e)
        {
            if (plugin != null)
            {
                this.lblDetectedModules.Text = "Detected modules : " + plugin.Dash.GetModuleCount().ToString();
            }
        }

        private Screen EditScreen(Screen current)
        {
            var editor = new ScreenEditor();
            var result = editor.ShowDialog(current ?? new Screen() { ScreenName = "NewScreen" }, plugin.PluginManager);
            if (result != null)
            {
                bool ok = false;
                while (!ok)
                {
                    ok = true;
                    if (result.ScreenName == string.Empty)
                    {
                        MessageBox.Show("Please give a name to your screen");
                        ok = false;
                    }
                    else if (plugin.Settings.Screens.Exists(i => i.ScreenName.Equals(result.ScreenName) && i != current))
                    {
                        MessageBox.Show("An screen with this name already exists");
                        ok = false;
                    }

                    if (ok) break;

                    result = editor.ShowDialog(result, plugin.PluginManager);
                    if (result == null)
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        private void lblDetectedModules_Click(object sender, EventArgs e)
        {
        }

        private void refreshingListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Settings_numValueChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            plugin.Settings.RPMStartOffset = (double)numRPMOffset.Value;
            plugin.Settings.LowFuelLapsLevel = (double)numLowFuelLaps.Value;
            plugin.Settings.RpmBlinkingLevel = (double)numRPMBlink.Value;
            plugin.Settings.LowFuelLapsAlertInterval = (int)numLowFuelRepeatInterval.Value;
            plugin.Settings.Intensity = (int)numIntensity.Value;
            plugin.Settings.AnnounceTime = (double)numAnnounceTime.Value;
            plugin.ApplySettings();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (this.lstScreens.SelectedItem != null)
            {
                Screen exp = this.lstScreens.SelectedItem as Screen;
                var idx = plugin.Settings.Screens.IndexOf(exp);

                exp = exp.JsonClone();
                exp.ScreenName = GetCopyName(exp.ScreenName);
                var result = EditScreen(exp);
                if (result != null)
                {
                    lock (plugin.Settings)
                    {
                        plugin.Settings.Screens.Insert(idx + 1, result);
                    }
                }
            }
            this.lstScreens.RefreshItems();
            plugin.ApplySettings();
        }

        private string GetCopyName(string currentName)
        {
            int i = 0;
            currentName = Regex.Replace(currentName, "Copy[0-9]*$", "");

            var newname = GetCopyName(currentName, i);
            while (plugin.Settings.Screens.Exists(j => j.ScreenName == newname))
            {
                i++;
                newname = GetCopyName(currentName, i);
            }
            return newname;
        }

        private static string GetCopyName(string currentName, int i)
        {
            return currentName + "Copy" + (i == 0 ? "" : i.ToString());
        }
    }
}