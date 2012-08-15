using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PluginInstaller.Properties;

namespace PluginInstaller
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		private string manifestPath;

		private void frmMain_Load(object sender, EventArgs e)
		{
			this.Icon = Icon.FromHandle (Resources.icon.GetHicon ());
			this.manifestPath = Path.Combine (Environment.CurrentDirectory, "files\\files.ini");

			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
			LogMessage ("Preparing to install: " + GetInstallCount() + " files.");
		}

		private void LogMessage (string message)
		{
			inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
		}

		private int GetInstallCount()
		{
			InstalledList list = new InstalledList();
			list.Load (manifestPath);
			return list.Count;
		}
	}
}
