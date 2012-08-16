using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InstallerCore
{
	public class UpdateHelper
	{
		/// <summary>
		/// Downloads a file or web page on the internet as a string
		/// </summary>
		/// <param name="stringLocation">The address of the resource to download</param>
		/// <param name="value">The string downloaded</param>
		/// <param name="error">The exception if the method returned false</param>
		/// <returns>True if it succeeded, false if there was an exception</returns>
		public static bool TryDownloadString (Uri stringLocation, out string value, out Exception error)
		{
			error = null;
			value = null;

			using (var downloadClient = new WebClient())
			{
				try
				{
					value = downloadClient.DownloadString (stringLocation);
					return true;
				}
				catch (Exception ex)
				{
					error = ex;
					return false;
				}
			}
		}

		/// <summary>
		/// Downloads a file to a location on the local disk
		/// </summary>
		/// <param name="fileLocation">The address of the resource to download</param>
		/// <param name="destination">The destination on the disk to copy to</param>
		/// <param name="error">The error if the method returned false</param>
		/// <returns>True if it succeeded, false if there was an exception</returns>
		public static bool TryDownloadFile (Uri fileLocation, Uri destination, out Exception error)
		{
			error = null;

			using (var downloadClient = new WebClient ())
			{
				try
				{
					downloadClient.DownloadFile (fileLocation, destination.AbsolutePath);
					return true;
				}
				catch (Exception ex)
				{
					error = ex;
					return false;
				}
			}
		}
	}
}
