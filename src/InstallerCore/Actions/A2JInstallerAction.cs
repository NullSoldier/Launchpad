using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InstallerCore.Actions
{
	public class A2JInstallerAction : BaseInstallerAction
	{
		public override string FileName
		{
			get { return "tools/A2J.exe"; }
		}

		public override InstallAction InstallType
		{
			get { return InstallAction.InstallOrReplace; }
		}

		public override void Perform (string path)
		{
			string sourcePath = Path.Combine (WorkPath.FullName, path);
			string destPath = Path.Combine (FlashDataPath.FullName, path);

			File.Copy (sourcePath, destPath, true);
		}
	}
}
