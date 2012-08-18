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
				flashDevelopRoot = Program.FlashDevelopRoot;
				updateCacheDir = Path.Combine (flashDevelopRoot, @"Data\Spaceport\updatecache\");
				updateZipPath = Path.Combine (updateCacheDir, Program.VersionToInstall + ".zip");
				filesDirectory = Path.Combine (updateCacheDir, "files");

				var waitRunner = new Thread (o => StartWaitRunner (GetFlashDevelopProcesses()));
				waitRunner.Name = "Waiting for Flash Developer Close Thread";
				waitRunner.Start();
				return;
			}

			var firstZip = InstallerHelper.GetLatestWaitingUpdate (Environment.CurrentDirectory);
			if (firstZip == null) {
				MessageBox.Show ("Update .zip package missing in root directory, exiting");
				Application.Exit (); return;
			}

			flashDevelopRoot = @"C:\Program Files\FlashDevelop"; //TODO: autodetect this
			updateCacheDir = Environment.CurrentDirectory;
			filesDirectory = Path.Combine (updateCacheDir, "files");
			updateZipPath = Path.Combine (updateCacheDir, firstZip);

			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
			Show();
		}

		

		private void RunInstaller()
		{
			progressForm.SetInstruction ("Extracting files files from " + Program.VersionToInstall + ".zip");
			progressForm.Show(this);
			
			UpdateExtractor extractor = new UpdateExtractor (updateCacheDir);
			extractor.ProgressChanged += (s, e) =>
			{
				if (e.TotalBytesToTransfer != 0)
				{
					progressForm.SetProgress ((e.BytesTransferred / e.TotalBytesToTransfer) * 100);
					progressForm.SetInstruction ("Unzipping " + e.CurrentEntry.FileName);
				}
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
			//Process.Start (Program.FlashDevelopAssembly);
			//Application.Exit ();
		}

		private void LogMessage (string message)
		{
			if (InvokeRequired)
				base.Invoke ((Action)delegate { LogMessage (message); });
			else
				inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
		}

		private void StartWaitRunner (IEnumerable<Process> processesToWaitFor)
		{
			foreach (var process in processesToWaitFor)
				process.WaitForExit();

			Invoke (new MethodInvoker (RunInstaller));
		}

		private IEnumerable<Process> GetFlashDevelopProcesses()
		{
			var knownAssembly = new FileInfo (Program.FlashDevelopAssembly);

			foreach (Process process in Process.GetProcesses())
			{
				string processAssemblyPath;
				try{ processAssemblyPath = process.MainModule.FileName; }
				catch (Win32Exception) { continue; }
				
				//TODO: find a better way to do this than just comparing full names
				//if (processAssemblyPath.Contains ("Flash"))
					//Debugger.Launch();

				var assembly = new FileInfo (processAssemblyPath);
				if (assembly.FullName.ToLower() == knownAssembly.FullName.ToLower())
				{
					Console.WriteLine ("Process found: " + processAssemblyPath);
					yield return process;
				}
			}
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			btnInstall.Enabled = false;
			btnAgree.Enabled = false;
			RunInstaller();
		}

		private void btnFinish_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnAgree_CheckedChanged(object sender, EventArgs e)
		{
			btnInstall.Enabled = true;
		}
	}
}
