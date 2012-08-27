using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace InstallerCore
{
	public class UpdateDownloader
	{
		/// <param name="remoteUpdateDir">The directory on the web where the "update" file will be
		/// Ex: http://entitygames.net/games/updates </param>
		/// <param name="updateCacheDir">The local updatecache where to download the updatepackage
		/// Ex: FlashDevelop/Data/Spaceport/updatecache </param>
		public UpdateDownloader (string remoteUpdateDir, string updateCacheDir)
		{
			this.remoteUpdateDir = remoteUpdateDir;
			this.updateCacheDir = updateCacheDir;
		}
		
		public event EventHandler Started;
		public event EventHandler Finished;
		public event EventHandler<UnhandledExceptionEventArgs> Failed;
		public event EventHandler<DownloadProgressChangedEventArgs> ProgressChanged;

		public void Download (Version version)
		{
			string versionFile = version + ".zip";
			string remotePatchURL = Path.Combine (remoteUpdateDir, versionFile);
			string localDestination = Path.Combine (updateCacheDir, versionFile);

			downloadUpdate (remotePatchURL, localDestination);
		}
		
		public bool TryGetWaitingPatchOnDisk (out Version versionWaiting)
		{
			versionWaiting = null;

			if (!Directory.Exists (updateCacheDir))
				return false;

			string[] waitingZips = Directory.GetFiles (updateCacheDir, "*.zip", SearchOption.TopDirectoryOnly);
			Version latestVersionFound = null;

			foreach (string waitingPatch in waitingZips)
			{
				string versionStr = Path.GetFileNameWithoutExtension (waitingPatch);
				Version version = new Version (versionStr);

				if (latestVersionFound == null || version > latestVersionFound)
					latestVersionFound = version;
			}

			logger.Info ("Waiting patch on disk found with version v" + latestVersionFound);
			versionWaiting = latestVersionFound;
			return latestVersionFound != null;
		}

		private readonly string remoteUpdateDir;
		private readonly string updateCacheDir;
		private readonly ILog logger = LogManager.GetLogger (typeof(UpdateDownloader));

		private void downloadUpdate (string remotePatchURL, string localDestination)
		{
			using (var downloadClient = new WebClient ())
			{
				try
				{
					FileHelper.EnsureFileDirExists (localDestination);
					logger.DebugFormat ("Downloading file from {0} to {1}", remotePatchURL, localDestination);
					
					downloadClient.DownloadFileCompleted += onFileDownloaded;
					downloadClient.DownloadProgressChanged += onProgressChanged;
					downloadClient.DownloadFileAsync (new Uri (remotePatchURL), localDestination);

					onDownloadStarted();
				}
				catch (Exception ex)
				{
					//TODO: reroute to Download failed
					logger.Fatal ("Downloading update failed", ex);
					throw ex;
				}
			}
		}

		#region Event Handlers
		private void onFileDownloaded (object sender, AsyncCompletedEventArgs ev)
		{
			var handler = Finished;
			if (handler != null)
				handler (this, ev);
		}

		private void onDownloadStarted()
		{
			var handler = Started;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onProgressChanged (object sender, DownloadProgressChangedEventArgs ev)
		{
			var handler = ProgressChanged;
			if (handler != null)
				handler (this, ev);
		}
		#endregion
	}
}
