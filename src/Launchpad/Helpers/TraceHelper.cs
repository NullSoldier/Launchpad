using System;
using System.Collections.Generic;
using System.Text;
using PluginCore;
using PluginCore.Managers;

namespace Launchpad.Helpers
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
	}
}
