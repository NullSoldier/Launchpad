using System;
using System.Collections.Generic;
using System.Text;

namespace UpdaterCore
{
	public static class StringHelper
	{
		public static string[] Split(this string self, string split)
		{
			return self.Split (new[] { split },
				StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
