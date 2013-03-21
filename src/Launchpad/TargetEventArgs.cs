using System;
using System.Collections.Generic;
using System.Text;

namespace Launchpad
{
	public class TargetEventArgs : EventArgs
	{
		public TargetEventArgs(Target target)
		{
			this.TargetDevice = target;
		}

		public readonly Target TargetDevice;
	}
}
