﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SerialDash
{
    public partial class DisplayLetter : UserControl
    {
        public DisplayLetter()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
        }

        public void SetValue(int value)
        {
            for (int i = 0; i < 8; i++)
            {
                var item = this.Controls.Find("pb" + (i + 1).ToString(), true).FirstOrDefault() as PictureBox;
                item.BackColor = ((value & (int)Math.Pow(2, i)) > 0) ? Color.Red : Color.Transparent;
            }
        }
    }
}