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
		public event EventHandler InstallFailed;
		public event EventHandler RollingBackFinished;

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
			transaction.RolledBack += onRollingBackFinished;

			foreach (InstallerFile installFile in installList.Files)
			{
				// Take the original path and chop off the install root,
				// then append it to the flash develop folder
				string relativeInstallPath = installFile.File.FullName.Substring (filesDirectory.Length + 1);
				string destinationPath = Path.Combine (flashDevelopRoot, relativeInstallPath);

				var fileCopyAction = new RevertableFileCopy (installFile.File.FullName,
					destinationPath, ensureDirectoryExists:true);
				fileCopyAction.FileCopied += (o, ev) => onFileInstalled (ev.FullPath);

				transaction.Do (fileCopyAction);
			}

			try {
				transaction.Commit();
			}
			catch (Exception ex) {
				OnInstallFailed (ex);
				transaction.Rollback();
				// Tell the user
				// Start rollback
			}

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

		public void OnInstallFailed (Exception ex)
		{
			var handler = InstallFailed;
			if (handler != null)
				handler (this, new UnhandledExceptionEventArgs (ex, false));
		}

		public void onRollingBackFinished (object sender, EventArgs e)
		{
			var handler = RollingBackFinished;
			if (handler != null)
				handler (this, e);
		}
		#endregion
	}
}
