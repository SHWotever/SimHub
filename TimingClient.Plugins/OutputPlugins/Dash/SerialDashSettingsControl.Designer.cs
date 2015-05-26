namespace ACHub.Plugins.OutputPlugins.Dash
{
    partial class SerialDashSettingsControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lstScreens = new ACToolsUtilities.UI.RefreshingListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewScreen = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDetectedModules = new System.Windows.Forms.Label();
            this.cbReverseModule4 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule3 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule2 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ledEditor2 = new ACHub.Plugins.OutputPlugins.Dash.LedEditor();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.numRPMOffset = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.numRPMBlink = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.numLowFuelLaps = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.numLowFuelRepeatInterval = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.detectTimer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.numIntensity = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRPMOffset)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRPMBlink)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLowFuelLaps)).BeginInit();
            this.flowLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLowFuelRepeatInterval)).BeginInit();
            this.flowLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 820);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(153, 3);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel1.SetRowSpan(this.groupBox2, 2);
            this.groupBox2.Size = new System.Drawing.Size(784, 404);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Screens";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lstScreens, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(778, 385);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // lstScreens
            // 
            this.lstScreens.DisplayMember = "Description";
            this.lstScreens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScreens.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstScreens.FormattingEnabled = true;
            this.lstScreens.ItemHeight = 14;
            this.lstScreens.Location = new System.Drawing.Point(3, 3);
            this.lstScreens.Name = "lstScreens";
            this.lstScreens.Size = new System.Drawing.Size(772, 350);
            this.lstScreens.TabIndex = 3;
            this.lstScreens.SelectedIndexChanged += new System.EventHandler(this.refreshingListBox1_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnNewScreen);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Controls.Add(this.btnUp);
            this.flowLayoutPanel1.Controls.Add(this.btnDown);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 356);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(778, 29);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnNewScreen
            // 
            this.btnNewScreen.Location = new System.Drawing.Point(3, 3);
            this.btnNewScreen.Name = "btnNewScreen";
            this.btnNewScreen.Size = new System.Drawing.Size(75, 23);
            this.btnNewScreen.TabIndex = 2;
            this.btnNewScreen.Text = "New";
            this.btnNewScreen.UseVisualStyleBackColor = true;
            this.btnNewScreen.Click += new System.EventHandler(this.btnNewScreen_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(84, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(165, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Remove";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(246, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 6;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(327, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel7);
            this.groupBox1.Controls.Add(this.lblDetectedModules);
            this.groupBox1.Controls.Add(this.cbReverseModule4);
            this.groupBox1.Controls.Add(this.cbReverseModule3);
            this.groupBox1.Controls.Add(this.cbReverseModule2);
            this.groupBox1.Controls.Add(this.cbReverseModule1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modules settings";
            // 
            // lblDetectedModules
            // 
            this.lblDetectedModules.AutoSize = true;
            this.lblDetectedModules.Location = new System.Drawing.Point(-3, 134);
            this.lblDetectedModules.Name = "lblDetectedModules";
            this.lblDetectedModules.Size = new System.Drawing.Size(102, 13);
            this.lblDetectedModules.TabIndex = 4;
            this.lblDetectedModules.Text = "Detected modules : ";
            this.lblDetectedModules.Click += new System.EventHandler(this.lblDetectedModules_Click);
            // 
            // cbReverseModule4
            // 
            this.cbReverseModule4.AutoSize = true;
            this.cbReverseModule4.Location = new System.Drawing.Point(3, 85);
            this.cbReverseModule4.Name = "cbReverseModule4";
            this.cbReverseModule4.Size = new System.Drawing.Size(112, 17);
            this.cbReverseModule4.TabIndex = 3;
            this.cbReverseModule4.Text = "Reverse module 4";
            this.cbReverseModule4.UseVisualStyleBackColor = true;
            // 
            // cbReverseModule3
            // 
            this.cbReverseModule3.AutoSize = true;
            this.cbReverseModule3.Location = new System.Drawing.Point(3, 62);
            this.cbReverseModule3.Name = "cbReverseModule3";
            this.cbReverseModule3.Size = new System.Drawing.Size(112, 17);
            this.cbReverseModule3.TabIndex = 2;
            this.cbReverseModule3.Text = "Reverse module 3";
            this.cbReverseModule3.UseVisualStyleBackColor = true;
            // 
            // cbReverseModule2
            // 
            this.cbReverseModule2.AutoSize = true;
            this.cbReverseModule2.Location = new System.Drawing.Point(3, 39);
            this.cbReverseModule2.Name = "cbReverseModule2";
            this.cbReverseModule2.Size = new System.Drawing.Size(112, 17);
            this.cbReverseModule2.TabIndex = 1;
            this.cbReverseModule2.Text = "Reverse module 2";
            this.cbReverseModule2.UseVisualStyleBackColor = true;
            // 
            // cbReverseModule1
            // 
            this.cbReverseModule1.AutoSize = true;
            this.cbReverseModule1.Location = new System.Drawing.Point(3, 16);
            this.cbReverseModule1.Name = "cbReverseModule1";
            this.cbReverseModule1.Size = new System.Drawing.Size(112, 17);
            this.cbReverseModule1.TabIndex = 0;
            this.cbReverseModule1.Text = "Reverse module 1";
            this.cbReverseModule1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.ledEditor2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 413);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(934, 404);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Leds";
            // 
            // ledEditor2
            // 
            this.ledEditor2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledEditor2.Location = new System.Drawing.Point(3, 16);
            this.ledEditor2.Name = "ledEditor2";
            this.ledEditor2.Size = new System.Drawing.Size(928, 385);
            this.ledEditor2.TabIndex = 9;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.flowLayoutPanel2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 159);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(144, 248);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Limits";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel5);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel2.Controls.Add(this.label7);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel6);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(138, 229);
            this.flowLayoutPanel2.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "RPM Start : ";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.AutoSize = true;
            this.flowLayoutPanel5.Controls.Add(this.numRPMOffset);
            this.flowLayoutPanel5.Controls.Add(this.label6);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(125, 26);
            this.flowLayoutPanel5.TabIndex = 13;
            // 
            // numRPMOffset
            // 
            this.numRPMOffset.DecimalPlaces = 1;
            this.numRPMOffset.Location = new System.Drawing.Point(3, 3);
            this.numRPMOffset.Name = "numRPMOffset";
            this.numRPMOffset.Size = new System.Drawing.Size(54, 20);
            this.numRPMOffset.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Max RPM %";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "RPM Blink";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.numRPMBlink);
            this.flowLayoutPanel4.Controls.Add(this.label5);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 61);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(125, 26);
            this.flowLayoutPanel4.TabIndex = 12;
            // 
            // numRPMBlink
            // 
            this.numRPMBlink.DecimalPlaces = 1;
            this.numRPMBlink.Location = new System.Drawing.Point(3, 3);
            this.numRPMBlink.Name = "numRPMBlink";
            this.numRPMBlink.Size = new System.Drawing.Size(54, 20);
            this.numRPMBlink.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Max RPM %";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Low fuel alert level";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.numLowFuelLaps);
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 106);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(113, 26);
            this.flowLayoutPanel3.TabIndex = 11;
            // 
            // numLowFuelLaps
            // 
            this.numLowFuelLaps.DecimalPlaces = 1;
            this.numLowFuelLaps.Location = new System.Drawing.Point(3, 3);
            this.numLowFuelLaps.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numLowFuelLaps.Name = "numLowFuelLaps";
            this.numLowFuelLaps.Size = new System.Drawing.Size(54, 20);
            this.numLowFuelLaps.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Laps rem.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Repeat low fuel alert";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.AutoSize = true;
            this.flowLayoutPanel6.Controls.Add(this.numLowFuelRepeatInterval);
            this.flowLayoutPanel6.Controls.Add(this.label8);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 151);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(107, 26);
            this.flowLayoutPanel6.TabIndex = 15;
            // 
            // numLowFuelRepeatInterval
            // 
            this.numLowFuelRepeatInterval.Location = new System.Drawing.Point(3, 3);
            this.numLowFuelRepeatInterval.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numLowFuelRepeatInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLowFuelRepeatInterval.Name = "numLowFuelRepeatInterval";
            this.numLowFuelRepeatInterval.Size = new System.Drawing.Size(54, 20);
            this.numLowFuelRepeatInterval.TabIndex = 10;
            this.numLowFuelRepeatInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(60, 6);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "seconds";
            // 
            // detectTimer
            // 
            this.detectTimer.Enabled = true;
            this.detectTimer.Interval = 2000;
            this.detectTimer.Tick += new System.EventHandler(this.detectTimer_Tick);
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.AutoSize = true;
            this.flowLayoutPanel7.Controls.Add(this.label9);
            this.flowLayoutPanel7.Controls.Add(this.numIntensity);
            this.flowLayoutPanel7.Location = new System.Drawing.Point(3, 105);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Size = new System.Drawing.Size(125, 26);
            this.flowLayoutPanel7.TabIndex = 14;
            // 
            // numIntensity
            // 
            this.numIntensity.Location = new System.Drawing.Point(49, 3);
            this.numIntensity.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numIntensity.Name = "numIntensity";
            this.numIntensity.Size = new System.Drawing.Size(54, 20);
            this.numIntensity.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Intensity";
            // 
            // SerialDashSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SerialDashSettingsControl";
            this.Size = new System.Drawing.Size(940, 820);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRPMOffset)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRPMBlink)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLowFuelLaps)).EndInit();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLowFuelRepeatInterval)).EndInit();
            this.flowLayoutPanel7.ResumeLayout(false);
            this.flowLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIntensity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDetectedModules;
        private System.Windows.Forms.CheckBox cbReverseModule4;
        private System.Windows.Forms.CheckBox cbReverseModule3;
        private System.Windows.Forms.CheckBox cbReverseModule2;
        private System.Windows.Forms.CheckBox cbReverseModule1;
        private System.Windows.Forms.Timer detectTimer;
        private System.Windows.Forms.Button btnNewScreen;
        private ACToolsUtilities.UI.RefreshingListBox lstScreens;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox3;
        private LedEditor ledEditor2;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.NumericUpDown numRPMOffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numRPMBlink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numLowFuelLaps;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.NumericUpDown numLowFuelRepeatInterval;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numIntensity;
    }
}
