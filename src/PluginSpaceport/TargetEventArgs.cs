using System;
using System.Collections.Generic;
using System.Text;

namespace PluginSpaceport
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
