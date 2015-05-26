using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingClient.Plugins.UI
{
    public partial class PluginsSettings : UserControl
    {
        private Size itemSize;

        /// <summary>
        /// CTor
        /// </summary>
        public PluginsSettings()
        {
            InitializeComponent();
            itemSize = this.verticalTabControl1.ItemSize;
            this.verticalTabControl1.TabPages.Clear();
        }

        /// <summary>
        /// Add plugin settings control
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="control"></param>
        public void AddPlugin(string name, string version, Control control)
        {
            this.verticalTabControl1.ItemSize = itemSize;
            var tab = new TabPage();
            tab.Text = string.Format("{0}\r\n{1}", name, version);
            control.Dock = DockStyle.Fill;
            tab.Controls.Add(control);
            this.verticalTabControl1.TabPages.Add(tab);
            this.verticalTabControl1.ItemSize = itemSize;
        }
    }
}
