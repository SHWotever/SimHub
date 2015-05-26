namespace ACHub.Plugins.UI
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
            this.tabInputs = new System.Windows.Forms.TabPage();
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doSampleDataSnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.btnDownInput = new System.Windows.Forms.Button();
            this.btnUpInput = new System.Windows.Forms.Button();
            this.btnUpEvent = new System.Windows.Forms.Button();
            this.btnDownEvent = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.InputSettings = new ACHub.Plugins.UI.PluginsSettings();
            this.tabInputs.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabInputs
            // 
            this.tabInputs.Controls.Add(this.InputSettings);
            this.tabInputs.ForeColor = System.Drawing.Color.Black;
            this.tabInputs.Location = new System.Drawing.Point(4, 22);
            this.tabInputs.Name = "tabInputs";
            this.tabInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tabInputs.Size = new System.Drawing.Size(898, 685);
            this.tabInputs.TabIndex = 1;
            this.tabInputs.Text = "Plugins";
            this.tabInputs.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(892, 679);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 342);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(886, 334);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Events mapping";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lstEventMapping, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(880, 315);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // lstEventMapping
            // 
            this.lstEventMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEventMapping.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstEventMapping.FormattingEnabled = true;
            this.lstEventMapping.ItemHeight = 14;
            this.lstEventMapping.Location = new System.Drawing.Point(3, 3);
            this.lstEventMapping.Name = "lstEventMapping";
            this.lstEventMapping.Size = new System.Drawing.Size(874, 280);
            this.lstEventMapping.TabIndex = 0;
            // 
            // btnAddEventMapping
            // 
            this.btnAddEventMapping.Location = new System.Drawing.Point(127, 3);
            this.btnAddEventMapping.Name = "btnAddEventMapping";
            this.btnAddEventMapping.Size = new System.Drawing.Size(56, 23);
            this.btnAddEventMapping.TabIndex = 1;
            this.btnAddEventMapping.Text = "New";
            this.btnAddEventMapping.UseVisualStyleBackColor = true;
            this.btnAddEventMapping.Click += new System.EventHandler(this.btnAddEventMapping_Click);
            // 
            // btnRemoveEventMapping
            // 
            this.btnRemoveEventMapping.Location = new System.Drawing.Point(251, 3);
            this.btnRemoveEventMapping.Name = "btnRemoveEventMapping";
            this.btnRemoveEventMapping.Size = new System.Drawing.Size(56, 23);
            this.btnRemoveEventMapping.TabIndex = 2;
            this.btnRemoveEventMapping.Text = "Remove";
            this.btnRemoveEventMapping.UseVisualStyleBackColor = true;
            this.btnRemoveEventMapping.Click += new System.EventHandler(this.btnRemoveEventMapping_Click);
            // 
            // btnEditEventMapping
            // 
            this.btnEditEventMapping.Location = new System.Drawing.Point(189, 3);
            this.btnEditEventMapping.Name = "btnEditEventMapping";
            this.btnEditEventMapping.Size = new System.Drawing.Size(56, 23);
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
            this.groupBox1.Size = new System.Drawing.Size(886, 333);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input mapping";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lstInputMappings, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(880, 314);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lstInputMappings
            // 
            this.lstInputMappings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInputMappings.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInputMappings.FormattingEnabled = true;
            this.lstInputMappings.ItemHeight = 14;
            this.lstInputMappings.Location = new System.Drawing.Point(3, 3);
            this.lstInputMappings.Name = "lstInputMappings";
            this.lstInputMappings.Size = new System.Drawing.Size(874, 279);
            this.lstInputMappings.TabIndex = 0;
            // 
            // btnAddActionMapping
            // 
            this.btnAddActionMapping.Location = new System.Drawing.Point(127, 3);
            this.btnAddActionMapping.Name = "btnAddActionMapping";
            this.btnAddActionMapping.Size = new System.Drawing.Size(56, 23);
            this.btnAddActionMapping.TabIndex = 1;
            this.btnAddActionMapping.Text = "New";
            this.btnAddActionMapping.UseVisualStyleBackColor = true;
            this.btnAddActionMapping.Click += new System.EventHandler(this.btnAddActionMapping_Click);
            // 
            // btnRemoveInputMapping
            // 
            this.btnRemoveInputMapping.Location = new System.Drawing.Point(251, 3);
            this.btnRemoveInputMapping.Name = "btnRemoveInputMapping";
            this.btnRemoveInputMapping.Size = new System.Drawing.Size(56, 23);
            this.btnRemoveInputMapping.TabIndex = 2;
            this.btnRemoveInputMapping.Text = "Remove";
            this.btnRemoveInputMapping.UseVisualStyleBackColor = true;
            this.btnRemoveInputMapping.Click += new System.EventHandler(this.btnRemoveInputMapping_Click);
            // 
            // btnEditInputMapping
            // 
            this.btnEditInputMapping.Location = new System.Drawing.Point(189, 3);
            this.btnEditInputMapping.Name = "btnEditInputMapping";
            this.btnEditInputMapping.Size = new System.Drawing.Size(56, 23);
            this.btnEditInputMapping.TabIndex = 3;
            this.btnEditInputMapping.Text = "Edit";
            this.btnEditInputMapping.UseVisualStyleBackColor = true;
            this.btnEditInputMapping.Click += new System.EventHandler(this.btnEditInputMapping_Click);
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
            this.menuStrip1.Visible = false;
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabInputs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(906, 711);
            this.tabControl1.TabIndex = 0;
            // 
            // btnDownInput
            // 
            this.btnDownInput.Location = new System.Drawing.Point(65, 3);
            this.btnDownInput.Name = "btnDownInput";
            this.btnDownInput.Size = new System.Drawing.Size(56, 23);
            this.btnDownInput.TabIndex = 4;
            this.btnDownInput.Text = "Down";
            this.btnDownInput.UseVisualStyleBackColor = true;
            this.btnDownInput.Click += new System.EventHandler(this.btnDownInput_Click);
            // 
            // btnUpInput
            // 
            this.btnUpInput.Location = new System.Drawing.Point(3, 3);
            this.btnUpInput.Name = "btnUpInput";
            this.btnUpInput.Size = new System.Drawing.Size(56, 23);
            this.btnUpInput.TabIndex = 5;
            this.btnUpInput.Text = "Up";
            this.btnUpInput.UseVisualStyleBackColor = true;
            this.btnUpInput.Click += new System.EventHandler(this.btnUpInput_Click);
            // 
            // btnUpEvent
            // 
            this.btnUpEvent.Location = new System.Drawing.Point(3, 3);
            this.btnUpEvent.Name = "btnUpEvent";
            this.btnUpEvent.Size = new System.Drawing.Size(56, 23);
            this.btnUpEvent.TabIndex = 4;
            this.btnUpEvent.Text = "Up";
            this.btnUpEvent.UseVisualStyleBackColor = true;
            this.btnUpEvent.Click += new System.EventHandler(this.btnUpEvent_Click);
            // 
            // btnDownEvent
            // 
            this.btnDownEvent.Location = new System.Drawing.Point(65, 3);
            this.btnDownEvent.Name = "btnDownEvent";
            this.btnDownEvent.Size = new System.Drawing.Size(56, 23);
            this.btnDownEvent.TabIndex = 5;
            this.btnDownEvent.Text = "Down";
            this.btnDownEvent.UseVisualStyleBackColor = true;
            this.btnDownEvent.Click += new System.EventHandler(this.btnDownEvent_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnUpInput);
            this.flowLayoutPanel1.Controls.Add(this.btnDownInput);
            this.flowLayoutPanel1.Controls.Add(this.btnAddActionMapping);
            this.flowLayoutPanel1.Controls.Add(this.btnEditInputMapping);
            this.flowLayoutPanel1.Controls.Add(this.btnRemoveInputMapping);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 285);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(880, 29);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.btnUpEvent);
            this.flowLayoutPanel2.Controls.Add(this.btnDownEvent);
            this.flowLayoutPanel2.Controls.Add(this.btnAddEventMapping);
            this.flowLayoutPanel2.Controls.Add(this.btnEditEventMapping);
            this.flowLayoutPanel2.Controls.Add(this.btnRemoveEventMapping);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 286);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(880, 29);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // InputSettings
            // 
            this.InputSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputSettings.Location = new System.Drawing.Point(3, 3);
            this.InputSettings.Name = "InputSettings";
            this.InputSettings.Size = new System.Drawing.Size(892, 679);
            this.InputSettings.TabIndex = 0;
            // 
            // PluginManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PluginManagerUI";
            this.Size = new System.Drawing.Size(906, 711);
            this.tabInputs.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabInputs;
        private PluginsSettings InputSettings;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox lstEventMapping;
        private System.Windows.Forms.Button btnAddEventMapping;
        private System.Windows.Forms.Button btnRemoveEventMapping;
        private System.Windows.Forms.Button btnEditEventMapping;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lstInputMappings;
        private System.Windows.Forms.Button btnAddActionMapping;
        private System.Windows.Forms.Button btnRemoveInputMapping;
        private System.Windows.Forms.Button btnEditInputMapping;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doSampleDataSnapshotToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnUpEvent;
        private System.Windows.Forms.Button btnDownEvent;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnUpInput;
        private System.Windows.Forms.Button btnDownInput;
    }
}
