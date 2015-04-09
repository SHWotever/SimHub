namespace TimingClient.Plugins.UI
{
    partial class PluginManagerUI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lstEventMapping = new System.Windows.Forms.ListBox();
            this.btnAddEventMapping = new System.Windows.Forms.Button();
            this.btnRemoveEventMapping = new System.Windows.Forms.Button();
            this.btnEditEventMapping = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstInputMappings = new System.Windows.Forms.ListBox();
            this.btnAddActionMapping = new System.Windows.Forms.Button();
            this.btnRemoveInputMapping = new System.Windows.Forms.Button();
            this.btnEditInputMapping = new System.Windows.Forms.Button();
            this.tabInputs = new System.Windows.Forms.TabPage();
            this.InputSettings = new TimingClient.Plugins.UI.PluginsSettings();
            this.tabData = new System.Windows.Forms.TabPage();
            this.DataSettings = new TimingClient.Plugins.UI.PluginsSettings();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.outputSettings = new TimingClient.Plugins.UI.PluginsSettings();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doSampleDataSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabInputs.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabInputs);
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Controls.Add(this.tabOutput);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(906, 711);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.tableLayoutPanel2);
            this.tabGeneral.Controls.Add(this.menuStrip1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(898, 685);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(892, 655);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(886, 322);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Events mapping";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.lstEventMapping, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnAddEventMapping, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnRemoveEventMapping, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnEditEventMapping, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(880, 303);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // lstEventMapping
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lstEventMapping, 3);
            this.lstEventMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEventMapping.FormattingEnabled = true;
            this.lstEventMapping.Location = new System.Drawing.Point(3, 3);
            this.lstEventMapping.Name = "lstEventMapping";
            this.lstEventMapping.Size = new System.Drawing.Size(874, 268);
            this.lstEventMapping.TabIndex = 0;
            // 
            // btnAddEventMapping
            // 
            this.btnAddEventMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddEventMapping.Location = new System.Drawing.Point(589, 277);
            this.btnAddEventMapping.Name = "btnAddEventMapping";
            this.btnAddEventMapping.Size = new System.Drawing.Size(288, 23);
            this.btnAddEventMapping.TabIndex = 1;
            this.btnAddEventMapping.Text = "Add";
            this.btnAddEventMapping.UseVisualStyleBackColor = true;
            this.btnAddEventMapping.Click += new System.EventHandler(this.btnAddEventMapping_Click);
            // 
            // btnRemoveEventMapping
            // 
            this.btnRemoveEventMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemoveEventMapping.Location = new System.Drawing.Point(3, 277);
            this.btnRemoveEventMapping.Name = "btnRemoveEventMapping";
            this.btnRemoveEventMapping.Size = new System.Drawing.Size(287, 23);
            this.btnRemoveEventMapping.TabIndex = 2;
            this.btnRemoveEventMapping.Text = "Remove";
            this.btnRemoveEventMapping.UseVisualStyleBackColor = true;
            this.btnRemoveEventMapping.Click += new System.EventHandler(this.btnRemoveEventMapping_Click);
            // 
            // btnEditEventMapping
            // 
            this.btnEditEventMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditEventMapping.Location = new System.Drawing.Point(296, 277);
            this.btnEditEventMapping.Name = "btnEditEventMapping";
            this.btnEditEventMapping.Size = new System.Drawing.Size(287, 23);
            this.btnEditEventMapping.TabIndex = 3;
            this.btnEditEventMapping.Text = "Edit";
            this.btnEditEventMapping.UseVisualStyleBackColor = true;
            this.btnEditEventMapping.Click += new System.EventHandler(this.btnEditEventMapping_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(886, 321);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input mapping";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.lstInputMappings, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddActionMapping, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveInputMapping, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEditInputMapping, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(880, 302);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lstInputMappings
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstInputMappings, 3);
            this.lstInputMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInputMappings.FormattingEnabled = true;
            this.lstInputMappings.Location = new System.Drawing.Point(3, 3);
            this.lstInputMappings.Name = "lstInputMappings";
            this.lstInputMappings.Size = new System.Drawing.Size(874, 267);
            this.lstInputMappings.TabIndex = 0;
            // 
            // btnAddActionMapping
            // 
            this.btnAddActionMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddActionMapping.Location = new System.Drawing.Point(589, 276);
            this.btnAddActionMapping.Name = "btnAddActionMapping";
            this.btnAddActionMapping.Size = new System.Drawing.Size(288, 23);
            this.btnAddActionMapping.TabIndex = 1;
            this.btnAddActionMapping.Text = "Add";
            this.btnAddActionMapping.UseVisualStyleBackColor = true;
            this.btnAddActionMapping.Click += new System.EventHandler(this.btnAddActionMapping_Click);
            // 
            // btnRemoveInputMapping
            // 
            this.btnRemoveInputMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRemoveInputMapping.Location = new System.Drawing.Point(3, 276);
            this.btnRemoveInputMapping.Name = "btnRemoveInputMapping";
            this.btnRemoveInputMapping.Size = new System.Drawing.Size(287, 23);
            this.btnRemoveInputMapping.TabIndex = 2;
            this.btnRemoveInputMapping.Text = "Remove";
            this.btnRemoveInputMapping.UseVisualStyleBackColor = true;
            this.btnRemoveInputMapping.Click += new System.EventHandler(this.btnRemoveInputMapping_Click);
            // 
            // btnEditInputMapping
            // 
            this.btnEditInputMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditInputMapping.Location = new System.Drawing.Point(296, 276);
            this.btnEditInputMapping.Name = "btnEditInputMapping";
            this.btnEditInputMapping.Size = new System.Drawing.Size(287, 23);
            this.btnEditInputMapping.TabIndex = 3;
            this.btnEditInputMapping.Text = "Edit";
            this.btnEditInputMapping.UseVisualStyleBackColor = true;
            this.btnEditInputMapping.Click += new System.EventHandler(this.btnEditInputMapping_Click);
            // 
            // tabInputs
            // 
            this.tabInputs.Controls.Add(this.InputSettings);
            this.tabInputs.Location = new System.Drawing.Point(4, 22);
            this.tabInputs.Name = "tabInputs";
            this.tabInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabInputs.Size = new System.Drawing.Size(682, 685);
            this.tabInputs.TabIndex = 1;
            this.tabInputs.Text = "Inputs";
            this.tabInputs.UseVisualStyleBackColor = true;
            // 
            // InputSettings
            // 
            this.InputSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputSettings.Location = new System.Drawing.Point(3, 3);
            this.InputSettings.Name = "InputSettings";
            this.InputSettings.Size = new System.Drawing.Size(676, 679);
            this.InputSettings.TabIndex = 0;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.DataSettings);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(3);
            this.tabData.Size = new System.Drawing.Size(898, 685);
            this.tabData.TabIndex = 2;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // DataSettings
            // 
            this.DataSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSettings.Location = new System.Drawing.Point(3, 3);
            this.DataSettings.Name = "DataSettings";
            this.DataSettings.Size = new System.Drawing.Size(892, 679);
            this.DataSettings.TabIndex = 0;
            // 
            // tabOutput
            // 
            this.tabOutput.Controls.Add(this.outputSettings);
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Size = new System.Drawing.Size(898, 685);
            this.tabOutput.TabIndex = 3;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // outputSettings
            // 
            this.outputSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputSettings.Location = new System.Drawing.Point(0, 0);
            this.outputSettings.Name = "outputSettings";
            this.outputSettings.Size = new System.Drawing.Size(898, 685);
            this.outputSettings.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(892, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doSampleDataSnapshotToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // doSampleDataSnapshotToolStripMenuItem
            // 
            this.doSampleDataSnapshotToolStripMenuItem.Name = "doSampleDataSnapshotToolStripMenuItem";
            this.doSampleDataSnapshotToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.doSampleDataSnapshotToolStripMenuItem.Text = "Do sample data snapshot";
            // 
            // PluginManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PluginManagerUI";
            this.Size = new System.Drawing.Size(906, 711);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabInputs.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabInputs;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.TabPage tabOutput;
        private PluginsSettings InputSettings;
        private PluginsSettings DataSettings;
        private PluginsSettings outputSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lstInputMappings;
        private System.Windows.Forms.Button btnAddActionMapping;
        private System.Windows.Forms.Button btnRemoveInputMapping;
        private System.Windows.Forms.Button btnEditInputMapping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox lstEventMapping;
        private System.Windows.Forms.Button btnAddEventMapping;
        private System.Windows.Forms.Button btnRemoveEventMapping;
        private System.Windows.Forms.Button btnEditEventMapping;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doSampleDataSnapshotToolStripMenuItem;
    }
}
