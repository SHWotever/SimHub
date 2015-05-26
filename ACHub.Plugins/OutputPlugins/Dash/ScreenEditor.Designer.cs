namespace ACHub.Plugins.OutputPlugins.Dash
{
    partial class ScreenEditor
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbInGameScreen = new System.Windows.Forms.CheckBox();
            this.cbIdleScreen = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numBlinkTime = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Announce = new System.Windows.Forms.GroupBox();
            this.flpAnnounce = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.screenAnnounceEditor1 = new ACHub.Plugins.OutputPlugins.Dash.ScreenAnnounceEditor();
            this.screenAnnounceEditor2 = new ACHub.Plugins.OutputPlugins.Dash.ScreenAnnounceEditor();
            this.screenAnnounceEditor3 = new ACHub.Plugins.OutputPlugins.Dash.ScreenAnnounceEditor();
            this.screenAnnounceEditor4 = new ACHub.Plugins.OutputPlugins.Dash.ScreenAnnounceEditor();
            this.tabControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlinkTime)).BeginInit();
            this.Announce.SuspendLayout();
            this.flpAnnounce.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Screen name";
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(79, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(901, 20);
            this.txtName.TabIndex = 19;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(971, 412);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(963, 386);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(963, 386);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(905, 617);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 22);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Announce, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(983, 642);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.cbInGameScreen);
            this.flowLayoutPanel1.Controls.Add(this.cbIdleScreen);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.numBlinkTime);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(983, 26);
            this.flowLayoutPanel1.TabIndex = 22;
            // 
            // cbInGameScreen
            // 
            this.cbInGameScreen.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbInGameScreen.AutoSize = true;
            this.cbInGameScreen.Location = new System.Drawing.Point(3, 4);
            this.cbInGameScreen.Name = "cbInGameScreen";
            this.cbInGameScreen.Size = new System.Drawing.Size(212, 17);
            this.cbInGameScreen.TabIndex = 21;
            this.cbInGameScreen.Text = "Include this screen when AC is running ";
            this.cbInGameScreen.UseVisualStyleBackColor = true;
            // 
            // cbIdleScreen
            // 
            this.cbIdleScreen.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbIdleScreen.AutoSize = true;
            this.cbIdleScreen.Location = new System.Drawing.Point(221, 4);
            this.cbIdleScreen.Name = "cbIdleScreen";
            this.cbIdleScreen.Size = new System.Drawing.Size(230, 17);
            this.cbIdleScreen.TabIndex = 22;
            this.cbIdleScreen.Text = "Include this screen when AC is not running ";
            this.cbIdleScreen.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(457, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Blink interval (0 = disabled)";
            // 
            // numBlinkTime
            // 
            this.numBlinkTime.Location = new System.Drawing.Point(596, 3);
            this.numBlinkTime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numBlinkTime.Name = "numBlinkTime";
            this.numBlinkTime.Size = new System.Drawing.Size(79, 20);
            this.numBlinkTime.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(681, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "ms";
            // 
            // Announce
            // 
            this.Announce.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.Announce, 2);
            this.Announce.Controls.Add(this.flpAnnounce);
            this.Announce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Announce.Location = new System.Drawing.Point(3, 55);
            this.Announce.Name = "Announce";
            this.Announce.Size = new System.Drawing.Size(977, 119);
            this.Announce.TabIndex = 24;
            this.Announce.TabStop = false;
            this.Announce.Text = "Announce content (leave empty for no announce)";
            // 
            // flpAnnounce
            // 
            this.flpAnnounce.AutoSize = true;
            this.flpAnnounce.Controls.Add(this.screenAnnounceEditor1);
            this.flpAnnounce.Controls.Add(this.screenAnnounceEditor2);
            this.flpAnnounce.Controls.Add(this.screenAnnounceEditor3);
            this.flpAnnounce.Controls.Add(this.screenAnnounceEditor4);
            this.flpAnnounce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAnnounce.Location = new System.Drawing.Point(3, 16);
            this.flpAnnounce.Name = "flpAnnounce";
            this.flpAnnounce.Size = new System.Drawing.Size(971, 100);
            this.flpAnnounce.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(977, 431);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Live content";
            // 
            // screenAnnounceEditor1
            // 
            this.screenAnnounceEditor1.Location = new System.Drawing.Point(3, 3);
            this.screenAnnounceEditor1.Name = "screenAnnounceEditor1";
            this.screenAnnounceEditor1.Size = new System.Drawing.Size(177, 94);
            this.screenAnnounceEditor1.TabIndex = 0;
            this.screenAnnounceEditor1.Title = "Module 1";
            this.screenAnnounceEditor1.Value = "";
            // 
            // screenAnnounceEditor2
            // 
            this.screenAnnounceEditor2.Location = new System.Drawing.Point(186, 3);
            this.screenAnnounceEditor2.Name = "screenAnnounceEditor2";
            this.screenAnnounceEditor2.Size = new System.Drawing.Size(177, 94);
            this.screenAnnounceEditor2.TabIndex = 1;
            this.screenAnnounceEditor2.Title = "Module 2";
            this.screenAnnounceEditor2.Value = "";
            // 
            // screenAnnounceEditor3
            // 
            this.screenAnnounceEditor3.Location = new System.Drawing.Point(369, 3);
            this.screenAnnounceEditor3.Name = "screenAnnounceEditor3";
            this.screenAnnounceEditor3.Size = new System.Drawing.Size(177, 94);
            this.screenAnnounceEditor3.TabIndex = 2;
            this.screenAnnounceEditor3.Title = "Module 3";
            this.screenAnnounceEditor3.Value = "";
            // 
            // screenAnnounceEditor4
            // 
            this.screenAnnounceEditor4.Location = new System.Drawing.Point(552, 3);
            this.screenAnnounceEditor4.Name = "screenAnnounceEditor4";
            this.screenAnnounceEditor4.Size = new System.Drawing.Size(177, 94);
            this.screenAnnounceEditor4.TabIndex = 3;
            this.screenAnnounceEditor4.Title = "Module 4";
            this.screenAnnounceEditor4.Value = "";
            // 
            // ScreenEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 642);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScreenEditor";
            this.Text = "ScreenEditor";
            this.tabControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBlinkTime)).EndInit();
            this.Announce.ResumeLayout(false);
            this.Announce.PerformLayout();
            this.flpAnnounce.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox cbInGameScreen;
        private System.Windows.Forms.CheckBox cbIdleScreen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numBlinkTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox Announce;
        private System.Windows.Forms.FlowLayoutPanel flpAnnounce;
        private ScreenAnnounceEditor screenAnnounceEditor1;
        private ScreenAnnounceEditor screenAnnounceEditor2;
        private ScreenAnnounceEditor screenAnnounceEditor3;
        private ScreenAnnounceEditor screenAnnounceEditor4;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}