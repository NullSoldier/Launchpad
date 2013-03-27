using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UpdaterCore;

namespace Launchpad.Helpers
{
	public static class SPHelper
	{
		public static Process RunOnTarget (this SPWrapper self, Target t,
			Action<string> output, 
			Action<string> errors,
			Action<int, Process> exited)
		{
			switch (t.Platform) {
				case DevicePlatform.iOS:
				case DevicePlatform.Android:
					return self.Push (t, output, errors, exited);
				case DevicePlatform.Sim:
					return self.Sim (output, errors, exited);
				default:
					throw new ArgumentOutOfRangeException ("t", "Can't push to this platform");
			}
		}
	}
}
