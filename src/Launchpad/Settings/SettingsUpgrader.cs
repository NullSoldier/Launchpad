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

			if (settings is Settings_1_0)
				v1_0_0_0v1_0_1_0 ((Settings_1_0)settings, out converted);

			return converted ?? (Settings)settings;
		}

		private static ILog logger = LogManager.GetLogger (typeof (Settings));

		private static void v1_0_0_0v1_0_1_0 (Settings_1_0 s, out Settings c)
		{
			c = new Settings {
				CheckForUpdates = s.CheckForUpdates,
				DeployDefault = s.DeployDefault,
				DeploySim = s.DeploySim,
				SettingsVersion = new Version (1, 0, 1)
			};
			c.DeviceTargets = s.DeviceTargets.Select (t =>
				new Target (t.Name, t.Name, t.Platform)).ToList();

			logger.Info ("Upgraded settings to " + c.SettingsVersion);
		}
	}
}
