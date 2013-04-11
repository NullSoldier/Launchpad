using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UpdaterCore.Update;

namespace UpdaterCore
{
	public class UpdateRunner
	{
		/// <param name="updateLocation">The location to a .update file. Ex: http://domain.com/updates/update </param>
		/// <param name="currentVersion">Look for updates later than this version</param>
		public UpdateRunner (Uri updateLocation, Version currentVersion)
		{
			this.updateLocation = updateLocation;
			this.currentVersion = currentVersion;
		}

		public event EventHandler CheckUpdateStarted;
		public event EventHandler CheckUpdateStopped;
		public event EventHandler<UpdateCheckerEventArgs> UpdateFound;
		public event EventHandler<UpdateCheckerEventArgs> UpdateNotFound;
		public event EventHandler<UpdateCheckerEventArgs> CheckUpdateFailed;

		public void Start()
		{
			isCheckingUpdates = true;
			updateThread = new Thread (runUpdateChecker);
			updateThread.IsBackground = false;
			updateThread.Name = "Check for updates thread";
			updateThread.Start();

			onStartCheckUpdate();
		}

		public void Stop()
		{
			if (!isCheckingUpdates)
				return;

			updateThread.Abort();
			isCheckingUpdates = false;
			onStopCheckUpdate();
		}

		private readonly Uri updateLocation;
		private Version currentVersion;
		private Thread updateThread;
		private bool isCheckingUpdates;
		private const int checkSleepTime = 60000;

		private void runUpdateChecker()
		{
			while (isCheckingUpdates)
			{
				Version versionFound;
				string patchNotes;

				if (TryFetchUpdate (out versionFound, out patchNotes))
					onUpdateFound (versionFound, patchNotes);
				else
					onUpdateNotFound();

				Thread.Sleep (checkSleepTime);
			}
		}

		public bool TryCheckOnceForUpdate()
		{
			if (isCheckingUpdates)
				throw new InvalidOperationException ("Already checking for updates");

			Version versionFound;
			string patchNotes;

			if (TryFetchUpdate (out versionFound, out patchNotes)) {
				onUpdateFound (versionFound, patchNotes);
				return true;
			} else {
				onUpdateNotFound();
				return false;
			}
		}

		private bool TryFetchUpdate (out Version versionFound, out string patchNotes)
		{
			Exception ex;
			patchNotes = null;

			if (!TryDownloadVersion (out versionFound, out ex)) {
				onCheckUpdateFailed (ex);
				return false;
			}

			if (versionFound <= currentVersion)
				return false;

			Uri patchNotesURI = GetPatchNotesUri (updateLocation, versionFound);
			if (!UpdateHelper.TryDownloadString (patchNotesURI, out patchNotes, out ex))
				patchNotes = "No patch notes available.";

			return true;
		}

		private bool TryDownloadVersion (out Version versionFound, out Exception ex)
		{
			versionFound = null;
			string version;

			if (!UpdateHelper.TryDownloadString (updateLocation, out version, out ex))
				return false;

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
			if (handler != null) {
				var zipURI = new Uri (updateLocation, version + ".zip");
				var updateInfo = new UpdateInformation (version, patchNotes, zipURI);

				handler (this, new UpdateCheckerEventArgs (updateInfo));
			}
		}

		private void onUpdateNotFound()
		{
			var handler = UpdateNotFound;
			if (handler != null) {
				handler (this, new UpdateCheckerEventArgs (updateLocation));
			}
		}

		private void onCheckUpdateFailed (Exception ex)
		{
			var handler = CheckUpdateFailed;
			if (handler != null)
				handler (this, new UpdateCheckerEventArgs (updateLocation, ex));
		}

		#endregion

		private static Uri GetUpdatePackageUri (Uri updateLocation, Version versionFound)
		{
			return new Uri (updateLocation, versionFound + ".zip");
		}

		private static Uri GetPatchNotesUri (Uri updateLocation, Version versionFound)
		{
			return new Uri (updateLocation, versionFound + ".patchnotes");
		}
	}
}
