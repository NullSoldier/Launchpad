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
		public UpdateExtractor (Uri localUpdateLocation)
		{
			this.localUpdateLocation = localUpdateLocation;
		}

		public event EventHandler Finished;
		public event EventHandler<UnhandledExceptionEventArgs> Failed;
		public event EventHandler<ExtractProgressEventArgs> ProgressChanged;

		public void Unzip (Version version)
		{
			string localPath = localUpdateLocation.AbsolutePath;
			string zipPath = Path.Combine (localPath, version + ".zip");
			string unzipPath = Path.Combine(localPath, "files/");

			try
			{
				if (Directory.Exists (unzipPath))
					Directory.Delete (unzipPath, true);
			
				// Make sure we have a valid, fresh directory to extract to
				Directory.CreateDirectory (unzipPath);
			}
			catch (Exception ex) { onFailed (ex); }

			if (!ZipFile.IsZipFile (zipPath))
			{
				onFailed (new ZipException ("File is not a zip: " + zipPath));
				return;
			}

			ZipFile zip = ZipFile.Read (zipPath);
			zip.ExtractProgress += onProgressChanged;
			zip.ZipError += (o, e) => onFailed (e.Exception);
			zip.ExtractAll (unzipPath);

			onFinished();
		}

		private readonly Uri localUpdateLocation;

		private void onFinished ()
		{
			var handler = Finished;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onProgressChanged (object sender, ExtractProgressEventArgs ev)
		{
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
