using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace InstallerCore
{
	public class UpdateRunner
	{
		public UpdateRunner (Uri updateLocation)
		{
			this.updateLocation = updateLocation;
		}

		public event EventHandler UpdateFound;
		public event EventHandler CheckUpdateStarted;
		public event EventHandler CheckUpdateStopped;
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
		private Thread updateThread;
		private bool isCheckingUpdates;

		private void updateCheckRunner()
		{
			while (isCheckingUpdates)
			{
				string version;
				Exception ex;

				if (UpdateHelper.TryDownloadString (this.updateLocation, out version, out ex))
				{
					onUpdateFound();
					Stop();
					return;
				}
				
				onCheckUpdateFailed (ex);
				Thread.Sleep (checkSleepTime);
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

		private void onUpdateFound ()
		{
			var handler = UpdateFound;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onCheckUpdateFailed (Exception ex)
		{
			var handler = CheckUpdateFailed;
			if (handler != null)
				handler (this, new UnhandledExceptionEventArgs (ex, false));
		}
	}
}
