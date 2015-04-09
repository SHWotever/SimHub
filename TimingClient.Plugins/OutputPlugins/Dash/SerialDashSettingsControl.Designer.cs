namespace TimingClient.Plugins.OutputPlugins.Dash
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDetectedModules = new System.Windows.Forms.Label();
            this.cbReverseModule4 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule3 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule2 = new System.Windows.Forms.CheckBox();
            this.cbReverseModule1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewScreen = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.detectTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 212F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 820);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDetectedModules);
            this.groupBox1.Controls.Add(this.cbReverseModule4);
            this.groupBox1.Controls.Add(this.cbReverseModule3);
            this.groupBox1.Controls.Add(this.cbReverseModule2);
            this.groupBox1.Controls.Add(this.cbReverseModule1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modules settings";
            // 
            // lblDetectedModules
            // 
            this.lblDetectedModules.AutoSize = true;
            this.lblDetectedModules.Location = new System.Drawing.Point(6, 116);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNewScreen);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Location = new System.Drawing.Point(153, 215);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 251);
            this.panel1.TabIndex = 2;
            // 
            // btnNewScreen
            // 
            this.btnNewScreen.Location = new System.Drawing.Point(335, 73);
            this.btnNewScreen.Name = "btnNewScreen";
            this.btnNewScreen.Size = new System.Drawing.Size(75, 23);
            this.btnNewScreen.TabIndex = 2;
            this.btnNewScreen.Text = "New";
            this.btnNewScreen.UseVisualStyleBackColor = true;
            this.btnNewScreen.Click += new System.EventHandler(this.btnNewScreen_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(65, 22);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(181, 147);
            this.listBox1.TabIndex = 1;
            // 
            // detectTimer
            // 
            this.detectTimer.Enabled = true;
            this.detectTimer.Interval = 2000;
            this.detectTimer.Tick += new System.EventHandler(this.detectTimer_Tick);
            // 
            // SerialDashSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SerialDashSettingsControl";
            this.Size = new System.Drawing.Size(940, 820);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnNewScreen;
    }
}
