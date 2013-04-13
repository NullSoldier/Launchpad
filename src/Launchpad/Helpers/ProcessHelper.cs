using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using PluginCore.Managers;
using UpdaterCore;

namespace LaunchPad.Helpers
{
	public static class ProcessHelper
	{
		public static IEnumerable<string> ReadLinesToEnd (
			this StreamReader reader)
		{
			return reader
				.ReadToEnd()
				.Split (Environment.NewLine);
		}

		public static void StartReadAsync (this Process p,
			Action<string> output,
			Action<string> errors,
			Action<int, Process> exited)
		{
			if (output != null) {
				var outThread = new Thread (startReadAsync);
				outThread.Start (new Tuple<StreamReader, Action<String>>
					(p.StandardOutput, output));
			}
			if (errors != null) {
				var errorThread = new Thread (startReadAsync);
				errorThread.Start (new Tuple<StreamReader, Action<String>>
					(p.StandardError, errors));
			}
			if (exited != null) {
				p.Exited += (s, ev) => {
					var proc = (Process) s;
					while (!proc.StandardOutput.EndOfStream
					        || !proc.StandardError.EndOfStream)
					{
						Thread.Sleep (1);
					}
					exited (proc.ExitCode, proc);
				};
			}
		}

		private static void startReadAsync (
			object param)
		{
			var args = (Tuple<StreamReader, Action<String>>)param;
			readStreamAsync (args.Item1, args.Item2);
		}

		private static void readStreamAsync (
			StreamReader reader,
			Action<string> onReadLine)
		{
			while (true) {
				var o = reader.ReadLine ();
				if (o == null)
					return;
				onReadLine (o);
			}
		}
	}
}
