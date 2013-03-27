using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Launchpad.Helpers
{
	public static class Check
	{
		[Conditional ("DEBUG")]
		public static void ArgNull<T> (T value, string name)
			where T : class
		{
			if (value == null)
				throw new ArgumentNullException (name);
		}

		[Conditional ("DEBUG")]
		public static void IsNull<T> (T value)
			where T : class
		{
			if (value == null)
				throw new NoNullAllowedException ("Value cannot be null");
		}

		public static void IsPushable (DevicePlatform p)
		{
			switch (p)
			{
				case DevicePlatform.Android:
				case DevicePlatform.iOS:
					return;
				default:
					var msg = "You can only push to either Android or iOS";
					throw new ArgumentOutOfRangeException (msg);
			}
		}
	}
}
