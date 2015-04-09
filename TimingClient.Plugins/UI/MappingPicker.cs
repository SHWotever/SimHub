using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimingClient.Plugins.UI
{
    public partial class MappingPicker : Form
    {
        public MappingPicker()
        {
            InitializeComponent();
        }
        public Mapping ShowDialog(PluginManager manager, List<string> trigger, List<string> actions, Mapping currentMapping = null)
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox1.Items.AddRange(trigger.OrderBy(i => i).ToArray());
            this.listBox2.Items.AddRange(actions.OrderBy(i => i).ToArray());
            if (currentMapping != null)
            {
                try
                {
                    this.listBox1.SelectedItem = currentMapping.Trigger;
                }
                catch { }
                try
                {
                    this.listBox2.SelectedItem = currentMapping.Target;
                }
                catch { }
                this.button1.Text = "OK";
            }

            manager.InputTriggered += manager_InputTriggered;

            if (this.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (listBox1.SelectedItem != null && listBox2.SelectedItem != null)
                {
                    return new Mapping() { Trigger = listBox1.SelectedItem.ToString(), Target = listBox2.SelectedItem.ToString() };
                }
            }

            manager.InputTriggered -= manager_InputTriggered;

            return null;
        }

        bool manager_InputTriggered(string input)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    this.listBox1.SelectedItem = input;
                    //Debug.WriteLine(input);
                }
                catch { }
            });

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
