namespace TimingClient.Plugins.UI
{
    partial class MappingPicker
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Trigger = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.flpPRessType = new System.Windows.Forms.FlowLayoutPanel();
            this.rbShortPress = new System.Windows.Forms.RadioButton();
            this.rbLongPress = new System.Windows.Forms.RadioButton();
            this.rbWhile = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.flpPRessType.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(497, 480);
            this.listBox1.TabIndex = 0;
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(506, 16);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(498, 480);
            this.listBox2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Trigger, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.flpPRessType, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1007, 548);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Trigger
            // 
            this.Trigger.AutoSize = true;
            this.Trigger.Location = new System.Drawing.Point(3, 0);
            this.Trigger.Name = "Trigger";
            this.Trigger.Size = new System.Drawing.Size(40, 13);
            this.Trigger.TabIndex = 2;
            this.Trigger.Text = "Trigger";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(506, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Action";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(929, 522);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // flpPRessType
            // 
            this.flpPRessType.Controls.Add(this.rbShortPress);
            this.flpPRessType.Controls.Add(this.rbLongPress);
            this.flpPRessType.Controls.Add(this.rbWhile);
            this.flpPRessType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPRessType.Location = new System.Drawing.Point(0, 499);
            this.flpPRessType.Margin = new System.Windows.Forms.Padding(0);
            this.flpPRessType.Name = "flpPRessType";
            this.flpPRessType.Size = new System.Drawing.Size(503, 20);
            this.flpPRessType.TabIndex = 5;
            this.flpPRessType.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // rbShortPress
            // 
            this.rbShortPress.AutoSize = true;
            this.rbShortPress.Checked = true;
            this.rbShortPress.Location = new System.Drawing.Point(3, 3);
            this.rbShortPress.Name = "rbShortPress";
            this.rbShortPress.Size = new System.Drawing.Size(78, 17);
            this.rbShortPress.TabIndex = 0;
            this.rbShortPress.TabStop = true;
            this.rbShortPress.Text = "Short press";
            this.rbShortPress.UseVisualStyleBackColor = true;
            // 
            // rbLongPress
            // 
            this.rbLongPress.AutoSize = true;
            this.rbLongPress.Location = new System.Drawing.Point(87, 3);
            this.rbLongPress.Name = "rbLongPress";
            this.rbLongPress.Size = new System.Drawing.Size(77, 17);
            this.rbLongPress.TabIndex = 1;
            this.rbLongPress.Text = "Long press";
            this.rbLongPress.UseVisualStyleBackColor = true;
            // 
            // rbWhile
            // 
            this.rbWhile.AutoSize = true;
            this.rbWhile.Location = new System.Drawing.Point(170, 3);
            this.rbWhile.Name = "rbWhile";
            this.rbWhile.Size = new System.Drawing.Size(108, 17);
            this.rbWhile.TabIndex = 2;
            this.rbWhile.Text = "Do while pressing";
            this.rbWhile.UseVisualStyleBackColor = true;
            // 
            // MappingPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 548);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MappingPicker";
            this.Text = "MappingPicker";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flpPRessType.ResumeLayout(false);
            this.flpPRessType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label Trigger;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flpPRessType;
        private System.Windows.Forms.RadioButton rbShortPress;
        private System.Windows.Forms.RadioButton rbLongPress;
        private System.Windows.Forms.RadioButton rbWhile;
    }
}