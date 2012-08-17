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
			progressForm = new frmPerformAction();

			CreateHandle();
			loadForm();
		}

		private string flashDevelopRoot;
		private string updateCacheDir;
		private string updateZipPath;
		private string filesDirectory;
		private frmPerformAction progressForm;

		private void loadForm ()
		{
			if (Program.WaitForFlashDevelopClose)
			{

				MessageBox.Show ("Starting wait runner");
				flashDevelopRoot = Program.FlashDevelopRoot;
				updateCacheDir = Path.Combine (flashDevelopRoot, @"Data\Spaceport\updatecache\");
				updateZipPath = Path.Combine (updateCacheDir, Program.VersionToInstall + ".zip");
				filesDirectory = Path.Combine (updateCacheDir, "files");

				var waitRunner = new Thread ((o) => StartWaitRunner());
				waitRunner.Name = "Waiting for Flash Developer Close Thread";
				waitRunner.Start();
				return;
			}

			flashDevelopRoot = @"C:\Program Files\FlashDevelop"; //TODO: autodetect this
			updateCacheDir = Environment.CurrentDirectory;
			updateZipPath = Path.Combine (updateCacheDir, Directory.GetFiles (this.updateCacheDir, "*.zip").First());
			filesDirectory = Path.Combine (updateCacheDir, "files");

			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
			Show();
			ListInstallFiles();
		}

		private void ListInstallFiles()
		{
			var installList = new InstallFileList (filesDirectory);

			LogMessage ("Preparing to install: " + installList.Count + " files.");

			foreach (InstallerFile installerFile in installList.Files)
			{
				var file = installerFile.File;
				var filesDirIndex = file.FullName.IndexOf ("files") + 5;
				var filesRelativePath = file.FullName.Substring (filesDirIndex);

				LogMessage (string.Format ("* {0} ({1})", filesRelativePath, installerFile.Version));
			}
		}

		private void RunInstaller()
		{
			MessageBox.Show ("Running installer");
			progressForm.SetInstruction ("Extracting files files from " + Program.VersionToInstall + ".zip");
			progressForm.Show(this);
			
			UpdateExtractor extractor = new UpdateExtractor (updateCacheDir);
			extractor.ProgressChanged += (s, e) =>
			{
				if (e.TotalBytesToTransfer != 0)
					progressForm.SetProgress ((e.BytesTransferred / e.TotalBytesToTransfer) * 100);
			};
			extractor.Finished += (s, e) => 
			{
				progressForm.SetInstruction ("Installing files from " + filesDirectory + " to " + flashDevelopRoot);

				Installer installer = new Installer();
				installer.FileInstalled += (o, ev) => LogMessage ("File installed: " + ev.FileInstalled.FullName);
				installer.FinishedInstalling += (o, ev) => Invoke ((Action)installer_FinishedInstalling);
				installer.Start (updateCacheDir, flashDevelopRoot);
			};
			extractor.Unzip (Program.VersionToInstall);
		}

		private void installer_FinishedInstalling()
		{
			progressForm.SetProgress (100);
			progressForm.SetInstruction ("Finished installing.");
			LogMessage ("Files finished installing.");
			btnFinish.Enabled = true;
			
			MessageBox.Show ("Update finished installing, starting FlashDevelop.");
			Process.Start (Program.FlashDevelopAssembly);
			Application.Exit ();
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
			while (Process.GetProcessesByName ("FlashDevelop.exe").Length > 0)		
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
