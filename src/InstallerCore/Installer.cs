using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

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

		private const string installManifestName = "installList.ini";
		private Thread installThread;

		private void installThreadRunner(object arg)
		{
			string[] args = (string[])arg;
			startInstallProcess (args[0], args[1]);
		}

		private void startInstallProcess (string installRoot, string flashDevelopRoot)
		{
			// Get list of files to install
			string installManifestPath = Path.Combine (installRoot, installManifestName);
			IniWrapper iniWrapper = new IniWrapper (installManifestPath);

			foreach (IniParser.KeyData kvp in iniWrapper.GetSection ("Files"))
			{
				string sourcePath = Path.Combine (installRoot, kvp.KeyName);
				string destPath = Path.Combine (flashDevelopRoot, kvp.Value);

				File.Copy (sourcePath, destPath, true);
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
