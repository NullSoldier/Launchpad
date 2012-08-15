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

		public void Start (string installRoot, string flashDevelopRoot)
		{
			installThread = new Thread (installThreadRunner);
			installThread.Name = "Install Files Thread";
			installThread.Start(new [] {installRoot, flashDevelopRoot});
		}

		private const string installManifestName = "files.ini";
		private Thread installThread;

		private void installThreadRunner(object arg)
		{
			string[] args = (string[])arg;
			startInstallProcess (args[0], args[1]);
		}

		private void startInstallProcess (string installRoot, string flashDevelopRoot)
		{
			// Get list of files to install
			var installList = new InstallFileList ();
			installList.Load (installRoot);

			foreach (InstallerFile installFile in installList.Files)
			{
				string destPath = Path.Combine (flashDevelopRoot, installFile.File.Name);

			
				var targetDir = new DirectoryInfo (destPath).Parent;
				if (!targetDir.Exists)
					targetDir.Create();

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
