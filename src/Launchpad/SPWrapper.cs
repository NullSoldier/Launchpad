using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
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

		public string ProjectDirectory { get; set; }

		public Process Sim (
			Action<string> output, 
			Action<string> errors,
			Action<int, Process> exited)
		{
			return StartProcess ("sim", output, errors, exited);
		}

		public Process Push (Target t,
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			return Push (new [] {t}, output, errors, exited);
		}

		public Process Push (IEnumerable<Target> ts,
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			StringBuilder builder = new StringBuilder();
			foreach (var t in ts) {
				Check.IsPushable (t.Platform);
				builder.Append (" ");
				builder.Append (t.ID);
			}
			return StartProcess ("push " + builder, output, errors, exited);
		}

		public Process Build (
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			return StartProcess ("build", output, errors, exited);
		}

		public Process StartGetDevicesNames (
			Action<Target, DiscoveryStatus> found)
		{
			var parseDevice = new Action<string> (o => {
				if (o == null) {
					return;
				}
				var target = new Target (o.Substring (1),
					DevicePlatform.iOS);
				var status = o.StartsWith ("+") ?
					DiscoveryStatus.FOUND :
					DiscoveryStatus.LOST;
				found (target, status);
			});
			return StartProcess ("push --list", parseDevice, null, null);
		}

		public Process InstallToDevice (
			DevicePlatform platform,
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			string device;
			switch  (platform) {
				case DevicePlatform.iOS: device = "ios"; break;
				case DevicePlatform.Android: device = "android"; break;
				default: throw new ArgumentOutOfRangeException ("platform");
			}
			var p = StartProcess ("install " + device, output, errors, exited);
			p.StandardInput.Close();
			return p;
		}

		public string GetFirstOutput (string cmd)
		{
			var lines = GetOutput (cmd);
			return lines == null ? null
				: lines.FirstOrDefault();
		}

		public IEnumerable<string> GetOutput (string cmd)
		{
			var p = StartProcess (cmd, null, null, null);
			p.WaitForExit();
			if (p.ExitCode != SUCCESS_CODE)
				return null;
			return p.StandardOutput
				.ReadToEnd()
				.Split (Environment.NewLine);
		}

		private readonly FileInfo sp;
		private const int SUCCESS_CODE = 0;

		private Process StartProcess (string cmd,
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			var start = new ProcessStartInfo
			{
				FileName = sp.FullName,
				WorkingDirectory = ProjectDirectory,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				Arguments = cmd
			};
			
			var process = new Process();
			process.StartInfo = start;
			process.EnableRaisingEvents = true;
			process.Start ();

			if (exited != null) {
				process.Exited += (s, ev) => exited (((Process)s).ExitCode, (Process)s);
			}
			if (output != null) {
				process.OutputDataReceived += (s, ev) => output (ev.Data);
				process.BeginOutputReadLine();
			}
			if (errors != null) {
				process.ErrorDataReceived += (s, ev) => errors (ev.Data);
				process.BeginErrorReadLine();
			}
			return process;
		}
	}
}
