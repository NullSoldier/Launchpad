using System;
using System.Collections.Generic;
using System.Text;

namespace PluginSpaceport
{
	public class Target
	{
		public Target (string name, DevicePlatform platform)
		{
			this.Name = name;
			this.Platform = platform;
		}

		public readonly string Name;
		public readonly DevicePlatform Platform;

		public string PlatformName
		{
			get { return Enum.GetName (typeof (DevicePlatform), Platform); }
		}
	}

	public enum DevicePlatform
	{
		Unknown,
		iOS,
		Sim,
		Android
	}

}
