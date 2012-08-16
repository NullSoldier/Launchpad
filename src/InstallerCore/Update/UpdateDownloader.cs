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
		public UpdateDownloader (Uri updateLocation, Uri localUpdateLocation)
		{
			this.updateLocation = updateLocation;
			this.localUpdateLocation = localUpdateLocation;
		}
		
		public event EventHandler Started;
		public event EventHandler Finished;
		public event EventHandler<UnhandledExceptionEventArgs> Failed;
		public event EventHandler<DownloadProgressChangedEventArgs> ProgressChanged;

		public void Download (Version version)
		{
			string versionFile = version + ".zip";
			Uri patchUri = new Uri (Path.Combine (updateLocation.AbsoluteUri, versionFile));
			Uri localDestination = new Uri (localUpdateLocation, version + ".zip");

			downloadUpdate (patchUri, localDestination);
		}
		
		public bool TryGetWaitingPatchOnDisk (out Version versionWaiting)
		{
			DirectoryInfo dir = new DirectoryInfo (localUpdateLocation.AbsolutePath);
			if (!dir.Exists)
			{
				versionWaiting = null;
				return false;
			}

			FileInfo[] waitingZips = dir.GetFiles ("*.zip", SearchOption.TopDirectoryOnly);
			Version latestVersionFound = null;

			foreach (FileInfo waitingPatch in waitingZips)
			{
				Version version = new Version(waitingPatch.Name);

				if (latestVersionFound == null || version > latestVersionFound)
					latestVersionFound = version;
			}

			versionWaiting = latestVersionFound;
			return latestVersionFound != null;
		}

		private readonly Uri updateLocation;
		private readonly Uri localUpdateLocation;

		private void downloadUpdate (Uri remoteUpdateLocation, Uri destination)
		{
			using (var downloadClient = new WebClient ())
			{
				try
				{
					downloadClient.DownloadFileCompleted += onFileDownloaded;
					downloadClient.DownloadProgressChanged += onProgressChanged;
					downloadClient.DownloadFileAsync (remoteUpdateLocation, destination.AbsolutePath);
				}
				catch (Exception ex)
				{
					//TODO: reroute to Download failed
					throw ex;
				}
			}
		}

		private void onFileDownloaded (object sender, AsyncCompletedEventArgs ev)
		{
			var handler = Finished;
			if (handler != null)
				handler (this, ev);
		}

		private void onProgressChanged (object sender, DownloadProgressChangedEventArgs ev)
		{
			var handler = ProgressChanged;
			if (handler != null)
				handler (this, ev);
		}
	}
}
