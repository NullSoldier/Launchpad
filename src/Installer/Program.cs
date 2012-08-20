using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PluginInstaller
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			InstallerArgsParser.Parse (Environment.GetCommandLineArgs(), new InstallerEntry());
		}
	}
}
