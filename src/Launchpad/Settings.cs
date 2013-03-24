using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using PluginCore.Localization;

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

		private List<Target> targets = new List<Target>();
		private string installDir = "";
	}
}
