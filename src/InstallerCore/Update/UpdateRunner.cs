using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace InstallerCore
{
	public class UpdateRunner
	{
		public UpdateRunner (Uri updateLocation, Version currentVersion)
		{
			this.updateLocation = updateLocation;
			this.currentVersion = currentVersion;
		}

		public event EventHandler CheckUpdateStarted;
		public event EventHandler CheckUpdateStopped;
		public event EventHandler<UpdateCheckerEventArgs> UpdateFound;
		public event EventHandler<UpdateCheckerEventArgs> CheckUpdateFailed;

		public void Start()
		{
			isCheckingUpdates = true;

			updateThread = new Thread (updateCheckRunner);
			updateThread.Start();

			onStartCheckUpdate();
		}

		public void Stop()
		{
			if (!isCheckingUpdates)
				return;
			
			isCheckingUpdates = false;
			onStopCheckUpdate();
		}

		public  bool TryCheckOnceForUpdate (out Version versionFound, Version currentVersion)
		{
			Exception ex;
			
			if (!TryDownloadVersion (out versionFound, out ex))
			{
				onCheckUpdateFailed (ex);
				return false;
			}

			return versionFound > currentVersion;
		}

		private const int checkSleepTime = 60000;
		private readonly Uri updateLocation;
		private readonly Version currentVersion;
		private Thread updateThread;
		private bool isCheckingUpdates;

		private void updateCheckRunner()
		{
			while (isCheckingUpdates)
			{
				Version versionFound;
				Exception ex;

				if (!TryDownloadVersion (out versionFound, out ex))
				{
					onCheckUpdateFailed (ex);
					Thread.Sleep (checkSleepTime);
					continue;
				}

				if (versionFound <= currentVersion)
				{
					Thread.Sleep (checkSleepTime);
					continue;
				}

				Stop();
				onUpdateFound (versionFound);
				return;
			}
		}

		private bool TryDownloadVersion (out Version versionFound, out Exception ex)
		{
			string version;

			if (!UpdateHelper.TryDownloadString (this.updateLocation, out version, out ex))
			{
				versionFound = null;
				return false;
			}

			//TODO: reduce point of failure at version string formatting
			versionFound = new Version (version);
			return true;
		}

		private void onStartCheckUpdate()
		{
			var handler = CheckUpdateStarted;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}
	
		private void onStopCheckUpdate()
		{
			var handler = CheckUpdateStopped;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onUpdateFound (Version version)
		{
			var handler = UpdateFound;
			if (handler != null)
				handler (this, new UpdateCheckerEventArgs (version));
		}

		private void onCheckUpdateFailed (Exception ex)
		{
			var handler = CheckUpdateFailed;
			if (handler != null)
				handler (this, new UpdateCheckerEventArgs (updateLocation, ex));
		}
	}
}
