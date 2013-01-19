using System;
using System.Drawing;
using System.Windows.Forms;
using InstallerCore.Update;
using PluginCore;

namespace SpaceportUpdaterPlugin
{
	public partial class frmPatch : Form
	{
		public frmPatch (UpdaterHook updater)
		{
			this.updater = updater;
			InitializeComponent ();

			if (updater.FoundUpdate == null)
			{
				updater.UpdateRunner.UpdateFound += (s, e) => loadUpdateInformation (e.UpdateInfo);
				SetWaitingMode();
			}
			else
				loadUpdateInformation (updater.FoundUpdate);
			
			updater.UpdateDownloader.Started += (s, e) => SetDownloadingMode ();
			updater.UpdateDownloader.ProgressChanged += (s, e) => SetDownloadProgress (e.ProgressPercentage);
			updater.UpdateDownloader.Finished += (s, e) => OnDownloadFinished();
		}

		private UpdaterHook updater;
		private int fullHeight;
		private string preparingFormat = "Preparing to install version v{0}";

		private void frmPatch_Load(object sender, EventArgs e)
		{
			fullHeight = Size.Height;
			MinimumSize = new Size (MinimumSize.Width, fullHeight - inNotes.Height);

			HidePatchNotes();
		}

		private void loadUpdateInformation (UpdateInformation waitingUpdate)
		{
			this.inNotes.Text = waitingUpdate.PatchNotes.Replace ("\n", Environment.NewLine);
			SetWaitingInstallMode (waitingUpdate);
		}

		private void SetWaitingMode()
		{
			lblInstruction.Text = "Waiting for update information...";
			btnInstall.Enabled = false;
			progressBar.Style = ProgressBarStyle.Marquee;
		}
		
		private void SetWaitingInstallMode (UpdateInformation waitingUpdate)
		{
			lblInstruction.Text = string.Format (preparingFormat, waitingUpdate.Version);
			lnkNotes.Visible = true;
			btnInstall.Enabled = true;
			progressBar.Style = ProgressBarStyle.Continuous;
		}

		private void SetDownloadingMode ()
		{
			btnInstall.Enabled = false;
			lblInstruction.Text = "Downloading update from entitygames.net...";
			progressBar.Style = ProgressBarStyle.Continuous;
		}

		private void SetUnzippingMode()
		{
			btnInstall.Enabled = false;
			lblInstruction.Text = "Download finished. Waiting on restart.";
			progressBar.Style = ProgressBarStyle.Continuous;
			progressBar.Value = progressBar.Maximum;
		}

		private void SetDownloadProgress (float percentage)
		{
			progressBar.Value = (int)(((progressBar.Maximum - progressBar.Minimum) * (percentage / 100))
				+ progressBar.Minimum);
		}

		private void HidePatchNotes()
		{
			this.inNotes.Visible = false;
			this.Size = new Size (Size.Width, fullHeight - inNotes.Size.Height);
			this.MaximumSize = this.Size;
			this.SizeGripStyle = SizeGripStyle.Hide;

			this.lnkNotes.Text = "View patch notes...";
		}

		private void ShowPatchNotes()
		{
			inNotes.Visible = true;
			MaximumSize = Size.Empty;
			Size = new Size (Size.Width, fullHeight);
			SizeGripStyle = SizeGripStyle.Show;

			this.lnkNotes.Text = "Hide patch notes...";
		}

		private void OnDownloadFinished()
		{
			SetUnzippingMode();

			//TODO: do verification that it downloaded successfully here
			var dialogResult = MessageBox.Show ("Restart now to finish installing?", "Restart", MessageBoxButtons.YesNo,
				MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

			if (dialogResult != DialogResult.Yes)
			{
				updater.InstallOnClose = true;
				Close();
				return;
			}
			
			Close();
			updater.RestartForUpdate();
		}

		private void lnkNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (inNotes.Visible)
				HidePatchNotes();
			else
				ShowPatchNotes();
		}

		private void frmPatch_Resize(object sender, EventArgs e)
		{
			if (inNotes.Visible)
				fullHeight = Size.Height;
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			btnInstall.Enabled = false;

			if (!updater.DownloadUpdate (updater.FoundUpdate.Version))
				OnDownloadFinished();
		}
	}
}
