using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevicePlatform = LaunchPad.DevicePlatform;

namespace Launchpad
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

		public bool DeployDefault
		{
			get { return deployDefault; }
			set { deployDefault = value; }
		}

		public bool DeploySim
		{
			get { return deploySim; }
			set { deploySim = value; }
		}

		public bool CheckForUpdates
		{
			get { return checkUpdates; }
			set { checkUpdates = value; }
		}

		private List<Target> targets = new List<Target>();
		private bool deployDefault = true;
		private bool deploySim = false;
		private bool checkUpdates = false;
	}

	[Serializable]
	public class Target
	{
		public Target()
		{
		}

		public Target(string id, DevicePlatform platform)
			: this (id, string.Empty, platform)
		{
		}

		public Target(string id, string name, DevicePlatform platform)
		{
			
		}

		public readonly string Name;
		public readonly string ID;
		public readonly DevicePlatform Platform;
	}
}