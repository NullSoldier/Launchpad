using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Launchpad.Observable;

namespace Launchpad
{
	public class SPDeviceWatcher
		: IObservable<Target>
	{
		public SPDeviceWatcher (SPWrapper sp)
		{
			this.sp = sp;

			timer = new Timer (2000);
			timer.AutoReset = true;
			timer.Elapsed += (s, ev) =>
				sp.GetDevicesNames (ProcessDevices);
		}

		public readonly List<Target> Active = new List<Target> ();

		public IDisposable Subscribe (IObserver<Target> o)
		{
			subs.Add (o);
			foreach (var t in Active) {
				o.NotifyAdded (t);
			}
			return new Unsubscriber<Target> (subs, o);
		}

		public void Start ()
		{
			timer.Start();
		}
		
		public void Stop()
		{
			timer.Stop();
		}

		private readonly Timer timer;
		private readonly SPWrapper sp;
		private readonly List<IObserver<Target>> subs = new List<IObserver<Target>>();

		//TODO: use hashet and implement hash on target by using the name
		private void ProcessDevices(IEnumerable<Target> devices)
		{
			var current = new List<Target> (Active);

			foreach (var d in devices) {
				if (!current.Contains (d))
				{
					// We don't know about the device, a new device was found
					Active.Add (d);
					subs.ForEach (s => s.NotifyAdded (d));
				}
				current.Remove (d);
			}

			// Only items left are things that were removed
			foreach (var d in current) {
				Active.Remove (d);
				subs.ForEach (s => s.NotifyRemoved (d));
			}
		}
	}
}
