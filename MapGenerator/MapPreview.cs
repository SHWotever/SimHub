﻿using ACSharedMemory;
using MapGenerator.Renderers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace MapGenerator
{
    public partial class MapPreview : Form
    {
        public class DragLink
        {
            private Panel ScrollPanel;
            private PictureBox PictureBox;

            public DragLink(PictureBox pb, Panel scrollPanel)
            {
                this.PictureBox = pb;
                this.ScrollPanel = scrollPanel;

                this.PictureBox.MouseDown += PictureBox_MouseDown;
                this.PictureBox.MouseUp += PictureBox_MouseUp;
                this.PictureBox.MouseMove += PictureBox_MouseMove;
                this.PictureBox.MouseEnter += PictureBox_MouseEnter;
            }

            private void PictureBox_MouseEnter(object sender, EventArgs e)
            {
                //PictureBox.Focus();
            }

            private void PictureBox_MouseMove(object sender, MouseEventArgs e)
            {
                Debug.WriteLine(e.Location.X + " " + e.Location.Y);
                if (isDragging)
                {
                    this.ScrollPanel.HorizontalScroll.Value =
                        Math.Min(
                            Math.Max(0, this.ScrollPanel.HorizontalScroll.Value - Cursor.Position.X + lastPosition.X),
                            this.ScrollPanel.HorizontalScroll.Maximum);
                    this.ScrollPanel.VerticalScroll.Value =
                        Math.Min(
                            Math.Max(0, this.ScrollPanel.VerticalScroll.Value - Cursor.Position.Y + lastPosition.Y),
                            this.ScrollPanel.VerticalScroll.Maximum);
                }

                lastPosition = Cursor.Position;
            }

            private void PictureBox_MouseUp(object sender, MouseEventArgs e)
            {
                isDragging = false;
            }

            private Point lastPosition;
            private bool isDragging;

            private void PictureBox_MouseDown(object sender, MouseEventArgs e)
            {
                isDragging = true;
                lastPosition = Cursor.Position;
                this.ScrollPanel.Focus();
            }
        }

        public static MColor ToMediaColor(DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        private DragLink dragLink;

        public MapPreview()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Application.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            InitializeComponent();
            this.tableLayoutPanel1.Enabled = false;
            dragLink = new DragLink(this.pictureBox1, this.panel1);
            //this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            this.panel1.MouseWheel += pictureBox1_MouseWheel;
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            //this.tbZoom.Value = Math.Min(Math.Max(this.tbZoom.Minimum, e.Delta), this.tbZoom.Maximum);
        }

        private MapRendererBase data;

        private void RefreshMap()
        {
            this.panel1.AutoScroll = true;
            this.panel1.HorizontalScroll.Enabled = true;
            this.pictureBox1.Dock = DockStyle.None;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            data.Scale = (decimal)numTrackZoom.Value;
            data.CloseLoopPoints = (int)numericUpDown1.Value;
            data.TrackBorderColor = ToMediaColor(TrackBorderColorPicker.SelectedColor);
            data.TrackColor = ToMediaColor(TrackColorPicker.SelectedColor);
            data.AlternateSectorColor = ToMediaColor(AlternateSectorMember.SelectedColor);

            data.InnerPathWidth = (int)numInner.Value;
            data.OuterPathWidth = (int)numOuter.Value;

            this.pictureBox1.Image = data.GetMap();

            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.lblOutputSize.Text = this.pictureBox1.Image.Width + "x" + this.pictureBox1.Image.Height;
            SetImageZoom();
        }

        public void SetImageZoom()
        {
            SetImageZoom(tbZoom.Value);
        }

        public void SetImageZoom(double zoom)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Width = (int)((double)this.pictureBox1.Image.Width * (zoom / 100d));
            this.pictureBox1.Height = (int)((double)this.pictureBox1.Image.Height * (zoom / 100d));
            this.lblZoom.Text = zoom + "%";
        }

        private void openTrackPositionDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Track record (*.trackrecord, *.json, *.tracktelemetry)|*.trackrecord;*.json;*.tracktelemetry";

            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastDataFolder))
            {
                if (Directory.Exists(Properties.Settings.Default.LastDataFolder))
                {
                    of.InitialDirectory = Properties.Settings.Default.LastDataFolder;
                }
            }

            if (of.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (of.FileName.ToLower().EndsWith("tracktelemetry"))
                    {
                        var telemetry = TelemetryReader.Read(of.FileName);
                        data = new AdvancedMapRenderer(of.FileName, telemetry);
                    }
                    else
                    {
                        data = new SimpleMapRenderer(of.FileName);
                    }
                    RefreshMap();
                    this.tableLayoutPanel1.Enabled = true;
                    this.Text = "Map Generator - " + Path.GetFileName(of.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void tbZoom_ValueChanged(object sender, EventArgs e)
        {
            SetImageZoom();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void exportMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshMap();
            FolderBrowserDialogEx diag = new FolderBrowserDialogEx();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastExportFolder))
            {
                if (Directory.Exists(Properties.Settings.Default.LastExportFolder))
                {
                    diag.SelectedPath = Properties.Settings.Default.LastExportFolder;
                }
            }

            //diag.
            if (diag.ShowDialog() == DialogResult.OK)
            {
                data.ExportMap(diag.SelectedPath);
                Properties.Settings.Default.LastExportFolder = diag.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void recordMapDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEx diag = new FolderBrowserDialogEx();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastDataFolder))
            {
                if (Directory.Exists(Properties.Settings.Default.LastDataFolder))
                {
                    diag.SelectedPath = Properties.Settings.Default.LastDataFolder;
                }
            }

            diag.Description = "Choose map data export folder";
            diag.ShowNewFolderButton = true;

            if (diag.ShowDialog() == DialogResult.OK)
            {
                TrackRecord recordForm = new TrackRecord();
                recordForm.OutputFolder = diag.SelectedPath;

                Properties.Settings.Default.LastDataFolder = diag.SelectedPath;
                Properties.Settings.Default.Save();

                recordForm.ShowDialog();
            }
        }
    }
}