using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PluginCore.Helpers;

namespace Launchpad
{
	public class LaunchpadPaths
	{
		public static string SettingsPath
		{
			get { return Path.Combine (PathHelper.DataDir, settings); }
		}

		public static string SpaceportPath
		{
			get { return Path.Combine (PathHelper.ToolDir, spaceport); }
		}

		public static DirectoryInfo CreateParentFolder (String path)
		{
			return Directory.CreateDirectory (new FileInfo (path).DirectoryName);
		}

		private const string settings = @"Launchpad\Settings.fdb";
		private const string spaceport = @"spaceport-sdk\sp.exe";
	}
}
