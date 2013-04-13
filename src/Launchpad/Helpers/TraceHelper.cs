using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using PluginCore;
using PluginCore.Managers;

namespace LaunchPad.Helpers
{
	public static class TraceHelper
	{
		public static void Trace (string msg, TraceType type)
		{
			if (msg != null)
				TraceManager.AddAsync (msg, (int)type);
		}

		public static void TraceError (string msg)
		{
			Trace (msg, TraceType.Error);
		}

		public static void TraceInfo (string msg)
		{
			Trace (msg, TraceType.Info);
		}

		public static void TraceProcessStart (string name, Process p)
		{
			var msg = String.Format ("Started {0} process ({1}).", name, p.Id);
			Trace (msg, TraceType.ProcessStart);
		}

		public static void TraceProcessEnd (string name, Process p)
		{
			var msg = String.Format ("{0} process ({1}) exited with code {2}.",
				name, p.Id, p.ExitCode);
			Trace (msg, TraceType.ProcessEnd);
		}

		public static void TraceProcessError (string name, Process p, string error=null)
		{
			var msg = String.Format ("{0} process ({1}) terminated with code {2}.",
				name, p.Id, p.ExitCode);
			if (error != null) {
				msg += Environment.NewLine + error;
			}
			Trace (msg, TraceType.ProcessError);
		}
	}
}
