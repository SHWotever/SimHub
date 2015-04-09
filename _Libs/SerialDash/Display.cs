using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialDash
{
    public partial class Display : UserControl
    {
        public Display()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            var bytes = SerialDash.SerialDashController.getDataFromDefaultFont(text, false);

            for (int i = 0; i < 8; i++)
            {
                (this.Controls.Find("displayLetter" + (i + 1).ToString(), true).First() as DisplayLetter).SetValue(bytes[i]);
            }

        }
    }
}
