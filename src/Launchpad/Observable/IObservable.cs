using System;
using System.Collections.Generic;
using System.Text;

namespace LaunchPad.Observable
{
	public interface IObservable<T>
	{
		IDisposable Subscribe (IObserver<T> o);
	}
}
