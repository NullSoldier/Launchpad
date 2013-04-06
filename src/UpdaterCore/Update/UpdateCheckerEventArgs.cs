﻿using System;
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

		public UpdateInformation UpdateInfo
		{
			get;
			private set;
		}

		public Version Version
		{
			get { return UpdateInfo.Version; }
		}

		public Uri CheckLocation
		{
			get;
			private set;
		}

		public Exception Exception
		{
			get;
			private set;
		}
	}
}
