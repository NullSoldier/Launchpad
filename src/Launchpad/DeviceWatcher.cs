using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;
using Launchpad.Helpers;
using Launchpad.Observable;
using PluginCore;
using PluginCore.Managers;
using Timer = System.Timers.Timer;

namespace Launchpad
{
	public class DeviceWatcher
		: IObservable<Target>
	{
		public DeviceWatcher (SPWrapper sp)
		{
			this.sp = sp;
			timer = new Timer (2000);
			timer.AutoReset = true;
			timer.Elapsed += onTimerElapsed;
		}

		public readonly List<Target> Active = new List<Target>();
		public bool IsWorking { get; private set; }
		public string LastError { get; private set; }

		public void Start()
		{
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
			if (listProcess != null)
				listProcess.Kill();
		}

		public IDisposable Subscribe (IObserver<Target> o)
		{
			subs.Add (o);
			foreach (var t in Active) {
				o.NotifyAdded (t);
			}
			return new Unsubscriber<Target> (subs, o);
		}

		private Process listProcess;
		private Thread startingThread;
		private StringBuilder workingError;
		private bool shownError;
		private bool starting;
		private readonly Timer timer;
		private readonly SPWrapper sp;
		private readonly List<IObserver<Target>> subs = new List<IObserver<Target>>();

		private void forgetAllDevices()
		{
			foreach (var device in Active.ToArray())
				processDevice (device, DiscoveryStatus.LOST);
			Active.Clear();
		}

		private void processDevice (Target target, DiscoveryStatus status)
		{
			switch (status) {
				case DiscoveryStatus.FOUND:
					Active.Add (target);
					subs.ForEach (s => s.NotifyAdded (target));
					break;
				case DiscoveryStatus.LOST:
					Active.Remove (target);
					subs.ForEach (s => s.NotifyRemoved (target));
					break;
			}
		}

		private void onTimerElapsed (object s, ElapsedEventArgs ev)
		{
			if (starting || IsWorking)
				return;

			startListProcessAsync();
		}

		private void startListProcessAsync()
		{
			starting = true;
			workingError = new StringBuilder();
			startingThread = new Thread (() => {
				var p = sp.StartGetDevicesNames (
					processDevice,
					onError,
					onExited);
				Thread.Sleep (2000);
				if (!p.HasExited)
					onStarted (p);
			});
			startingThread.Start();
		}

		private void onStarted (Process p)
		{
			starting = false;
			shownError = false;
			IsWorking = true;
			TraceHelper.TraceProcessStart ("device watcher", p);
			subs.ForEach (s => s.OnStart());
		}

		private void onExited (int i, Process p)
		{
			forgetAllDevices();
			startingThread.Abort();
			startingThread = null;
			listProcess = null;
			starting = false;
			IsWorking = false;
			LastError = workingError.ToString();

			if (i != 0 && !shownError) {
				shownError = true;
				TraceHelper.TraceProcessError ("device watcher", p, LastError);
				subs.ForEach (s => s.OnError());
			}
		}

		private void onError (string s)
		{
			if (s != null)
				workingError.Append (s + Environment.NewLine);
		}
	}
}
