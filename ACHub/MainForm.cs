using System;
using System.Collections;
using System.Windows.Forms;
using ACHub.Plugins;
using ACSharedMemory;

namespace ACHub
{
    public partial class MainForm : Form
    {
        private ACManager ACManager;
        private PluginManager PluginManager;

        private const string FORM_TITLE = "ACHub";

        public MainForm()
        {
            Hashtable props = new Hashtable();
            props["port"] = 8080;
            props["name"] = "ACManager";

            //Set up for remoting events properly
            //BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            //serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            //var serverChannel = new TcpServerChannel(props, serverProv);
            //ChannelServices.RegisterChannel(serverChannel, false);

            InitializeComponent();

            this.Text = string.Format("{0} - {1}", FORM_TITLE, "Game disconnected");

            ACManager = new ACSharedMemory.ACManager();
            ACManager.SynchronizingObject = this;

            //ACManagerRemoteMarshal = RemotingServices.Marshal(ACManager, "ACManager");
            //managerRemote = (ACManager)Activator.GetObject(typeof(ACManager), "tcp://cortex:8080/ACManager");
            //PluginManager = new ACHub.Plugins.PluginManager(managerRemote);

            PluginManager = new ACHub.Plugins.PluginManager(ACManager);
            PluginManager.GameStateChanged += PluginManager_GameStateChanged;
            this.pluginManagerUI1.Init(PluginManager);

            ACManager.Start();

            this.Resize += MainForm_Resize;

            var contextMenu = new ContextMenu();
            var mi = new MenuItem("Exit");
            mi.Click += notifyMenuExit_Click;
            contextMenu.MenuItems.Add(mi);

            mi = new MenuItem("Show");
            mi.Click += notifyShow_Click;
            contextMenu.MenuItems.Add(mi);

            notifyIcon.ContextMenu = contextMenu;
        }

        void notifyShow_Click(object sender, EventArgs e)
        {
            allowVisible = true;

            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;

            this.Show();
        }

        void notifyMenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PluginManager_GameStateChanged(bool running, PluginManager manager)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Text = string.Format("{0} - {1}", FORM_TITLE, running ? "Game connected" : "Game disconnected");
            });
        }

        private bool allowVisible;     // ContextMenu's Show command used
        //private bool allowClose;       // ContextMenu's Exit command used

        protected override void SetVisibleCore(bool value)
        {
            //if (!allowVisible)
            //{
            //    value = false;
            //    if (!this.IsHandleCreated) CreateHandle();
            //}
            base.SetVisibleCore(value);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                //notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void ImportStatusForm_Resize(object sender, EventArgs e)
        {
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            allowVisible = true;

            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;

            this.Show();
        }
    }
}