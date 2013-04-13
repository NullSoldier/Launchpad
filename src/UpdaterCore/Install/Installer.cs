﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Updater;
using UpdaterCore.Rollback;
using log4net;
using log4net.Core;

namespace UpdaterCore
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
		public void Start(
			DirectoryInfo updateCacheDirectory,
			FileInfo flashDevelopAssembly)
		{
			installThread = new Thread (() => {
				try {
					installFiles (
						updateCacheDirectory,
						flashDevelopAssembly);
				}
				catch (Exception ex) {
					logger.Error ("Installer failed " + ex.Message, ex);
					OnInstallFailed (ex);
				}
			});
			installThread.Name = "Install Files Thread";
			installThread.Start();
		}

		private Thread installThread;
		private ILog logger = LogManager.GetLogger (typeof (Installer));

		private void installFiles (
			DirectoryInfo updateCacheDir,
			FileInfo flashDevelopAssembly)
		{
			var dataDir = FDHelper.GetDataDir (flashDevelopAssembly);
			var flashDevelopDir = flashDevelopAssembly.DirectoryName;
			var updateDir = updateCacheDir
				.AppendDir ("files")
				.AppendDir ("update");

			var installList = new InstallFileList (updateDir);
			var transaction = new RevertableTransaction();
			transaction.RolledBack += onRollingBackFinished;

			foreach (InstallerFile installFile in installList.Files)
			{
				// Take the original path and chop off the install root,
				// then append it to the flash develop folder
				string relativeInstallPath = installFile.File.FullName
					.Substring (updateDir.FullName.Length + 1);
				
				string dest = hardcodeResolvePath (
					relativeInstallPath,
					flashDevelopDir,
					dataDir.FullName);

				var fileCopyAction = new RevertableFileCopy (
					installFile.File.FullName,
					dest,
					ensureDirectoryExists:true);

				fileCopyAction.FileCopied += (o, ev) => onFileInstalled (ev.Value);
				transaction.Do (fileCopyAction);
			}
			try {
				transaction.Commit();
			}
			catch (RevertableActionFailedException ex) {
				logger.Error ("Installer failed on action " + ex.FailedAction, ex.Exception);
				logger.DebugFormat ("Starting rollback of {0} items." + transaction.CompletedCount);
				OnInstallFailed (ex);
				transaction.Rollback();
				return;
			}
			onFinished();
		}

		private string hardcodeResolvePath (
			string relativePath,
			string flashDevelopDir,
			string dataDir)
		{
			if (relativePath.StartsWith ("Data")) {
				var parent = Path.Combine (dataDir, "..");
				return Path.Combine (parent, relativePath);
			}
			return Path.Combine (flashDevelopDir, relativePath);
		}

		#region Event Handlers
			private
			void onFinished()
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
