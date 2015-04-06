namespace MapGenerator
{
    partial class TestForm
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
            this.verticalTabControl1 = new MapGenerator.VerticalTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.verticalTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // verticalTabControl1
            // 
            this.verticalTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.verticalTabControl1.Controls.Add(this.tabPage1);
            this.verticalTabControl1.Controls.Add(this.tabPage2);
            this.verticalTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.verticalTabControl1.Location = new System.Drawing.Point(36, 12);
            this.verticalTabControl1.Multiline = true;
            this.verticalTabControl1.Name = "verticalTabControl1";
            this.verticalTabControl1.SelectedIndex = 0;
            this.verticalTabControl1.Size = new System.Drawing.Size(202, 169);
            this.verticalTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(23, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(175, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(23, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(175, 161);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 643);
            this.Controls.Add(this.verticalTabControl1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.verticalTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VerticalTabControl verticalTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}