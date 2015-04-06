using ACSharedMemory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimingClient.Plugins;

namespace ACHub
{
    public partial class MainForm : Form
    {
        private ACManager ACManager;
        private PluginManager PluginManager;

        private const string FORM_TITLE = "ACHub";

        public MainForm()
        {

            InitializeComponent();

            this.Text = string.Format("{0} - {1}", FORM_TITLE, "Disconected");

            ACManager = new ACSharedMemory.ACManager();
            PluginManager = new TimingClient.Plugins.PluginManager(ACManager);

            ACManager.SynchronizingObject = this;
            ACManager.GameStateChanged += ACManager_GameStateChanged;
            ACManager.Start();
        }

        private void ACManager_GameStateChanged(bool running, ACSharedMemory.ACManager manager)
        {

            this.Text = string.Format("{0} - {1}", FORM_TITLE, running ? "Connected" : "Disconected");

        }


    }
}
