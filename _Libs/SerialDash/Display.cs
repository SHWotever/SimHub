using System.Linq;
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