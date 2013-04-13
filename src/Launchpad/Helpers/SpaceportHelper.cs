using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UpdaterCore;

namespace LaunchPad.Helpers
{
	public static class SpaceportHelper
	{
		public static bool TryExecuteCmd (
			this SPWrapper self,
			string cmd,
			out string error)
		{
			IEnumerable<String> lines;
			if (self.TryGetOutput (cmd, out lines)) {
				error = null;
				return true;
			}
			error = string.Join (Environment.NewLine, lines.ToArray());
			return false;
		}

		public static Process RunOnTarget (
			this SPWrapper self,
			Target t,
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

		public static Process RunOnTargets (
			this SPWrapper self,
			IEnumerable<Target> ts,
			Action<string> output, 
			Action<string> errors,
			Action<int, Process> exited)
		{
			var sim = ts.FirstOrDefault (t => t.Platform == DevicePlatform.Sim);
			var pushable = ts
				.Where (isPushable)
				.ToArray();

			if (pushable.Length == 0 && sim != null)
				return self.Sim (output, errors, exited);
			else if (pushable.Length > 0 && sim == null)
				return self.Push (ts, output, errors, exited);
			else if (pushable.Length > 0 && sim != null) {
				errors ("Failed to push: Pushing to both a SIM and a phone does not currently work.");
				return null;
			}
			
			throw new Exception ("Cannot run on no targets");
		}

		private static bool isPushable (Target t)
		{
			return t.Platform == DevicePlatform.iOS
				|| t.Platform == DevicePlatform.Android;
		}
	}
}
