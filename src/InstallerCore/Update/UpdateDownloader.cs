using System;
using System.Collections.Generic;
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

		public void Download (Version version)
		{
			string versionFile = version + ".zip";
			Uri patchUri = new Uri (Path.Combine (updateLocation.AbsolutePath, versionFile));
			Uri localDestination = new Uri (localUpdateLocation, version + ".zip");

			Exception error;
			if (!UpdateHelper.TryDownloadFile (patchUri, localDestination, out error))
				throw error;
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
	}
}
