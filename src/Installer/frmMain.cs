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
using System.Threading;
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

			Icon = Icon.FromHandle (Resources.icon.GetHicon ());

			// Do this before frmMain_Load to hide it properly
			if (Program.WaitForFlashDevelopClose)
				Hide();
		}

		private string manifestRoot;
		private string flashDevelopRoot;
		private string zipPath;

		private void frmMain_Load(object sender, EventArgs e)
		{
			if (Program.WaitForFlashDevelopClose)
			{
				flashDevelopRoot = Program.FlashDevelopRoot;
				manifestRoot = Path.Combine (flashDevelopRoot, @"Data\Spaceport\updatecache\");
				zipPath = Path.Combine (manifestRoot, Program.VersionToInstall + ".zip");

				var waitRunner = new Thread ((o) => StartWaitRunner());
				waitRunner.Name = "Waiting for Flash Developer Close Thread";
				waitRunner.Start();
				return;
			}

			manifestRoot = Path.Combine (Environment.CurrentDirectory, "files");
			zipPath = Path.Combine (manifestRoot, Directory.GetFiles (manifestRoot, "*.zip").First());
			flashDevelopRoot = @"C:\Program Files\FlashDevelop"; //TODO: autodetect this

			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
			Show();
			ListInstallFiles();
		}

		private void ListInstallFiles()
		{
			var installList = new InstallFileList ();
			installList.Load (manifestRoot);

			LogMessage ("Preparing to install: " + installList.Count + " files.");

			foreach (InstallerFile file in installList.Files)
			{
				var filesDirIndex = file.File.FullName.IndexOf ("files") + 5;
				var filesRelativePath = file.File.FullName.Substring (filesDirIndex);

				LogMessage (string.Format ("* {0} ({1})", filesRelativePath, file.Version));
			}
		}

		private void RunInstaller()
		{
			frmPerformAction form = new frmPerformAction();
			form.Show(this);
			form.SetInstruction ("Extracting files files from " + Program.VersionToInstall + ".zip");
			
			UpdateExtractor extractor = new UpdateExtractor (new Uri (zipPath));
			extractor.ProgressChanged += (s, e) => form.SetProgress (e.BytesTransferred / e.TotalBytesToTransfer);
			extractor.Finished += (s, e) => 
			{
				form.SetInstruction ("Installing files to " + manifestRoot);

				Installer installer = new Installer();
				installer.FileInstalled += (o, ev) => LogMessage ("File installed: " + ev.FileInstalled.FullName);
				installer.FinishedInstalling += (o, ev) => Invoke ((Action)installer_FinishedInstalling);
				installer.Start (manifestRoot, flashDevelopRoot);
			};
			extractor.Unzip (Program.VersionToInstall);

			// Display Dialogue
			// Open flashdev
		}

		private void installer_FinishedInstalling()
		{
			LogMessage ("Files finished installing.");
			btnFinish.Enabled = true;

			Process.Start (Program.FlashDevelopRoot);
		}

		private void LogMessage (string message)
		{
			if (this.InvokeRequired)
				base.Invoke ((Action)delegate { LogMessage (message); });
			else
				inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
		}

		private void StartWaitRunner ()
		{
			while (Process.GetProcessesByName ("FlashDevelop.exe").Length >= 0)		
				Thread.Sleep (500);

			Invoke (new MethodInvoker (RunInstaller));
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			btnInstall.Enabled = false;
			RunInstaller();
		}

		private void btnFinish_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
