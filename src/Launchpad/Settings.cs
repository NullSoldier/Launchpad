using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LaunchPad
{
	[Serializable]
	public class Settings
	{
		[DisplayName("Selected Deploy Targets")]
		public List<Target> DeviceTargets
		{
			get { return targets; }
			set { targets = value; }
		}

		[DisplayName ("Deploy To Flash")]
		public bool DeployDefault
		{
			get { return deployDefault; }
			set { deployDefault = value; }
		}

		[DisplayName ("Deploy To Simulator")]
		public bool DeploySim
		{
			get { return deploySim; }
			set { deploySim = value; }
		}

		[DisplayName ("Check Updates Automatically")]
		public bool CheckForUpdates
		{
			get { return checkUpdates; }
			set { checkUpdates = value; }
		}

		[DisplayName ("Settings Version")]
		public Version SettingsVersion
		{
			get { return settingsVersion; }
			set { settingsVersion = value; }
		}

		private List<Target> targets = new List<Target>();
		private bool deployDefault = true;
		private bool deploySim = false;
		private bool checkUpdates = false;
		private Version settingsVersion;
	}
}