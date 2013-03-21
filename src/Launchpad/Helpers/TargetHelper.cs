using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Launchpad.Helpers
{
	public static class TargetHelper
	{
		public static string GetString (this DevicePlatform p)
		{
			return Enum.GetName (typeof (DevicePlatform), p);
		}

		public static void Add (this ImageList.ImageCollection ic,
			DevicePlatform p, Image i)
		{
			ic.Add (GetString (p), i);
		}
	}
}
