using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ACHub.Plugins.UI
{
    /// <summary>
    /// Mapping picker form
    /// </summary>
    public partial class MappingPicker : Form
    {
        /// <summary>
        /// CTor
        /// </summary>
        public MappingPicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show ui
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="trigger"></param>
        /// <param name="actions"></param>
        /// <param name="currentMapping"></param>
        /// <returns></returns>
        public Mapping ShowDialog(PluginManager manager, List<string> trigger, List<string> actions, Mapping currentMapping = null)
        {
            this.flpPRessType.Visible = false;
            Logging.Current.Info("Opening mapping picker");
            Logging.Current.Info("TRIGGERS");
            foreach (var item in trigger)
            {
                Logging.Current.Info(item);
            }

            Logging.Current.Info("ACTIONS");
            foreach (var item in actions)
            {
                Logging.Current.Info(item);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

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

        public InputMapping ShowInputDialog(PluginManager manager, List<string> trigger, List<string> actions, InputMapping currentMapping = null)
        {
            this.flpPRessType.Visible = true;
            Logging.Current.Info("Opening mapping picker");
            Logging.Current.Info("TRIGGERS");
            foreach (var item in trigger)
            {
                Logging.Current.Info(item);
            }

            Logging.Current.Info("ACTIONS");
            foreach (var item in actions)
            {
                Logging.Current.Info(item);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
            this.listBox1.Items.AddRange(trigger.OrderBy(i => i).ToArray());
            this.listBox2.Items.AddRange(actions.OrderBy(i => i).ToArray());
            if (currentMapping != null)
            {
                this.rbShortPress.Checked = currentMapping.PressType == PressType.ShortPress;
                this.rbLongPress.Checked = currentMapping.PressType == PressType.LongPress;
                this.rbWhile.Checked = currentMapping.PressType == PressType.During;

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
                    PressType pressType = PressType.ShortPress;

                    if (this.rbShortPress.Checked)
                        pressType = PressType.ShortPress;
                    if (this.rbLongPress.Checked)
                        pressType = PressType.LongPress;
                    if (this.rbWhile.Checked)
                        pressType = PressType.During;

                    return new InputMapping() { Trigger = listBox1.SelectedItem.ToString(), Target = listBox2.SelectedItem.ToString(), PressType = pressType };
                }
            }

            manager.InputTriggered -= manager_InputTriggered;

            return null;
        }

        private bool manager_InputTriggered(string input)
        {
            if (this.Visible)
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.listBox1.SelectedItem = input;
                    });
                }
                catch { }
            }
            return this.Visible;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}