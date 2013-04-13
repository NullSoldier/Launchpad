using System;
using UpdaterCore;
using log4net;
using log4net.Core;
using Settings_1_0 = Launchpad.Settings;
using Target_1_0 = Launchpad.Target;

namespace LaunchPad
{
	public static class SettingsUpgrader
	{
		public static Settings Convert (object settings)
		{
			Settings converted = null;
			Version version;

			if (settings is Settings_1_0)
				From_1_0 ((Settings_1_0)settings, out version, out converted);

			return converted ?? (Settings)settings;
		}

		private static ILog logger = LogManager.GetLogger (typeof (Settings));

		private static void From_1_0 (Settings_1_0 s,
			out Version v, out Settings c)
		{
			v = new Version (1, 0, 1);
			c = new Settings {
				CheckForUpdates = s.CheckForUpdates,
				DeployDefault = s.DeployDefault,
				DeploySim = s.DeploySim,
			};
			c.DeviceTargets = s.DeviceTargets.Select<Target_1_0, Target> (t =>
				new Target (t.Name, t.Name, t.Platform)).ToList();

			logger.Info ("Upgraded settings to " + v);
		}
	}
}
