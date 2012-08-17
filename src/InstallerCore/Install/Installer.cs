using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using PluginInstaller;

namespace InstallerCore
{
	public class Installer
	{
		public event EventHandler FinishedInstalling;
		public event EventHandler<InstallerEventArgs> FileInstalled;

		/// <summary>
		/// Takes the update zip directory and assumes it was extracted to zipdirectory/files
		/// Ex: FlashDevelop/Data/updatecache/
		/// And also takes the FlashDevelop directory where FlashDevelop.exe is
		/// Ex: FlashDevelop/
		/// </summary>
		public void Start (string updateCacheDirectory, string flashDevelopRoot)
		{
			installThread = new Thread (installThreadRunner);
			installThread.Name = "Install Files Thread";
			installThread.Start(new [] {updateCacheDirectory, flashDevelopRoot});
		}

		private Thread installThread;

		private void installThreadRunner(object arg)
		{
			string[] args = (string[])arg;
			startInstallProcess (args[0], args[1]);
		}

		private void startInstallProcess (string updateCacheDirectory, string flashDevelopRoot)
		{
			string filesDirectory = Path.Combine (updateCacheDirectory, "files");
			var installList = new InstallFileList (filesDirectory);

			foreach (InstallerFile installFile in installList.Files)
			{
				// Take the original path and chop off the install root,
				// then append it to the flash develop folder
				string relativeInstallPath = installFile.File.FullName.Substring (updateCacheDirectory.Length + 1);
				string destPath = Path.Combine (flashDevelopRoot, relativeInstallPath);
			
				var destDirectory = new FileInfo (destPath).Directory;
				if (!destDirectory.Exists)
					destDirectory.Create();

				installFile.File.CopyTo (destPath, true);
				onFileInstalled (destPath);
			}

			onFinished ();
		}

		private void onFinished()
		{
			var handler = FinishedInstalling;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onFileInstalled(string path)
		{
			var installedInfo = new FileInfo (path);

			var handler = FileInstalled;
			if (handler != null)
				handler (this, new InstallerEventArgs (installedInfo));
		}
	}
}
