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

			listBox1.Items.Clear();
			listBox2.Items.Clear();
			foreach (var track in System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\tracks")).OrderBy(i => i))
			{
				if (System.IO.Directory.GetDirectories(System.IO.Path.Combine(track, "ui")).Count() > 0)
				{
					foreach (var config in System.IO.Directory.GetDirectories(System.IO.Path.Combine(track, "ui")).OrderBy(i => i))
					{
						listBox1.Items.Add(System.IO.Path.GetFileName(track) + "|" + System.IO.Path.GetFileName(config));
					}
				}
				else
				{
					listBox1.Items.Add(System.IO.Path.GetFileName(track));
				}
			}

			foreach (var car in System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\cars")).OrderBy(i => i))
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

			if (data["SERVER"].ContainsKey("CONFIG_TRACK"))
			{
				data["SERVER"].RemoveKey("CONFIG_TRACK");
			}
			if (listBox1.Text.Contains("|"))
			{
				data["SERVER"].AddKey("CONFIG_TRACK", listBox1.Text.Split('|')[1]);
			}
			data["SERVER"]["TRACK"] = listBox1.Text.Split('|')[0];
			data["PRACTICE"]["TIME"] = numericUpDown1.Value.ToString();
			data["QUALIFY"]["TIME"] = numericUpDown2.Value.ToString();
			data["RACE"]["LAPS"] = numericUpDown3.Value.ToString();

			parser.WriteFile(cfgFile, data);

			IniData entryData = new IniData();
			entryData.Configuration.AssigmentSpacer = "";

			string dummyCar = listBox2.Items[0].ToString();

			for (int i = 2; i < 4; i++)
			{
				entryData.Sections.AddSection("CAR_" + i);
				entryData["CAR_" + i].AddKey("DRIVERNAME", "");
				entryData["CAR_" + i].AddKey("TEAM", "");
				entryData["CAR_" + i].AddKey("MODEL", dummyCar);
				entryData["CAR_" + i].AddKey("SKIN", System.IO.Path.GetFileName(
					System.IO.Directory.GetDirectories(System.IO.Path.Combine(this.textBox1.Text, "content\\cars", dummyCar, "skins")).FirstOrDefault()));
				entryData["CAR_" + i].AddKey("GUID", "");
				entryData["CAR_" + i].AddKey("SPECTATOR_MODE", "0");
			}

			for (int i = 2; i < 4; i++)
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