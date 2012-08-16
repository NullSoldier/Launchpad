using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore
{
	public class UpdateDownloader
	{
		public UpdateDownloader (Uri updateLocation)
		{
			this.updateLocation = updateLocation;
		}

		public void Download()
		{
			
		}

		private Uri updateLocation;
	}
}
