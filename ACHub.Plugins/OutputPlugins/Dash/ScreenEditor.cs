using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Screen editor UI
    /// </summary>
    public partial class ScreenEditor : Form
    {
        private BindingList<ScreenPart> parts = new BindingList<ScreenPart>();

        /// <summary>
        /// Show dialog
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public Screen ShowDialog(Screen screen, PluginManager manager)
        {
            this.tabControl1.TabPages.Clear();
            if (screen != null)
            {
                cbIdleScreen.Checked = screen.GameNotRunningScreen;
                cbInGameScreen.Checked = screen.GameRunningScreen;
                numBlinkTime.Value = screen.BlinkFrequency;
            }
            for (int i = 0; i < 4; i++)
            {
                var tab = new TabPage();
                tab.Text = "Module " + (i + 1).ToString();
                var control = new ScreenEditorControl(manager);
                control.Dock = DockStyle.Fill;

                if (screen != null && screen.ScrenParts != null && screen.ScrenParts.Count > i)
                {
                    control.Parts = new BindingList<ScreenPart>(screen.ScrenParts[i].Select(j => j.Clone() as ScreenPart).ToList());
                    control.Announce = screen.ScrenParts[i].AnnounceText;
                }
                if (screen != null && screen.ScrenAnnounce != null && screen.ScrenParts.Count > i)
                {
                    (flpAnnounce.Controls[i] as ScreenAnnounceEditor).Value = screen.ScrenAnnounce[i].AnnounceText;
                }

                tab.Controls.Add(control);
                tab.Controls[0].Dock = DockStyle.Fill;
                this.tabControl1.TabPages.Add(tab);
            }
            this.txtName.Text = screen.ScreenName;

            this.ShowDialog();

            if (this.DialogResult == DialogResult.OK)
            {
                //var t = new ScreenItem((tabControl1.TabPages[3].Controls[0] as ScreenEditorControl).Parts.ToList());
                return new Screen()
                {
                    GameNotRunningScreen = cbIdleScreen.Checked,
                    GameRunningScreen = cbInGameScreen.Checked,
                    ScreenName = this.txtName.Text,
                    BlinkFrequency = (int)numBlinkTime.Value,
                    ScrenParts = new List<ScreenItem>(
                        new ScreenItem[] {
                        new ScreenItem (   (tabControl1.TabPages[0].Controls[0] as ScreenEditorControl).Parts.ToList())
                        { AnnounceText = (tabControl1.TabPages[0].Controls[0] as ScreenEditorControl).Announce},
                            new ScreenItem (   (tabControl1.TabPages[1].Controls[0] as ScreenEditorControl).Parts.ToList())
                            { AnnounceText = (tabControl1.TabPages[1].Controls[0] as ScreenEditorControl).Announce},
                            new ScreenItem (   (tabControl1.TabPages[2].Controls[0] as ScreenEditorControl).Parts.ToList())
                            { AnnounceText = (tabControl1.TabPages[2].Controls[0] as ScreenEditorControl).Announce},
                            new ScreenItem (   (tabControl1.TabPages[3].Controls[0] as ScreenEditorControl).Parts.ToList())
                            { AnnounceText = (tabControl1.TabPages[3].Controls[0] as ScreenEditorControl).Announce}
                    }),
                    ScrenAnnounce = new List<ScreenAnnouncePart>(new ScreenAnnouncePart[]{
                    new ScreenAnnouncePart{ AnnounceText= (flpAnnounce.Controls[0] as ScreenAnnounceEditor).Value},
                    new ScreenAnnouncePart{ AnnounceText= (flpAnnounce.Controls[1] as ScreenAnnounceEditor).Value},
                    new ScreenAnnouncePart{ AnnounceText= (flpAnnounce.Controls[2] as ScreenAnnounceEditor).Value},
                    new ScreenAnnouncePart{ AnnounceText= (flpAnnounce.Controls[3] as ScreenAnnounceEditor).Value}
                    })
                };
            }
            return null;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public ScreenEditor()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtName.Text = new String(txtName.Text.Where(i => (i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z') || (i >= '0' && i <= '9') || i == '.').ToArray());
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            var i = e.KeyChar;
            if (i == '\b') { return; }
            if ((i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z') || (i >= '0' && i <= '9') || i == '.')
            {
            }
            else { e.Handled = true; }
        }
    }
}