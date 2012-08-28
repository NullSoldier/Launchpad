using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using InstallerCore.Rollback;
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
			installThread = new Thread (() => installFiles (updateCacheDirectory, flashDevelopRoot));
			installThread.Name = "Install Files Thread";
			installThread.Start();
		}

		private Thread installThread;

		private void installFiles (string updateCacheDirectory, string flashDevelopRoot)
		{
			string filesDirectory = Path.Combine (updateCacheDirectory, "files");
			var installList = new InstallFileList (filesDirectory);

			var transaction = new RevertableTransaction();

			foreach (InstallerFile installFile in installList.Files)
			{
				// Take the original path and chop off the install root,
				// then append it to the flash develop folder
				string relativeInstallPath = installFile.File.FullName.Substring (filesDirectory.Length + 1);
				string destPath = Path.Combine (flashDevelopRoot, relativeInstallPath);
			
				var destDirectory = new FileInfo (destPath).Directory;
				if (!destDirectory.Exists)
					destDirectory.Create();

				var fileCopyAction = new RevertableFileCopy (installFile.File.FullName, destPath);
				fileCopyAction.FileCopied += (o, ev) => onFileInstalled (ev.FullPath);
				
				transaction.Do (fileCopyAction);
			}

			transaction.ActionFailed += (s, ev) => transaction.Rollback();
			transaction.Commit();

			onFinished ();
		}

		#region Event Handlers
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

		private void onFailedRollingBack ()
		{
			
		}
		#endregion
	}
}
