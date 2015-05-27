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
    public partial class DashDisplay : UserControl
    {
        public DashDisplay()
        {
            InitializeComponent();
            this.SetText("             ");
        }

        public void SetText(string text)
        {
            this.displayWPF1.SetText(text);
        }
    }
}
