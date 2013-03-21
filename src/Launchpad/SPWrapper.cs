using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UpdaterCore;
using PluginCore.Managers;
using Launchpad.Helpers;
using Launchpad.Properties;

namespace Launchpad
{
	public class SPWrapper
	{
		public SPWrapper (string path)
		{
			sp = new FileInfo (path);
		}

		public string ProjectDirectory { private get; set; }

		public Process Sim (Action<string> output)
		{
			TraceManager.AddAsync ("Running on simulator");

			var process = CreateProcess (/*cmd*/"sim", /*args*/string.Empty);
			process.OutputDataReceived += (s, ev) => output (ev.Data);
			process.Start ();
			process.BeginOutputReadLine ();
			return process;
		}

		public Process Push (Target t, Action<string> output)
		{
			if (t.Platform != DevicePlatform.Android
				&& t.Platform != DevicePlatform.iOS)
			{
				throw new ArgumentOutOfRangeException ("t",
					"PushToDevice only supports devices on wifi");
			}

			TraceManager.AddAsync (string.Format ("Running on device {0} ({1})",
				t.Name, t.Platform.GetString ()));

			var process = CreateProcess ("push", t.ID);
			process.OutputDataReceived += (s, ev) => output (ev.Data);
			process.Start ();
			process.BeginOutputReadLine ();
			return process;
		}

		public void GetDevicesNames (Action<IEnumerable<Target>> complete)
		{
			var process = CreateProcess (/*cmd*/"push", /*args*/"");
			process.Start();

			string data = process.StandardOutput.ReadToEnd();

			IEnumerable<Target> targets = data.Split (new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
				.Select (i => new Target (i, DevicePlatform.iOS))
				.Concat (new [] {
					new Target ("sim", "Simulator", DevicePlatform.Sim),
					new Target ("flash", "FlashPlayer", DevicePlatform.FlashPlayer)
				});

			complete (targets);
		}

		private readonly FileInfo sp;

		private Process CreateProcess(string cmd, string args)
		{
			var fullArgs = cmd + " "
				+ args + " ";

			var start = new ProcessStartInfo
			{
				FileName = sp.FullName,
				WorkingDirectory = ProjectDirectory,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				Arguments = fullArgs
			};
			
			var process = new Process {StartInfo = start};
			return process;
		}
	}
}
