using System;
using System.Collections.Generic;
using System.Text;
using UpdaterCore.Update;

namespace UpdaterCore
{
	public class UpdateCheckerEventArgs : EventArgs
	{
		public UpdateCheckerEventArgs (UpdateInformation updateInfo)
		{
			this.UpdateInfo = updateInfo;
		}

		public UpdateCheckerEventArgs (Uri checkLocation, Exception exception=null)
		{
			this.CheckLocation = checkLocation;
			this.Exception = exception;
		}

		public readonly UpdateInformation UpdateInfo;
		public readonly Uri CheckLocation;
		public readonly Exception Exception;
	}
}
