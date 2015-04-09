using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingClient.Plugins.OutputPlugins.Dash
{
    public partial class SerialDashSettingsControl : UserControl
    {

        public SerialDashPlugin plugin;

        public SerialDashSettingsControl(SerialDashPlugin plugin)
            : this()
        {
            this.plugin = plugin;
            LoadSettings();
        }

        public void LoadSettings()
        {
            cbReverseModule1.Checked = plugin.Settings.ReverseScreen0;
            cbReverseModule2.Checked = plugin.Settings.ReverseScreen1;
            cbReverseModule3.Checked = plugin.Settings.ReverseScreen2;
            cbReverseModule4.Checked = plugin.Settings.ReverseScreen3;
        }

        public SerialDashSettingsControl()
        {

            InitializeComponent();
            this.cbReverseModule1.CheckedChanged += new System.EventHandler(this.cbReverseModule1_CheckedChanged);
            this.cbReverseModule2.CheckedChanged += new System.EventHandler(this.cbReverseModule1_CheckedChanged);
            this.cbReverseModule3.CheckedChanged += new System.EventHandler(this.cbReverseModule1_CheckedChanged);
            this.cbReverseModule4.CheckedChanged += new System.EventHandler(this.cbReverseModule1_CheckedChanged);
        }

        private void lblDetectedModules_Click(object sender, EventArgs e)
        {

        }

        private void detectTimer_Tick(object sender, EventArgs e)
        {
            if (plugin != null)
            {
                this.lblDetectedModules.Text = "Detected modules : " + plugin.Dash.GetModuleCount().ToString();
            }
        }

        private void cbReverseModule1_CheckedChanged(object sender, EventArgs e)
        {
            plugin.Settings.ReverseScreen0 = cbReverseModule1.Checked;
            plugin.Settings.ReverseScreen1 = cbReverseModule2.Checked;
            plugin.Settings.ReverseScreen2 = cbReverseModule3.Checked;
            plugin.Settings.ReverseScreen3 = cbReverseModule4.Checked;
            plugin.ApplySettings();

        }

        private void btnNewScreen_Click(object sender, EventArgs e)
        {
            (new ScreenEditor(plugin.PluginManager)).ShowDialog();
        }
    }
}
