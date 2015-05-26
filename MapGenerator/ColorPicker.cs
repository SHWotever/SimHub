using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
            this.pictureBox1.BackColor = Color.Black;
        }

        public Color SelectedColor
        {
            get
            {
                return this.pictureBox1.BackColor;
            }

            set
            {
                this.pictureBox1.BackColor = value;
            }
        }

        //public new bool Enabled {
        //    set {   this.pictureBox1. }
        //    get {return this.Enabled }
        //}

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var ColorDialog = new ColorDialog();
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.SelectedColor = ColorDialog.Color;
            }
        }
    }
}