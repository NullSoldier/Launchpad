using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InstallerCore
{
	public class UpdateHelper
	{
		public static bool TryDownloadString (Uri stringLocation, out string value, out Exception error)
		{
			error = null;
			value = null;
			WebClient downloadClient = null;
			
			try
			{
				downloadClient = new WebClient ();
				value = downloadClient.DownloadString (stringLocation);
				return true;
			}
			catch (Exception ex)
			{
				error = ex;
				return false;
			}
			finally
			{
				if (downloadClient != null)
					downloadClient.Dispose();
			}
		}
	}
}
