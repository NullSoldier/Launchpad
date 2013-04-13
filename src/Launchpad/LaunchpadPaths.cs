using System;
using System.IO;
using PluginCore.Helpers;

namespace LaunchPad
{
	public static class LaunchpadPaths
	{
		public static string SettingsPath
		{
			get { return Path.Combine (PathHelper.DataDir,
				"Launchpad\\Settings.fdb"); }
		}

		public static string SpaceportPath
		{
			get { return Path.Combine (PathHelper.ToolDir,
				"spaceport-sdk\\sp.exe"); }
		}

		public static DirectoryInfo CreateParentFolder (String path)
		{
			string dir = new FileInfo (path).DirectoryName;
			return Directory.CreateDirectory (dir);
		}
	}
}
