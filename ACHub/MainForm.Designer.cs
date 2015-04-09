namespace ACHub
{
    partial class MainForm
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.pluginManagerUI1 = new TimingClient.Plugins.UI.PluginManagerUI();
            this.SuspendLayout();
            // 
            // pluginManagerUI1
            // 
            this.pluginManagerUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginManagerUI1.Location = new System.Drawing.Point(0, 0);
            this.pluginManagerUI1.Name = "pluginManagerUI1";
            this.pluginManagerUI1.Size = new System.Drawing.Size(754, 640);
            this.pluginManagerUI1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 640);
            this.Controls.Add(this.pluginManagerUI1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TimingClient.Plugins.UI.PluginManagerUI pluginManagerUI1;
    }
}

