using IniParser;
using IniParser.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ServerLancher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = Properties.Settings.Default.LastPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastPath = this.textBox1.Text;
            Properties.Settings.Default.Save();
            foreach (var track in System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\tracks")))
            {
                listBox1.Items.Add(System.IO.Path.GetFileName(track));
            }

            foreach (var car in System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\cars")))
            {
                listBox2.Items.Add(System.IO.Path.GetFileName(car));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("acServer"))
            {
                process.Kill();
            }

            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";
            string cfgFile = System.IO.Path.Combine(this.textBox1.Text, "cfg\\server_cfg.ini");
            string entryFile = System.IO.Path.Combine(this.textBox1.Text, "cfg\\entry_list.ini");

            IniData data = parser.ReadFile(cfgFile);

            data["SERVER"]["CARS"] = listBox2.Text;
            data["SERVER"]["TRACK"] = listBox1.Text;
            data["PRACTICE"]["TIME"] = numericUpDown1.Value.ToString();
            data["QUALIFY"]["TIME"] = numericUpDown2.Value.ToString();
            data["RACE"]["LAPS"] = numericUpDown3.Value.ToString();

            parser.WriteFile(cfgFile, data);

            IniData entryData = new IniData();
            entryData.Configuration.AssigmentSpacer = "";
            for (int i = 0; i < 2; i++)
            {
                entryData.Sections.AddSection("CAR_" + i);
                entryData["CAR_" + i].AddKey("DRIVERNAME", "");
                entryData["CAR_" + i].AddKey("TEAM", "");
                entryData["CAR_" + i].AddKey("MODEL", listBox2.Text);
                entryData["CAR_" + i].AddKey("SKIN", System.IO.Path.GetFileName(
                    System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\cars", listBox2.Text, "skins")).FirstOrDefault()));
                entryData["CAR_" + i].AddKey("GUID", "");
                entryData["CAR_" + i].AddKey("SPECTATOR_MODE", "0");
            }

            parser.WriteFile(entryFile, entryData);

            Process.Start(new ProcessStartInfo() { FileName = System.IO.Path.Combine(this.textBox1.Text, "acServer.bat"), WorkingDirectory = this.textBox1.Text });
        }
    }
}