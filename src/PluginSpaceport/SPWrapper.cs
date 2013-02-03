using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using PluginCore.Managers;
using PluginSpaceport.Helpers;
using PluginSpaceport.Properties;

namespace PluginSpaceport
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

			var process = CreatePushProcess ("sim", /*args*/string.Empty);
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

			var process = CreatePushProcess ("push", t.ID);
			process.OutputDataReceived += (s, ev) => output (ev.Data);
			process.Start ();
			process.BeginOutputReadLine ();
			return process;
		}


		public void GetDevicesNames (Action<IEnumerable<Target>> complete)
		{
			var targets = new List<Target> {
				new Target (/*name*/"Simulator", /*id*/"sim", DevicePlatform.Sim),
				new Target (/*name*/"Flash Player", /*id*/"flash", DevicePlatform.FlashPlayer),
				new Target (/*id*/"Some_Sample_Phone", DevicePlatform.iOS)
			};
			var process = CreatePushProcess (/*target*/null, /*args*/"");
			process.Start();

			string data = process.StandardOutput.ReadToEnd();

			complete (targets);
		}

		private readonly FileInfo sp;

		private Process CreatePushProcess(string cmd, string args)
		{
			var fullArgs = cmd + " "
				+ args + " ";

			var start = new ProcessStartInfo
			{
				FileName = sp.FullName,
				WorkingDirectory = ProjectDirectory,
				Arguments = fullArgs,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true
			};
			
			var process = new Process {StartInfo = start};
			return process;
		}
	}
}
