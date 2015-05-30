using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialDashWPF
{
    /// <summary>
    /// Logique d'interaction pour DisplayWPF.xaml
    /// </summary>
    public partial class DisplayWPF : UserControl
    {
        public DisplayWPF()
        {
            InitializeComponent();
            letters = new Letter[] { displayLetter0, displayLetter1, displayLetter2, displayLetter3, displayLetter4, displayLetter5, displayLetter6, displayLetter7 };
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var item in (sender as StackPanel).Children.Cast<Letter>())
            {
                item.Height = (sender as StackPanel).ActualHeight;
                item.Width = (sender as StackPanel).ActualHeight * 0.54;

            }
        }
        Letter[] letters;
        public void SetText(string text)
        {
            var bytes = SerialDash.SerialDashController.getDataFromDefaultFont(text, false);

            for (int i = 0; i < 8; i++)
            {
                letters[i].SetValue(bytes[i]);
            }

        }
    }
}
