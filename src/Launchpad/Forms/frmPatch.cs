using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UpdaterCore;
using UpdaterCore.Update;

namespace Launchpad.Forms
{
	public partial class frmPatch : Form
	{
		public frmPatch (UpdaterHook updater)
		{
			this.updater = updater;
			InitializeComponent ();
		}

		private readonly UpdaterHook updater;
		private UpdateInformation waitingUpdate;
		private PatchFormState currentState;
		private int fullHeight;
		private const string PREPARING_FORMAT = "Preparing to install version v{0}";

		private void form_Loaded (object s, EventArgs e)
		{
			fullHeight = Size.Height;
			MinimumSize = new Size (MinimumSize.Width, fullHeight - inNotes.Height);
			hidePatchNotes();
		}

		private void form_Shown (object sender, EventArgs e)
		{
			// Is there an update already? show it!
			if (updater.FoundUpdate != null) {
				updateFound (updater.FoundUpdate);
			} else {
				setFormState (PatchFormState.Waiting);
				updater.UpdateRunner.UpdateFound += onUpdateFound;
				updater.UpdateRunner.UpdateNotFound += onUpdateNotFound;
				updater.DownloadUpdateInfo();
			}

			updater.UpdateDownloader.Started += onDownloadStarted;
			updater.UpdateDownloader.Finished += onDownloadFinished;
			updater.UpdateDownloader.ProgressChanged += onDownloadProgress;
		}

		private void form_Closing (object sender, FormClosingEventArgs e)
		{
			updater.UpdateRunner.UpdateFound -= onUpdateFound;
			updater.UpdateRunner.UpdateNotFound -= onUpdateNotFound;
			updater.UpdateDownloader.Started -= onDownloadStarted;
			updater.UpdateDownloader.Finished -= onDownloadFinished;
			updater.UpdateDownloader.ProgressChanged -= onDownloadProgress;
		}

		private void updateFound (UpdateInformation updateInfo)
		{
			waitingUpdate = updateInfo;
			setFormState (PatchFormState.WaitingInstall);
			showPatchNotes();
		}

		private void updateNotFound()
		{
			setFormState (PatchFormState.NoUpdate);
			MessageBox.Show ("There are no updates to download. " +
				"You have the latest version");
		}

		private void onDownloadFinished()
		{
			setFormState (PatchFormState.Unzipping);

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

		private void lnkNotes_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (inNotes.Visible) { hidePatchNotes(); }
			else { showPatchNotes(); }
		}

		private void frmPatch_Resize (object sender, EventArgs e)
		{
			if (inNotes.Visible)
				fullHeight = Size.Height;
		}

		private void btnInstall_Click (object sender, EventArgs e)
		{
			btnInstall.Enabled = false;
			if (!updater.DownloadUpdate (updater.FoundUpdate.Version))
				onDownloadFinished ();
		}

		private void setFormState(PatchFormState state)
		{
			switch (state)
			{
				case PatchFormState.Waiting:
					lblInstruction.Text = "Waiting for update information...";
					btnInstall.Enabled = false;
					progressBar.Style = ProgressBarStyle.Marquee;
					break;
				case PatchFormState.NoUpdate:
					lblInstruction.Text = "No update found.";
					lnkNotes.Visible = false;
					btnInstall.Enabled = false;
					progressBar.Style = ProgressBarStyle.Continuous;
					break;
				case PatchFormState.WaitingInstall:
					inNotes.Text = waitingUpdate.PatchNotes.Replace ("\n", Environment.NewLine);
					lblInstruction.Text = string.Format (PREPARING_FORMAT, waitingUpdate.Version);
					lnkNotes.Visible = true;
					btnInstall.Enabled = true;
					progressBar.Style = ProgressBarStyle.Continuous;
					break;
				case PatchFormState.Downloading:
					btnInstall.Enabled = false;
					lblInstruction.Text = "Downloading update from entitygames.net...";
					progressBar.Style = ProgressBarStyle.Continuous;
					break;
				case PatchFormState.Unzipping:
					btnInstall.Enabled = false;
					lblInstruction.Text = "Download finished. Waiting on restart.";
					progressBar.Style = ProgressBarStyle.Continuous;
					progressBar.Value = progressBar.Maximum;
					break;
			}
		}

		private void setDownloadProgress(float percentage)
		{
			int range = progressBar.Maximum - progressBar.Minimum;
			int value = range * (int)(percentage/100);
			progressBar.Value = value + progressBar.Minimum;
		}

		private void hidePatchNotes()
		{
			inNotes.Visible = false;
			Size = new Size (Size.Width, fullHeight - inNotes.Size.Height);
			MaximumSize = Size;
			SizeGripStyle = SizeGripStyle.Hide;
			lnkNotes.Text = "View patch notes...";
		}

		private void showPatchNotes()
		{
			inNotes.Visible = true;
			MaximumSize = Size.Empty;
			Size = new Size (Size.Width, fullHeight);
			SizeGripStyle = SizeGripStyle.Show;
			lnkNotes.Text = "Hide patch notes...";
		}

		private enum PatchFormState
		{
			Waiting,
			NoUpdate,
			WaitingInstall,
			Downloading,
			Unzipping
		}

		#region Update handlers
		private void onDownloadStarted (object s, EventArgs e)
		{
			setFormState (PatchFormState.Downloading);
		}

		private void onDownloadFinished (object s, EventArgs e)
		{
			onDownloadFinished ();
		}

		private void onDownloadProgress (object s, DownloadProgressChangedEventArgs e)
		{
			setDownloadProgress (e.ProgressPercentage);
		}

		private void onUpdateNotFound (object sender, UpdateCheckerEventArgs e)
		{
			updateNotFound ();
		}

		private void onUpdateFound (object sender, UpdateCheckerEventArgs e)
		{
			updateFound (e.UpdateInfo);
		}
		#endregion
	}
}
