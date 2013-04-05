using System;
using System.Collections.Generic;
using System.Text;

namespace Launchpad
{
	public static class SPPluginEvents
	{
		public const string Enabled = "SpaceportPlugin.Enabled";
		public const string Disabled = "SpaceportPlugin.Disabled";
		public const string ProjectOpened = "SpaceportPlugin.ProjectOpened";
		public const string ProjectClosed = "SpaceportPlugin.ProjectClosed";
		public const string StartDeploy = "SpaceportPlugin.Deploy";
		public const string StartBuild = "SpaceportPlugin.Build";
		public const string StartInstall = "SpaceportPlugin.Install";
	}
}
