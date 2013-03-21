using System;
using System.Collections.Generic;
using System.Text;

namespace Launchpad.Observable
{
	public interface IObservable<T>
	{
		IDisposable Subscribe (IObserver<T> o);
	}
}
