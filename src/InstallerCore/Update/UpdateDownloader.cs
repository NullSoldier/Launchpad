using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

namespace InstallerCore
{
	public class UpdateDownloader
	{
		/// <summary>
		/// 
		/// </summary>
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
				Version version = new Version(versionStr);

				if (latestVersionFound == null || version > latestVersionFound)
					latestVersionFound = version;
			}

			versionWaiting = latestVersionFound;
			return latestVersionFound != null;
		}

		private readonly string remoteUpdateDir;
		private readonly string updateCacheDir;

		private void downloadUpdate (string remotePatchURL, string localDestination)
		{
			using (var downloadClient = new WebClient ())
			{
				try
				{
					string destDir = new FileInfo (localDestination).Directory.FullName;
					if (!Directory.Exists (destDir))
						Directory.CreateDirectory (destDir);

					downloadClient.DownloadFileCompleted += onFileDownloaded;
					downloadClient.DownloadProgressChanged += onProgressChanged;
					downloadClient.DownloadFileAsync (new Uri (remotePatchURL), localDestination);

					onFileStarted();
				}
				catch (Exception ex)
				{
					//TODO: reroute to Download failed
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

		private void onFileStarted ()
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
