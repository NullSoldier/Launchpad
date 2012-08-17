using System;
using System.Collections.Generic;
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

		public void Unzip (Version version)
		{
			string localPath = localUpdateLocation.AbsolutePath;
			string zipPath = Path.Combine (localPath, version + ".zip");
			string unzipPath = Path.Combine(localPath, "files/");

			// Make sure we have a valid, fresh directory to extract to
			if (!Directory.Exists (unzipPath))
				Directory.CreateDirectory (unzipPath);
			else
				Directory.Delete (unzipPath, true);

			if (!ZipFile.IsZipFile (zipPath))
				return;

			ZipFile zip = ZipFile.Read (zipPath);
			zip.ExtractAll (unzipPath);
		}

		private readonly Uri localUpdateLocation;
	}
}
