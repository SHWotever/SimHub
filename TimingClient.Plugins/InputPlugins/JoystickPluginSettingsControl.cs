using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingClient.Plugins.InputPlugins
{
    public partial class JoystickPluginSettingsControl : UserControl
    {
        public JoystickPluginSettingsControl()
        {
            InitializeComponent();
        }

        public void Refresh(List<string> inputs)
        {
            if (this.cbShowStatus.Checked)
            {
                if (inputs.Count > 0)
                    this.txtSettings.Text += DateTime.Now.ToString() + " : " + string.Join("\r\n" + DateTime.Now.ToString() + " : ", inputs) + "\r\n";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
