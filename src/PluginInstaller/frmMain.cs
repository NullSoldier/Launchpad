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
using InstallerCore;
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
		private string manifestRoot;
		private string flashDeveloperRoot;

		private void frmMain_Load(object sender, EventArgs e)
		{
			Icon = Icon.FromHandle (Resources.icon.GetHicon ());
			manifestRoot = Path.Combine (Environment.CurrentDirectory, "files");
			flashDeveloperRoot = @"C:\Program Files (x86)\FlashDevelop"; //TODO: autodetect this

			var installList = new InstallFileList ();
			installList.Load (manifestRoot);

			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
			LogMessage ("Preparing to install: " + installList.Count + " files.");

			foreach (InstallerFile file in installList.Files)
				LogMessage (string.Format ("* {0} ({1})", file.File.Name, file.Version));
		}

		private void LogMessage (string message)
		{
			if (this.InvokeRequired)
				base.Invoke ((Action)delegate { LogMessage (message); });
			else
				inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
		}

		private void StartInstalling()
		{
			Installer installer = new Installer ();
			installer.FileInstalled += (s, e) => LogMessage ("File installed: " + e.FileInstalled.Name);
			installer.FinishedInstalling += (s, e) =>
			{
				base.Invoke ((Action)finishedInstalling);
			};

			installer.Start (manifestRoot, flashDeveloperRoot);
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			btnInstall.Enabled = false;

			StartInstalling();
		}

		private void finishedInstalling()
		{
			LogMessage ("Files finished installing.");
			btnFinish.Enabled = true;
		}
	}
}
