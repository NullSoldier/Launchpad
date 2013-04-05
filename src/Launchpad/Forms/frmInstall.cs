using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Launchpad.Helpers;
using UpdaterCore;

namespace Launchpad.Forms
{
	public partial class frmInstall : Form
	{
		public frmInstall (Action<DevicePlatform> started, Action stopped)
		{
			Check.ArgNull (started, "started");
			Check.ArgNull (stopped, "stopped");
			this.started = started;
			this.stopped = stopped;
			InitializeComponent ();
		}

		private void frmInstall_Load (object s, EventArgs ev)
		{
			setState (InstallerMode.Waiting);
			btnInstalliOS.Focus();
		}

		public void LogMessage (string msg)
		{
			if (msg == null)
				return;

			Invoke (new Action (() =>
				inLog.AppendText (msg + Environment.NewLine)
			));
		}

		public void LogFatal (string msg)
		{
			Invoke (new Action (delegate {
				var orig = inLog.SelectionColor;
				inLog.SelectionColor = Color.Red;
				inLog.AppendText (msg + Environment.NewLine);
				inLog.SelectionColor = orig;
				MessageBox.Show (this, msg, "Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}));
		}

		public void FinishInstall()
		{
			Invoke (new Action (() =>
				setState (InstallerMode.Waiting)
			));
		}

		private readonly Action<DevicePlatform> started;
		private readonly Action stopped;
		private InstallerMode currentMode;

		private void btnInstalliOS_Click (object s, EventArgs e)
		{
			startInstall (DevicePlatform.iOS);
		}

		private void btnInstallAndroid_Click (object s, EventArgs e)
		{
			startInstall (DevicePlatform.Android);
		}

		private void btnClose_Click (object sender, EventArgs e)
		{
			if (currentMode == InstallerMode.Installing) {
				LogMessage ("Canceling install process.");
				setState (InstallerMode.Waiting);
				stopped();
			}
			else {
				Close();
			}
		}

		private void frmInstall_FormClosing (object s, FormClosingEventArgs ev)
		{
			if (currentMode == InstallerMode.Installing)
				stopped();
		}

		private void startInstall (DevicePlatform platform)
		{
			inLog.Clear();
			LogMessage ("Starting install process connected "
				+ platform.GetString() + " devices");
			setState (InstallerMode.Installing);
			started (platform);
		}

		private void setState(InstallerMode mode)
		{
			currentMode = mode;
			switch (mode)
			{
				case InstallerMode.Installing:
					btnInstallAndroid.Enabled = false;
					btnInstalliOS.Enabled = false;
					btnClose.Text = "Cancel";
					break;
				case InstallerMode.Waiting:
					btnInstallAndroid.Enabled = true;
					btnInstalliOS.Enabled = true;
					btnClose.Text = "Close";
					break;
			}
		}

		private enum InstallerMode
		{
			Waiting,
			Installing
		}
	}
}
