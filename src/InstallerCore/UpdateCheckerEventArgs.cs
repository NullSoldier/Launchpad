using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore
{
	public class UpdateCheckerEventArgs : EventArgs
	{
		public UpdateCheckerEventArgs(Version version)
		{
			this.Version = version;
		}

		public Version Version
		{
			get;
			private set;
		}
	}
}
