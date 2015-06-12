namespace ACHub.Plugins.OutputPlugins.Dash
{
    partial class LedEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.LedNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSource = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OffColor = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OnColor = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.OnStart = new ACToolsUtilities.UI.DataGridViewExtension.DataGridViewNumericUpDownColumn();
            this.OnEnd = new ACToolsUtilities.UI.DataGridViewExtension.DataGridViewNumericUpDownColumn();
            this.Blink = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LedNumber,
            this.DataSource,
            this.OffColor,
            this.OnColor,
            this.OnStart,
            this.OnEnd,
            this.Blink});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1131, 734);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // LedNumber
            // 
            this.LedNumber.HeaderText = "Led";
            this.LedNumber.Name = "LedNumber";
            this.LedNumber.ReadOnly = true;
            // 
            // DataSource
            // 
            this.DataSource.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.DataSource.HeaderText = "Data";
            this.DataSource.Items.AddRange(new object[] {
            "Rpms",
            "FuelLaps",
            "FuelPercent",
            "AllTimeBestDelta","SessionBestDelta"});
            this.DataSource.Name = "DataSource";
            this.DataSource.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // OffColor
            // 
            this.OffColor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.OffColor.HeaderText = "Off";
            this.OffColor.Items.AddRange(new object[] {
            "Off",
            "Green",
            "Red"});
            this.OffColor.Name = "OffColor";
            // 
            // OnColor
            // 
            this.OnColor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.OnColor.HeaderText = "On";
            this.OnColor.Items.AddRange(new object[] {
            "Off",
            "Red",
            "Green"});
            this.OnColor.Name = "OnColor";
            this.OnColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OnColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // OnStart
            // 
            this.OnStart.HeaderText = "Start %";
            this.OnStart.Name = "OnStart";
            this.OnStart.Minimum = -200;
            this.OnStart.Maximum = 200;
            this.OnStart.DecimalPlaces = 1;
            // 
            // OnEnd
            // 
            this.OnEnd.HeaderText = "End %";
            this.OnEnd.Name = "OnEnd";
            this.OnEnd.Minimum = -200;
            this.OnEnd.Maximum = 200;
            this.OnEnd.DecimalPlaces = 1;
            // 
            // Blink
            // 
            this.Blink.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Blink.HeaderText = "Blink";
            this.Blink.Items.AddRange(new object[] {
            "Invert",
            "Off",
            "Green",
            "Red"});
            this.Blink.Name = "Blink";
            this.Blink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LedEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "LedEditor";
            this.Size = new System.Drawing.Size(1131, 734);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LedNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn DataSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn OffColor;
        private System.Windows.Forms.DataGridViewComboBoxColumn OnColor;
        private ACToolsUtilities.UI.DataGridViewExtension.DataGridViewNumericUpDownColumn OnStart;
        private ACToolsUtilities.UI.DataGridViewExtension.DataGridViewNumericUpDownColumn OnEnd;
        private System.Windows.Forms.DataGridViewComboBoxColumn Blink;
    }
}
