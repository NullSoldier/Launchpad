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
	public class PushWrapper
	{
		public PushWrapper (string path)
		{
			this.push = new FileInfo (path);
			// Assume support is in the same directory as spaceport-push
			this.supportPath = Path.Combine (push.Directory.FullName,
				Resources.SupportName);
		}

		public string ProjectDirectory { get; set; }

		public Process PushTo (Target target, Action<string> output)
		{
			return RunOnTarget (target, output);
		}

		public void GetDevicesNames (Action<IEnumerable<Target>> complete)
		{
			var targets = new List<Target> {
				new Target ("sim", DevicePlatform.Sim),
				new Target ("Flash Player", DevicePlatform.FlashPlayer),
				new Target ("Some Sample Phone", DevicePlatform.iOS)
			};
			var process = CreatePushProcess (/*target*/null, /*args*/"");
			process.Start();

			string data = process.StandardOutput.ReadToEnd();

			complete (targets);
		}

		private FileInfo push;
		private string supportPath;

		private Process RunOnTarget(Target target, Action<string> output)
		{
			TraceManager.AddAsync (string.Format ("Running on target {0} ({1})",
				target.Name,
				target.Platform.GetString()));

			var process = CreatePushProcess (target, string.Empty);
			process.OutputDataReceived += (s, ev) => output (ev.Data);
			process.Start ();
			process.BeginOutputReadLine ();

			return process;
		}

		private Process CreatePushProcess(Target target, string extraArgs)
		{
			var args = (target!=null?target.Name+" ":"")
			    + extraArgs + " "
			    + "--support-path=\"" + supportPath + "\"";

			var start = new ProcessStartInfo
			{
				FileName = push.FullName,
				WorkingDirectory = ProjectDirectory,
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
