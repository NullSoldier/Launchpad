using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UpdaterCore;
using Updater.Properties;
using log4net;

namespace Updater
{
	public partial class frmMain : Form
	{
		public frmMain (Version versionToInstall, string flashDevelopAssemblyPath, string updateCacheDir)
		{
			InitializeComponent();

			this.Icon = Icon.FromHandle (Resources.icon.GetHicon ());
			this.inLicense.Rtf = Resources.LICENSE;
			this.inAssemblyPath.Text = flashDevelopAssemblyPath;
			this.progressForm = new frmPerformAction();

			this.flashDevelopAssemblyPath = flashDevelopAssemblyPath;
			this.updateCacheDir = updateCacheDir;
			this.versionToInstall = versionToInstall;

			CalculateMetaDirectories();
			CreateHandle();
			LogMessage ("Spaceport installer " + Assembly.GetExecutingAssembly().GetName().Version + " loaded.");
		}

		public void RunGUIMode()
		{
			inLicense.Visible = false;
			btnAgree.Visible = false;
			inConsole.Visible = true;
			LogMessage ("Waiting for FlashDevelop to close...");

			AssemblyCloseDelayer.WaitForAssembliesAsync (() =>
				Invoke ((Action)(() => {
					inLicense.Visible = true;
					btnAgree.Visible = true;
					inConsole.Visible = false;
					LogMessage ("Flash develop closed, starting setup.");
				}))
			, flashDevelopAssemblyPath);

			Show();
		}

		public void RunInstaller()
		{
			if (!VerifyLocalUpdateExists()) {
				return;
			}

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
				logger.DebugFormat ("Finished extracting {0} files to {1}", e.EntriesTotal, filesDir);
				progressForm.SetInstruction ("Installing files from " + filesDir + " to " + flashDevelopDir);
				Installer installer = new Installer ();
				
				installer.FileInstalled += (o, ev) => {
					LogMessage ("File installed: " + ev.FileInstalled.FullName);
				};
				installer.FinishedInstalling += (o, ev) => {
					Invoke (new Action (FinishedInstalling));
				};
				installer.InstallFailed += (o, ev) => {
					invokeMessageBox ("Install failed, rolling back...");
				};
				installer.RollingBackFinished += (o, ev) => {
					invokeMessageBox ("Finished rolling back, exiting.");
					Application.Exit ();
				};

				installer.Start (updateCacheDir,
					new FileInfo (flashDevelopAssemblyPath));
			};
			extractor.Unzip (versionToInstall);

			progressForm.SetInstruction ("Extracting files files from " + versionToInstall + ".zip");
			progressForm.ShowDialog (this);
		}

		private string flashDevelopAssemblyPath;
		private string flashDevelopDir;
		private string updateCacheDir;
		private string filesDir;
		private string updateZipPath;
		private Version versionToInstall;
		private frmPerformAction progressForm;
		private readonly ILog logger = LogManager.GetLogger (typeof (InstallerEntry));
		
		private void CalculateMetaDirectories()
		{
			this.flashDevelopDir = new FileInfo (flashDevelopAssemblyPath).DirectoryName;
			this.filesDir = Path.Combine (updateCacheDir, "files");
			this.updateZipPath = Path.Combine (updateCacheDir, versionToInstall + ".zip");
		}

		private void FinishedInstalling()
		{
			progressForm.SetProgress (100);
			progressForm.SetInstruction ("Finished installing.");
			LogMessage ("Files finished installing.");
			btnFinish.Enabled = true;

			LogMessage ("Finished installing files, launching FlashDevelop at path " + flashDevelopAssemblyPath);
			MessageBox.Show ("Update finished installing, starting FlashDevelop.");
			Process.Start (flashDevelopAssemblyPath);
			Application.Exit();
		}

		private void LogMessage (string message)
		{
			if (InvokeRequired)
				base.Invoke (new MethodInvoker (delegate { LogMessage (message); }));
			else
			{
				logger.Debug (message);
				inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
			}
		}

		private bool VerifyLocalUpdateExists()
		{
			if (File.Exists (updateZipPath))
				return true;

			logger.InfoFormat ("Update package did not download properly, expected at {0}{0}{1}", Environment.NewLine, updateZipPath);
			MessageBox.Show ("Update package did not download properly, missing at" + Environment.NewLine + Environment.NewLine +
				updateZipPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			Close();
			Application.Exit ();
			return false;
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			btnInstall.Enabled = false;
			btnAgree.Enabled = false;
			btnBrowse.Enabled = false;
			inLicense.Visible = false;
			btnAgree.Visible = false;
			inConsole.Visible = true;
			inAssemblyPath.Enabled = false;

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

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = "Flash Develop (FlashDevelop.exe)";

			var result = dialog.ShowDialog (this);
			if (result == DialogResult.OK) {
				inAssemblyPath.Text = dialog.FileName;
				flashDevelopAssemblyPath = dialog.FileName;
				CalculateMetaDirectories();
			}
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void invokeMessageBox (string text)
		{
			Invoke (new Action (() =>
				MessageBox.Show (this, text)
			));
		}
	}
}
