using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UpdaterCore.Update;

namespace UpdaterCore
{
	public class UpdateChecker
	{
		///<param name="remoteManifestUri">The location to a .update file. EX: http://domain.com/updates/update </param>
		///<param name="currentVersion">Look for updates later than this version</param>
		public UpdateChecker (Uri remoteManifestUri, Version currentVersion)
		{
			this.remoteManifestUri = remoteManifestUri;
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
			updateThread = new Thread (RunUpdateChecker);
			updateThread.Name = "Check for updates thread";
			updateThread.Start();
			onStartCheckUpdate();
		}

		public void Stop()
		{
			if (isCheckingUpdates) {
				updateThread.Abort();
				isCheckingUpdates = false;
				onStopCheckUpdate();
			}
		}

		public void CheckForUpdateInfo()
		{
			UpdateInformation updateInfo;
			Exception ex;

			if (!TryGetUpdateManifest (out updateInfo, out ex)) {
				onCheckUpdateFailed (ex);
				return;
			}
			if (currentVersion >= updateInfo.Manifest.ProductVersion) {
				onUpdateNotFound();
				return;
			}
			onUpdateFound (updateInfo);
		}

		private Version currentVersion;
		private Thread updateThread;
		private bool isCheckingUpdates;
		private readonly Uri remoteManifestUri;
		private const int CHECK_DELAY = 60000;

		private void RunUpdateChecker()
		{
			while (isCheckingUpdates) {
				CheckForUpdateInfo();
				Thread.Sleep (CHECK_DELAY);
			}
		}

		//<returns>False if exception, otherwise true</returns>
		private bool TryGetUpdateManifest (out UpdateInformation updateInfo, out Exception ex)
		{
			string result;
			if (!UpdateHelper.TryDownloadString (remoteManifestUri, out result, out ex)) {
				updateInfo = null;
				return false;
			}

			UpdateManifest manifest;
			try {
				manifest = UpdateManifest.ParseManifest (result);
			} catch (Exception exception) {
				ex = exception;
				updateInfo = null;
				return false;
			}

			// Get the patch notes
			string patchNotes;
			var patchNotesUri = remoteManifestUri.Append (manifest.ProductVersion + ".patchnotes");
			if (!UpdateHelper.TryDownloadString (patchNotesUri, out patchNotes, out ex))
				patchNotes = "No patch notes available.";

			var patchZipUri = remoteManifestUri.Append (manifest.ProductVersion + ".zip");
			var updateUri = remoteManifestUri.Append ("updater." + manifest.UpdaterVersion + ".exe");
			updateInfo = new UpdateInformation (manifest, patchZipUri, updateUri, patchNotes);
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

		private void onUpdateFound (UpdateInformation updateInfo)
		{
			var handler = UpdateFound;
			if (handler != null) {
				handler (this, new UpdateCheckerEventArgs (updateInfo));
			}
		}

		private void onUpdateNotFound()
		{
			var handler = UpdateNotFound;
			if (handler != null) {
				handler (this, new UpdateCheckerEventArgs (remoteManifestUri));
			}
		}

		private void onCheckUpdateFailed (Exception ex)
		{
			var handler = CheckUpdateFailed;
			if (handler != null)
				handler (this, new UpdateCheckerEventArgs (remoteManifestUri, ex));
		}

		#endregion
	}
}
