using System;
using System.Collections.Generic;
using System.Text;

namespace Launchpad.Observable
{
	public class Unsubscriber<T> : IDisposable
	{
		public Unsubscriber(List<IObserver<T>> subs, IObserver<T> sub)
		{
			this.subs = subs;
			this.sub = sub;
		}

		public void Dispose()
		{
			subs.Remove (sub);
		}

		private List<IObserver<T>> subs;
		private IObserver<T> sub;
	}
}
