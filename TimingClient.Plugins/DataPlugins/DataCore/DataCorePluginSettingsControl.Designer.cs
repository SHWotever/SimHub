namespace TimingClient.Plugins.DataPlugins.DataCore
{
    partial class DataCorePluginSettingsControl
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
            this.btnAddExpression = new System.Windows.Forms.Button();
            this.btnRemoveExpression = new System.Windows.Forms.Button();
            this.btnEditExpression = new System.Windows.Forms.Button();
            this.lstExpressions = new ACToolsUtilities.UI.RefreshingListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddExpression
            // 
            this.btnAddExpression.Location = new System.Drawing.Point(3, 3);
            this.btnAddExpression.Name = "btnAddExpression";
            this.btnAddExpression.Size = new System.Drawing.Size(76, 23);
            this.btnAddExpression.TabIndex = 4;
            this.btnAddExpression.Text = "Add";
            this.btnAddExpression.UseVisualStyleBackColor = true;
            this.btnAddExpression.Click += new System.EventHandler(this.btnAddExpression_Click);
            // 
            // btnRemoveExpression
            // 
            this.btnRemoveExpression.Location = new System.Drawing.Point(166, 3);
            this.btnRemoveExpression.Name = "btnRemoveExpression";
            this.btnRemoveExpression.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveExpression.TabIndex = 5;
            this.btnRemoveExpression.Text = "Remove";
            this.btnRemoveExpression.UseVisualStyleBackColor = true;
            this.btnRemoveExpression.Click += new System.EventHandler(this.btnRemoveExpression_Click);
            // 
            // btnEditExpression
            // 
            this.btnEditExpression.Location = new System.Drawing.Point(85, 3);
            this.btnEditExpression.Name = "btnEditExpression";
            this.btnEditExpression.Size = new System.Drawing.Size(75, 23);
            this.btnEditExpression.TabIndex = 6;
            this.btnEditExpression.Text = "Edit";
            this.btnEditExpression.UseVisualStyleBackColor = true;
            this.btnEditExpression.Click += new System.EventHandler(this.btnEditExpression_Click);
            // 
            // lstExpressions
            // 
            this.lstExpressions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExpressions.FormattingEnabled = true;
            this.lstExpressions.Location = new System.Drawing.Point(3, 3);
            this.lstExpressions.Name = "lstExpressions";
            this.lstExpressions.Size = new System.Drawing.Size(764, 463);
            this.lstExpressions.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 523);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom data (c# scripts)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstExpressions, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(770, 504);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnAddExpression);
            this.flowLayoutPanel1.Controls.Add(this.btnEditExpression);
            this.flowLayoutPanel1.Controls.Add(this.btnRemoveExpression);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 472);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(764, 29);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // DataCorePluginSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DataCorePluginSettingsControl";
            this.Size = new System.Drawing.Size(776, 523);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ACToolsUtilities.UI.RefreshingListBox lstExpressions;
        private System.Windows.Forms.Button btnAddExpression;
        private System.Windows.Forms.Button btnRemoveExpression;
        private System.Windows.Forms.Button btnEditExpression;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
