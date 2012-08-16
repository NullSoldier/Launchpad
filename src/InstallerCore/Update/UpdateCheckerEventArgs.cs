using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore
{
	public class UpdateCheckerEventArgs : EventArgs
	{
		public UpdateCheckerEventArgs (Version version)
		{
			this.Version = version;
		}

		public UpdateCheckerEventArgs (Uri checkLocation, Exception exception)
		{
			this.CheckLocation = checkLocation;
			this.Exception = exception;
		}

		public Version Version
		{
			get;
			private set;
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
