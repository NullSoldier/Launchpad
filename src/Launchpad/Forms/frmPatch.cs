using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using LaunchPad.Properties;
using UpdaterCore;
using UpdaterCore.Update;

namespace LaunchPad.Forms
{
	public partial class frmPatch : Form
	{
		public frmPatch (UpdaterController updateController)
		{
			updater = updateController;
			InitializeComponent ();

			fullHeight = Size.Height;
			MinimumSize = new Size (MinimumSize.Width, fullHeight - inNotes.Height);
			Icon = Icon.FromHandle (Resources.spaceportIcon.GetHicon ());

			Launchpad.A.UpdateWindowOpened ();
			hidePatchNotes ();
		}

		private UpdateInformation waitingUpdate;
		private int fullHeight;
		private readonly UpdaterController updater;

		private void form_Shown (object sender, EventArgs e)
		{
			updater.UpdateDownloader.Started += onDownloadStarted;
			updater.UpdateDownloader.Finished += onDownloadFinished;
			updater.UpdateDownloader.ProgressChanged += onDownloadProgress;

			setFormState (PatchFormState.Waiting);
			updater.UpdateChecker.UpdateFound += onUpdateFound;
			updater.UpdateChecker.UpdateNotFound += onUpdateNotFound;
			updater.UpdateChecker.CheckUpdateFailed += onCheckUpdateFailed;
			updater.DownloadUpdateManifest ();
		}

		private void form_Closing (object sender, FormClosingEventArgs e)
		{
			updater.UpdateChecker.UpdateFound -= onUpdateFound;
			updater.UpdateChecker.UpdateNotFound -= onUpdateNotFound;
			updater.UpdateChecker.CheckUpdateFailed -= onCheckUpdateFailed;
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
			MessageBox.Show ("There are no updates to download. You have the latest version");
		}

		private void onDownloadFinished()
		{
			setDownloadProgress (100);
			setFormState (PatchFormState.Unzipping);

			var result = MessageBox.Show (
				"Restart now to finish installing?",
				"Restart",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);

			bool closeNow = result == DialogResult.Yes;
			updater.StartUpdating (waitingUpdate.Manifest.ProductVersion, closeNow);
			Close();
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
			updater.DownloadUpdate (waitingUpdate);
		}

		private void setFormState (PatchFormState state)
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
					setPatchNotes (waitingUpdate.PatchNotes);
					lblInstruction.Text = "Preparing to install version v" + waitingUpdate.Manifest.ProductVersion;
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

		private void setDownloadProgress (float percentage)
		{
			int range = progressBar.Maximum - progressBar.Minimum;
			int value = (int)(range * (percentage/100f));
			progressBar.Value = value + progressBar.Minimum;

			// Hack to get responsive progress bars on Windows 7
			if (progressBar.Value > progressBar.Minimum
				&& progressBar.Value <= progressBar.Maximum)
			{
				progressBar.Value -= 1;
				progressBar.Value += 1;
			}
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

		private void setPatchNotes(string notes)
		{
			if (notes.StartsWith ("{\\rtf"))
				inNotes.Rtf = notes;
			else
				inNotes.Text = notes.Replace ("\n", Environment.NewLine);
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

		private void onCheckUpdateFailed (object sender, UpdateCheckerEventArgs e)
		{
			MessageBox.Show (this, "Failed to get update from "
					+ e.CheckLocation
					+ Environment.NewLine
					+ Environment.NewLine
					+ e.Exception.Message,
				"Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}

		private void onUpdateFound (object sender, UpdateCheckerEventArgs e)
		{
			updateFound (e.UpdateInfo);
		}
		#endregion
	}
}
