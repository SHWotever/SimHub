namespace MapGenerator
{
    partial class MapPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numLoopFix = new System.Windows.Forms.NumericUpDown();
            this.lblLoopFix = new System.Windows.Forms.Label();
            this.numOuter = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.TrackBorderColorPicker = new MapGenerator.ColorPicker();
            this.label3 = new System.Windows.Forms.Label();
            this.numInner = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.TrackColorPicker = new MapGenerator.ColorPicker();
            this.label2 = new System.Windows.Forms.Label();
            this.numTrackZoom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.sectorSeparatorColorPicker = new MapGenerator.ColorPicker();
            this.label11 = new System.Windows.Forms.Label();
            this.numSectorSeparatorHeight = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numSectorSeparatorWidth = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDrawSectorSeparators = new System.Windows.Forms.CheckBox();
            this.AlternateSectorMember = new MapGenerator.ColorPicker();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.numTurnAngle = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.TurnColorPicker = new MapGenerator.ColorPicker();
            this.label8 = new System.Windows.Forms.Label();
            this.cbHighlightTurns = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbZoom = new System.Windows.Forms.TrackBar();
            this.lblZoom = new System.Windows.Forms.Label();
            this.lblOutputSize = new System.Windows.Forms.Label();
            this.sourceFile = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTrackPositionDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordMapDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.timer1 = new System.Windows.Forms.Timer();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopFix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOuter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrackZoom)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSectorSeparatorHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSectorSeparatorWidth)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTurnAngle)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbZoom)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 327F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 667F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOutputSize, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(994, 596);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnGenerate, 0, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(321, 550);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(315, 509);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(307, 483);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Track";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.numLoopFix, 0, 11);
            this.tableLayoutPanel4.Controls.Add(this.lblLoopFix, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.numOuter, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.TrackBorderColorPicker, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.numInner, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.TrackColorPicker, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.numTrackZoom, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 12;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(301, 477);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // numLoopFix
            // 
            this.numLoopFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numLoopFix.Location = new System.Drawing.Point(3, 217);
            this.numLoopFix.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numLoopFix.Name = "numLoopFix";
            this.numLoopFix.Size = new System.Drawing.Size(295, 20);
            this.numLoopFix.TabIndex = 21;
            // 
            // lblLoopFix
            // 
            this.lblLoopFix.AutoSize = true;
            this.lblLoopFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoopFix.Location = new System.Drawing.Point(3, 201);
            this.lblLoopFix.Name = "lblLoopFix";
            this.lblLoopFix.Size = new System.Drawing.Size(295, 13);
            this.lblLoopFix.TabIndex = 23;
            this.lblLoopFix.Text = "Loop close points";
            // 
            // numOuter
            // 
            this.numOuter.Dock = System.Windows.Forms.DockStyle.Top;
            this.numOuter.Location = new System.Drawing.Point(3, 178);
            this.numOuter.Name = "numOuter";
            this.numOuter.Size = new System.Drawing.Size(295, 20);
            this.numOuter.TabIndex = 19;
            this.numOuter.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(295, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Track border width";
            // 
            // TrackBorderColorPicker
            // 
            this.TrackBorderColorPicker.Dock = System.Windows.Forms.DockStyle.Top;
            this.TrackBorderColorPicker.Location = new System.Drawing.Point(3, 136);
            this.TrackBorderColorPicker.Name = "TrackBorderColorPicker";
            this.TrackBorderColorPicker.SelectedColor = System.Drawing.Color.Black;
            this.TrackBorderColorPicker.Size = new System.Drawing.Size(295, 23);
            this.TrackBorderColorPicker.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(295, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Track border";
            // 
            // numInner
            // 
            this.numInner.Dock = System.Windows.Forms.DockStyle.Top;
            this.numInner.Location = new System.Drawing.Point(3, 97);
            this.numInner.Name = "numInner";
            this.numInner.Size = new System.Drawing.Size(295, 20);
            this.numInner.TabIndex = 13;
            this.numInner.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(295, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Track width";
            // 
            // TrackColorPicker
            // 
            this.TrackColorPicker.Dock = System.Windows.Forms.DockStyle.Top;
            this.TrackColorPicker.Location = new System.Drawing.Point(3, 55);
            this.TrackColorPicker.Name = "TrackColorPicker";
            this.TrackColorPicker.SelectedColor = System.Drawing.Color.White;
            this.TrackColorPicker.Size = new System.Drawing.Size(295, 23);
            this.TrackColorPicker.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Track color";
            // 
            // numTrackZoom
            // 
            this.numTrackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTrackZoom.AutoSize = true;
            this.numTrackZoom.DecimalPlaces = 2;
            this.numTrackZoom.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numTrackZoom.Location = new System.Drawing.Point(3, 16);
            this.numTrackZoom.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTrackZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numTrackZoom.Name = "numTrackZoom";
            this.numTrackZoom.Size = new System.Drawing.Size(295, 20);
            this.numTrackZoom.TabIndex = 3;
            this.numTrackZoom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scale factor";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(307, 483);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Sectors";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.sectorSeparatorColorPicker, 0, 8);
            this.tableLayoutPanel5.Controls.Add(this.label11, 0, 7);
            this.tableLayoutPanel5.Controls.Add(this.numSectorSeparatorHeight, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.numSectorSeparatorWidth, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.cbDrawSectorSeparators, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.AlternateSectorMember, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 9;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(307, 483);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // sectorSeparatorColorPicker
            // 
            this.sectorSeparatorColorPicker.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectorSeparatorColorPicker.Enabled = false;
            this.sectorSeparatorColorPicker.Location = new System.Drawing.Point(3, 162);
            this.sectorSeparatorColorPicker.Name = "sectorSeparatorColorPicker";
            this.sectorSeparatorColorPicker.SelectedColor = System.Drawing.Color.DarkRed;
            this.sectorSeparatorColorPicker.Size = new System.Drawing.Size(301, 23);
            this.sectorSeparatorColorPicker.TabIndex = 30;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 146);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Sector separator color";
            // 
            // numSectorSeparatorHeight
            // 
            this.numSectorSeparatorHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.numSectorSeparatorHeight.Enabled = false;
            this.numSectorSeparatorHeight.Location = new System.Drawing.Point(3, 123);
            this.numSectorSeparatorHeight.Name = "numSectorSeparatorHeight";
            this.numSectorSeparatorHeight.Size = new System.Drawing.Size(301, 20);
            this.numSectorSeparatorHeight.TabIndex = 28;
            this.numSectorSeparatorHeight.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Sector separator height";
            // 
            // numSectorSeparatorWidth
            // 
            this.numSectorSeparatorWidth.Dock = System.Windows.Forms.DockStyle.Top;
            this.numSectorSeparatorWidth.Enabled = false;
            this.numSectorSeparatorWidth.Location = new System.Drawing.Point(3, 84);
            this.numSectorSeparatorWidth.Name = "numSectorSeparatorWidth";
            this.numSectorSeparatorWidth.Size = new System.Drawing.Size(301, 20);
            this.numSectorSeparatorWidth.TabIndex = 26;
            this.numSectorSeparatorWidth.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Sector separator width";
            // 
            // cbDrawSectorSeparators
            // 
            this.cbDrawSectorSeparators.AutoSize = true;
            this.cbDrawSectorSeparators.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbDrawSectorSeparators.Location = new System.Drawing.Point(3, 48);
            this.cbDrawSectorSeparators.Name = "cbDrawSectorSeparators";
            this.cbDrawSectorSeparators.Size = new System.Drawing.Size(301, 17);
            this.cbDrawSectorSeparators.TabIndex = 24;
            this.cbDrawSectorSeparators.Text = "Sector separators";
            this.cbDrawSectorSeparators.UseVisualStyleBackColor = true;
            this.cbDrawSectorSeparators.CheckedChanged += new System.EventHandler(this.cbDrawSectorSeparators_CheckedChanged);
            // 
            // AlternateSectorMember
            // 
            this.AlternateSectorMember.Dock = System.Windows.Forms.DockStyle.Top;
            this.AlternateSectorMember.Location = new System.Drawing.Point(3, 19);
            this.AlternateSectorMember.Name = "AlternateSectorMember";
            this.AlternateSectorMember.SelectedColor = System.Drawing.Color.LightGray;
            this.AlternateSectorMember.Size = new System.Drawing.Size(301, 23);
            this.AlternateSectorMember.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(301, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Alternate sector color";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel6);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(307, 483);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "Turns";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.numTurnAngle, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.TurnColorPicker, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.cbHighlightTurns, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 6;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(307, 483);
            this.tableLayoutPanel6.TabIndex = 4;
            // 
            // numTurnAngle
            // 
            this.numTurnAngle.Dock = System.Windows.Forms.DockStyle.Top;
            this.numTurnAngle.Location = new System.Drawing.Point(3, 81);
            this.numTurnAngle.Name = "numTurnAngle";
            this.numTurnAngle.Size = new System.Drawing.Size(301, 20);
            this.numTurnAngle.TabIndex = 23;
            this.numTurnAngle.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(301, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Turn angle threshhold";
            // 
            // TurnColorPicker
            // 
            this.TurnColorPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TurnColorPicker.Location = new System.Drawing.Point(3, 39);
            this.TurnColorPicker.Name = "TurnColorPicker";
            this.TurnColorPicker.SelectedColor = System.Drawing.Color.DarkRed;
            this.TurnColorPicker.Size = new System.Drawing.Size(301, 23);
            this.TurnColorPicker.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Turns color";
            // 
            // cbHighlightTurns
            // 
            this.cbHighlightTurns.AutoSize = true;
            this.cbHighlightTurns.Checked = true;
            this.cbHighlightTurns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHighlightTurns.Location = new System.Drawing.Point(3, 3);
            this.cbHighlightTurns.Name = "cbHighlightTurns";
            this.cbHighlightTurns.Size = new System.Drawing.Size(93, 17);
            this.cbHighlightTurns.TabIndex = 24;
            this.cbHighlightTurns.Text = "Highlight turns";
            this.cbHighlightTurns.UseVisualStyleBackColor = true;
            this.cbHighlightTurns.CheckedChanged += new System.EventHandler(this.cbHighlightTurns_CheckedChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGenerate.Location = new System.Drawing.Point(3, 518);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(315, 29);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Preview";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(330, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 550);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(661, 550);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tbZoom, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblZoom, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(327, 556);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(667, 40);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // tbZoom
            // 
            this.tbZoom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbZoom.LargeChange = 10;
            this.tbZoom.Location = new System.Drawing.Point(3, 3);
            this.tbZoom.Maximum = 100;
            this.tbZoom.Minimum = 10;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(605, 34);
            this.tbZoom.SmallChange = 5;
            this.tbZoom.TabIndex = 8;
            this.tbZoom.Value = 50;
            this.tbZoom.ValueChanged += new System.EventHandler(this.tbZoom_ValueChanged);
            // 
            // lblZoom
            // 
            this.lblZoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblZoom.Location = new System.Drawing.Point(614, 0);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(50, 40);
            this.lblZoom.TabIndex = 9;
            this.lblZoom.Text = "100%";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOutputSize
            // 
            this.lblOutputSize.AutoSize = true;
            this.lblOutputSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutputSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputSize.Location = new System.Drawing.Point(3, 556);
            this.lblOutputSize.Name = "lblOutputSize";
            this.lblOutputSize.Size = new System.Drawing.Size(321, 40);
            this.lblOutputSize.TabIndex = 10;
            this.lblOutputSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sourceFile
            // 
            this.sourceFile.FileName = "*.json";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.recordToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(994, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTrackPositionDumpToolStripMenuItem,
            this.exportMapToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openTrackPositionDumpToolStripMenuItem
            // 
            this.openTrackPositionDumpToolStripMenuItem.Name = "openTrackPositionDumpToolStripMenuItem";
            this.openTrackPositionDumpToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.openTrackPositionDumpToolStripMenuItem.Text = "Open track record";
            this.openTrackPositionDumpToolStripMenuItem.Click += new System.EventHandler(this.openTrackPositionDumpToolStripMenuItem_Click);
            // 
            // exportMapToolStripMenuItem
            // 
            this.exportMapToolStripMenuItem.Name = "exportMapToolStripMenuItem";
            this.exportMapToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.exportMapToolStripMenuItem.Text = "Export map";
            this.exportMapToolStripMenuItem.Click += new System.EventHandler(this.exportMapToolStripMenuItem_Click);
            // 
            // recordToolStripMenuItem
            // 
            this.recordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordMapDataToolStripMenuItem});
            this.recordToolStripMenuItem.Name = "recordToolStripMenuItem";
            this.recordToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.recordToolStripMenuItem.Text = "Record";
            // 
            // recordMapDataToolStripMenuItem
            // 
            this.recordMapDataToolStripMenuItem.Name = "recordMapDataToolStripMenuItem";
            this.recordMapDataToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.recordMapDataToolStripMenuItem.Text = "Record map data";
            this.recordMapDataToolStripMenuItem.Click += new System.EventHandler(this.recordMapDataToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MapPreview
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 620);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Name = "MapPreview";
            this.Text = "Map Generator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopFix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOuter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrackZoom)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSectorSeparatorHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSectorSeparatorWidth)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTurnAngle)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbZoom)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog sourceFile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTrackPositionDumpToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TrackBar tbZoom;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ToolStripMenuItem exportMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordMapDataToolStripMenuItem;
        private System.Windows.Forms.Label lblOutputSize;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.NumericUpDown numLoopFix;
        private System.Windows.Forms.Label lblLoopFix;
        private System.Windows.Forms.NumericUpDown numOuter;
        private System.Windows.Forms.Label label5;
        private ColorPicker TrackBorderColorPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numInner;
        private System.Windows.Forms.Label label4;
        private ColorPicker TrackColorPicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numTrackZoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private ColorPicker sectorSeparatorColorPicker;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numSectorSeparatorHeight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numSectorSeparatorWidth;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbDrawSectorSeparators;
        private ColorPicker AlternateSectorMember;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.NumericUpDown numTurnAngle;
        private System.Windows.Forms.Label label7;
        private ColorPicker TurnColorPicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbHighlightTurns;
    }
}