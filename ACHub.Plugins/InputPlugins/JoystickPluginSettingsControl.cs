﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ACHub.Plugins.InputPlugins
{
    /// <summary>
    /// Joystick plugin settings control
    /// </summary>
    public partial class JoystickPluginSettingsControl : UserControl
    {
        /// <summary>
        /// CTor
        /// </summary>
        public JoystickPluginSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Refesh inputs
        /// </summary>
        /// <param name="inputs"></param>
        public void Refresh(List<string> inputs)
        {
            if (this.cbShowStatus.Checked)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (inputs.Count > 0)
                        this.txtSettings.Text += DateTime.Now.ToString() + " : " + string.Join("\r\n" + DateTime.Now.ToString() + " : ", inputs) + "\r\n";
                });
            }
        }

        public void Refresh(string input)
        {
            if (this.cbShowStatus.Checked)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.txtSettings.Text += DateTime.Now.ToString() + " : " + DateTime.Now.ToString() + input + "\r\n";
                });
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}