using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore.Rollback
{
	public class GenericEventArgs<T> : EventArgs
	{
		public GenericEventArgs (T value)
		{
			this.Value = value;
		}

		public T Value
		{
			get;
			private set;
		}
	}
}
