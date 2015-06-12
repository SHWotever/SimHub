using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SerialDashWPF
{
    /// <summary>
    /// Logique d'interaction pour Letter.xaml
    /// </summary>
    public partial class Letter : UserControl
    {
        public Letter()
        {
            InitializeComponent();
            segments = new Rectangle[] { pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8 };
        }

        private Rectangle[] segments;

        public void SetValue(int value)
        {
            var on = new SolidColorBrush(Colors.Red);
            var off = new SolidColorBrush(Color.FromRgb(0x12, 0x12, 0x12));

            for (int i = 0; i < 8; i++)
            {
                var item = segments[i];
                item.Fill = ((value & (int)Math.Pow(2, i)) > 0) ? on : off;
            }
        }
    }
}