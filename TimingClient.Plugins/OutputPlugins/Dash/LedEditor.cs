using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACHub.Plugins.OutputPlugins.Dash
{
    /// <summary>
    /// Led editor UI
    /// </summary>
    public partial class LedEditor : UserControl
    {
      private  bool loading = false;
        private SerialDashPlugin plugin;
        /// <summary>
        /// CTor
        /// </summary>
        public LedEditor()
        {
            loading = true;
            InitializeComponent();
            for (int i = 0; i < 8 * 4; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = "Screen " + ((int)(i / 8) + 1).ToString() + "  led " + (i % 8 + 1).ToString();
            }
            dataGridView1.DataError += DataGridView1_DataError;

        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Returns leds settings
        /// </summary>
        /// <returns></returns>
        public List<LedDefinition> GetLeds()
        {
            List<LedDefinition> leds = new List<LedDefinition>();
            for (int i = 0; i < 8 * 4; i++)
            {
                var ld = new LedDefinition();
                ld.DataSource = this.dataGridView1.Rows[i].Cells[1].Value as string;
                ld.OffColor = this.dataGridView1.Rows[i].Cells[2].Value as string;
                ld.OnColor = (string)this.dataGridView1.Rows[i].Cells[3].Value as string;
                ld.OnRangeStart = int.Parse(this.dataGridView1.Rows[i].Cells[4].Value.ToString());
                ld.OnRangeEnd = int.Parse(this.dataGridView1.Rows[i].Cells[5].Value.ToString());
                ld.BlinkColor = this.dataGridView1.Rows[i].Cells[6].Value as string;

                leds.Add(ld);
            }
            return leds;
        }

        /// <summary>
        /// Load leds settings
        /// </summary>
        /// <param name="leds"></param>
        /// <param name="plugin"></param>
        public void LoadLeds(List<LedDefinition> leds, SerialDashPlugin plugin)
        {
            this.plugin = plugin;
            loading = true;
            this.dataGridView1.Rows.Clear();
            for (int i = 0; i < 8 * 4; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = "Screen " + ((int)(i / 8) + 1).ToString() + "  led " + (i % 8 + 1).ToString();


                this.dataGridView1.Rows[i].Cells[4].Value = 0;
                this.dataGridView1.Rows[i].Cells[5].Value = 0;

                if (leds != null && leds.Count > i)
                {
                    if (leds[i].OffColor != null)
                        this.dataGridView1.Rows[i].Cells[2].Value = leds[i].OffColor;
                    if (leds[i].OnColor != null)
                        this.dataGridView1.Rows[i].Cells[3].Value = leds[i].OnColor;
                    if (leds[i].BlinkColor != null)
                        this.dataGridView1.Rows[i].Cells[6].Value = leds[i].BlinkColor;

                    this.dataGridView1.Rows[i].Cells[1].Value = leds[i].DataSource;

                    this.dataGridView1.Rows[i].Cells[4].Value = leds[i].OnRangeStart;

                    this.dataGridView1.Rows[i].Cells[5].Value = leds[i].OnRangeEnd;

                }
            }
            loading = false;

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loading)
            {
                plugin.Settings.LedSettings = GetLeds();
            }
        }
    }
}
