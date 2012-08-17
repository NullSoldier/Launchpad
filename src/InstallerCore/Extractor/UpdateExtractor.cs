using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace InstallerCore
{
	public class UpdateExtractor
	{
		/// <summary>
		/// Takes a URI that points to a directory with an update zip and extracts to directory/files
		/// Ex: FlashDevelop/Data/updatecache/0.0.2.0.zip
		/// </summary>
		public UpdateExtractor (string updateCacheDir)
		{
			this.updateCacheDir = updateCacheDir;
		}

		public event EventHandler Finished;
		public event EventHandler<UnhandledExceptionEventArgs> Failed;
		public event EventHandler<ExtractProgressEventArgs> ProgressChanged;

		public void Unzip (Version version)
		{
			string zipFilePath = Path.Combine (this.updateCacheDir, version + ".zip");
			string unzipDirPath = Path.Combine (this.updateCacheDir, "files/");

			try
			{
				// Make sure we have a valid, fresh directory to extract to
				if (Directory.Exists (unzipDirPath))
					Directory.Delete (unzipDirPath, true);
			
				Directory.CreateDirectory (unzipDirPath);
			}
			catch (Exception ex) { onFailed (ex); }

			if (ZipFile.IsZipFile (zipFilePath))
			{
				ZipFile zip = ZipFile.Read (zipFilePath);
				zip.ExtractProgress += onProgressChanged;
				zip.ZipError += (o, e) => onFailed (e.Exception);
				zip.ExtractAll (unzipDirPath);
			}
			else
				onFailed (new ZipException ("File is not a zip: " + zipFilePath));
		}

		private readonly string updateCacheDir;
		private bool isFinished;

		private void onFinished ()
		{
			var handler = Finished;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onProgressChanged (object sender, ExtractProgressEventArgs ev)
		{
			if (ev.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
				onFinished();

			var handler = ProgressChanged;
			if (handler != null)
				handler (sender, ev);
		}

		private void onFailed (Exception ex)
		{
			var handler = Failed;
			if (handler != null)
				handler (this, new UnhandledExceptionEventArgs (ex, false)); 
		}
	}
}
