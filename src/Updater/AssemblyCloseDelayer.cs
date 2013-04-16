using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UpdaterCore;
using log4net;

namespace Updater
{
	public static class AssemblyCloseDelayer
	{
		public static void WaitForAssembliesAsync (Action closed, params string[] assemblyPaths)
		{
			var thread = new Thread(() => {
				var waitFor = GetProcessesByNameAssemblyNames (assemblyPaths);

				foreach (var process in waitFor) {
					logger.Debug ("Waiting for process to close: " + process.ProcessName);
					process.WaitForExit();
				}
				closed();
			});
			thread.Name = "Waiting on processes thread";
			thread.Start();
		}

		private static readonly ILog logger = LogManager.GetLogger (typeof (AssemblyCloseDelayer));

		private static IEnumerable<Process> GetProcessesByNameAssemblyNames (
			IEnumerable<string> assemblyNames)
		{
			foreach (string assemblyPath in assemblyNames) {
				foreach (Process process in GetProcessByAssemblyPath (assemblyPath))
					yield return process;
			}
		}

		private static IEnumerable<Process> GetProcessByAssemblyPath (string assemblyPath)
		{
			string knownPath = assemblyPath.ToLower();

			foreach (Process process in Process.GetProcesses())
			{
				string processAssemblyPath;
				try { processAssemblyPath = process.MainModule.FileName; }
				// We can't access 64bit or closed processes
				catch (Win32Exception) { continue; }
				catch (InvalidOperationException) { continue; }

				//TODO: Compare to file volumes instead for reliability?
				var assembly = new FileInfo (processAssemblyPath);
				if (assembly.FullName.ToLower() == knownPath)
					yield return process;
			}
		}
	}
}
