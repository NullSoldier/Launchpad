using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PluginSpaceport.Helpers
{
	public static class SPHelper
	{
		public static Process RunOnTarget (this SPWrapper self, Target t, Action<string> output)
		{
			switch (t.Platform)
			{
				case DevicePlatform.Sim:	 return self.Sim  (output);
				case DevicePlatform.iOS:	 return self.Push (t, output);
				case DevicePlatform.Android: return self.Push (t, output);
				default: throw new ArgumentOutOfRangeException ("t", "Can't push to this platform");
			}
		}
	}
}
