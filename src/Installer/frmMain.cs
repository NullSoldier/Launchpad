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
using InstallerCore;
using PluginInstaller.Properties;

namespace PluginInstaller
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

		public void RunInstaller()
		{
			if (!VerifyLocalUpdateExists ())
			{
				Application.Exit();
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
				progressForm.SetInstruction ("Installing files from " + filesDir + " to " + flashDevelopDir);

				Installer installer = new Installer ();
				installer.FileInstalled += (o, ev) => LogMessage ("File installed: " + ev.FileInstalled.FullName);
				installer.FinishedInstalling += (o, ev) => Invoke (new MethodInvoker (FinishedInstalling));
				installer.Start (updateCacheDir, flashDevelopDir);
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
			
			MessageBox.Show ("Update finished installing, starting FlashDevelop.");
			Process.Start (flashDevelopAssemblyPath);
			Application.Exit();
		}

		private void LogMessage (string message)
		{
			if (InvokeRequired)
				base.Invoke (new MethodInvoker (delegate { LogMessage (message); }));
			else
				inConsole.Text += string.Format ("{0}{1}", message, Environment.NewLine);
		}

		private bool VerifyLocalUpdateExists()
		{
			if (File.Exists (updateZipPath))
				return true;

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
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog (this) == DialogResult.OK)
			{
				inAssemblyPath.Text = dialog.FileName;
				flashDevelopAssemblyPath = dialog.FileName;
				CalculateMetaDirectories();
			}
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}
