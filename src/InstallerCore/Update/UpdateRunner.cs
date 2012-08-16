using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using InstallerCore.Update;

namespace InstallerCore
{
	public class UpdateRunner
	{
		public UpdateRunner (Uri updateLocation, Version currentVersion)
		{
			this.updateLocation = updateLocation;
			this.currentVersion = currentVersion;
		}

		public event EventHandler CheckUpdateStarted;
		public event EventHandler CheckUpdateStopped;
		public event EventHandler<UpdateCheckerEventArgs> UpdateFound;
		public event EventHandler<UpdateCheckerEventArgs> CheckUpdateFailed;

		public void Start()
		{
			isCheckingUpdates = true;

			updateThread = new Thread (updateCheckRunner);
			updateThread.Start();

			onStartCheckUpdate();
		}

		public void Stop()
		{
			if (!isCheckingUpdates)
				return;
			
			isCheckingUpdates = false;
			onStopCheckUpdate();
		}

		public bool TryCheckOnceForUpdate (out UpdateInformation updateInfo)
		{
			updateInfo = null;

			Exception ex;
			Version versionFound;
			string patchNotes;

			if (!TryDownloadVersion (out versionFound, out ex))
			{
				onCheckUpdateFailed (ex);
				return false;
			}

			// No need to go further, this is not a new version
			if (versionFound <= currentVersion)
				return false;

			// Grab patch notes too
			Uri patchNotesURI = new Uri (updateLocation, versionFound + ".patchnotes");
			if (!UpdateHelper.TryDownloadString (patchNotesURI, out patchNotes, out ex))
				patchNotes = "No patch notes available.";

			updateInfo = new UpdateInformation (versionFound, patchNotes,
				new Uri(updateLocation, versionFound + ".zip"));

			return true;
		}

		private const int checkSleepTime = 60000;
		private readonly Uri updateLocation;
		private readonly Version currentVersion;
		private Thread updateThread;
		private bool isCheckingUpdates;

		private void updateCheckRunner()
		{
			while (isCheckingUpdates)
			{
				Version versionFound;
				string patchNotes;
				Exception ex;

				if (!TryDownloadVersion (out versionFound, out ex))
				{
					onCheckUpdateFailed (ex);
					Thread.Sleep (checkSleepTime);
					continue;
				}

				if (versionFound <= currentVersion)
				{
					Thread.Sleep (checkSleepTime);
					continue;
				}

				Uri patchNotesURI = new Uri (updateLocation, versionFound + ".patchnotes");
				if (!UpdateHelper.TryDownloadString (patchNotesURI, out patchNotes, out ex))
					patchNotes = "No patch notes available.";

				Stop();
				onUpdateFound (versionFound, patchNotes);
				return;
			}
		}

		private bool TryDownloadVersion (out Version versionFound, out Exception ex)
		{
			string version;

			if (!UpdateHelper.TryDownloadString (this.updateLocation, out version, out ex))
			{
				versionFound = null;
				return false;
			}

			//TODO: reduce point of failure at version string formatting
			versionFound = new Version (version);
			return true;
		}

		#region Event Handlers
		private void onStartCheckUpdate()
		{
			var handler = CheckUpdateStarted;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}
	
		private void onStopCheckUpdate()
		{
			var handler = CheckUpdateStopped;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onUpdateFound (Version version, string patchNotes)
		{
			var handler = UpdateFound;
			if (handler != null)
			{
				var zipURI = new Uri (updateLocation, version + ".zip");
				var updateInfo = new UpdateInformation (version, patchNotes, zipURI);

				handler (this, new UpdateCheckerEventArgs (updateInfo));
			}
		}

		private void onCheckUpdateFailed (Exception ex)
		{
			var handler = CheckUpdateFailed;
			if (handler != null)
				handler (this, new UpdateCheckerEventArgs (updateLocation, ex));
		}
		#endregion
	}
}
