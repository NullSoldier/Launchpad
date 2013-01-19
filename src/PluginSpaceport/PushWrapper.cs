using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using PluginSpaceport.Properties;

namespace PluginSpaceport
{
	public class PushWrapper
	{
		public PushWrapper (string path)
		{
			this.push = new FileInfo (path);
			this.supportPath = Path.Combine (push.Directory.FullName, Resources.SupportName);
			// Assume support is in the same directory as spaceport-push
		}

		public void PushToDevice(string name, Action<string> output)
		{
			RunOnTarget (name, output);
		}

		public Process PushToSim(Action<string> output)
		{
			return RunOnTarget ("sim", output);
		}

		public void GetDevicesNames (Action<IEnumerable<Target>> recievedDevices)
		{
			var targets = new List<Target>();
			var process = CreatePushProcess ("", "");
			process.Start();

			string data = process.StandardOutput.ReadToEnd();
			targets.Add (new Target ("SampleDevice", DevicePlatform.Unknown));

			recievedDevices (targets);
		}

		private FileInfo push;
		private string supportPath;

		private Process RunOnTarget(string name, Action<string> output)
		{
			var process = CreatePushProcess (name, string.Empty);
			process.OutputDataReceived += (s, ev) => {
				output (ev.Data);
			};
			process.Start ();
			return process;
		}

		private Process CreatePushProcess(string target, string extraArgs)
		{
			var args = target + " "
			    + extraArgs + " "
			    + "-support-path=\"" + supportPath + "\"";

			var start = new ProcessStartInfo
			{
				FileName = push.FullName,
				Arguments = args,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true
			};
			
			var process = new Process {StartInfo = start};
			return process;
		}
	}
}
