using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using UpdaterCore.Update;
using log4net;

namespace UpdaterCore
{
	public class UpdateDownloader
	{
		/// <param name="updateCacheDir">The data relative local dir updatecache where to download the updatepackage
		/// Ex: FlashDevelop/Data/[Launchpad/updatecache] </param>
		public UpdateDownloader (string updateCacheDir)
		{
			this.updateCacheDir = updateCacheDir;
		}
		
		public event EventHandler Started;
		public event EventHandler Finished;
		public event EventHandler<UnhandledExceptionEventArgs> Failed;
		public event EventHandler<DownloadProgressChangedEventArgs> ProgressChanged;

		public void Download (UpdateInformation updateInfo)
		{
			var localDest = Path.Combine (updateCacheDir,
				Path.GetFileName (updateInfo.PatchZipURI.LocalPath));
			DownloadUpdatePack (updateInfo.PatchZipURI, localDest);
		}

		private readonly string updateCacheDir;
		private readonly ILog logger = LogManager.GetLogger (typeof (UpdateDownloader));

		private void DownloadUpdatePack (Uri remotePatchUri, string localDest)
		{
			using (var downloadClient = new WebClient ()) {
				try {
					FileHelper.EnsureFileDirExists (localDest);
					logger.DebugFormat ("Downloading file from {0} to {1}",
						remotePatchUri, localDest);
					
					downloadClient.DownloadFileCompleted += onFileDownloaded;
					downloadClient.DownloadProgressChanged += onProgressChanged;
					downloadClient.DownloadFileAsync (remotePatchUri, localDest);
					onDownloadStarted();
				}
				catch (Exception ex) {
					logger.Fatal ("Downloading update failed", ex);
					onDownloadFailed (ex);
				}
			}
		}

		#region Event Handlers
		private void onFileDownloaded (object sender, AsyncCompletedEventArgs ev)
		{
			Trace.WriteLine ("Finished");
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
			Trace.WriteLine ("Percentage: " + ev.ProgressPercentage);

			var handler = ProgressChanged;
			if (handler != null)
				handler (this, ev);
		}

		private void onDownloadFailed (Exception ex)
		{
			var handler = Failed;
			if (handler != null)
				handler (this, new UnhandledExceptionEventArgs (ex, false));
		}
		#endregion
	}
}
