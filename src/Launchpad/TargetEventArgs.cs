using System;
using System.Collections.Generic;
using System.Text;

namespace LaunchPad
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
