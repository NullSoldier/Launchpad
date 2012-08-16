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
		public event EventHandler<UnhandledExceptionEventArgs> CheckUpdateFailed;

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

		private const int checkSleepTime = 60000;
		private readonly Uri updateLocation;
		private readonly Version currentVersion;
		private Thread updateThread;
		private bool isCheckingUpdates;

		private void updateCheckRunner()
		{
			while (isCheckingUpdates)
			{
				string version;
				Exception ex;

				if (!UpdateHelper.TryDownloadString (this.updateLocation, out version, out ex))
				{
					onCheckUpdateFailed (ex);
					Thread.Sleep (checkSleepTime);
					continue;
				}

				//TODO: reduce point of failure at version string formatting
				var versionFound = new Version (version);
				if (versionFound <= currentVersion)
				{
					Thread.Sleep (checkSleepTime);
					continue;
				}

				Stop();
				onUpdateFound(versionFound);
				return;
			}
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
				handler (this, new UnhandledExceptionEventArgs (ex, false));
		}
	}
}
