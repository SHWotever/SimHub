using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    public partial class ScreenAnnounceEditor : UserControl
    {
        public ScreenAnnounceEditor()
        {
            InitializeComponent();
            this.dispPreview.SetText("");
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {
            this.dispPreview.SetText(SerialDash.SerialDashController.ReplaceChars(this.txtText.Text));
        }

        public string Value { get { return this.txtText.Text; } set { this.txtText.Text = value; } }
        public string Title { get { return this.groupBox1.Text; } set { this.groupBox1.Text = value; } }




    }
}
