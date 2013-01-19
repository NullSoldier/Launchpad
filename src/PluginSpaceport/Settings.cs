using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using PluginCore.Localization;

namespace PluginSpaceport
{
	[Serializable]
	public class Settings
	{
		[DisplayName("Spaceport Installation Directory")]
		[LocalizedDescription("StartPage.Description.CustomStartPage")]
		[LocalizedCategory("StartPage.Category.Custom")]
		[DefaultValue("teeset")]
		public string SpaceportInstallDir { get; set; }

		[DisplayName("Selected Deploy Targets")]
		public List<String> DeviceTargets
		{
			get { return targets; }
			set { targets = value; }
		}

		private List<string> targets = new List<string>();
	}
}
