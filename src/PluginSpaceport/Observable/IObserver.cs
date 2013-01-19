using System;
using System.Collections.Generic;
using System.Text;

namespace PluginSpaceport.Observable
{
	public interface IObserver<T>
	{
		void NotifyAdded (T t);
		void NotifyRemoved (T item);
		void OnComplete();
		void OnError();
	}
}
