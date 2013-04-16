using System;
using System.Collections.Generic;
using System.Text;

namespace UpdaterCore
{
	public static class UriHelper
	{
		public static Uri Append (this Uri self, string path)
		{
			return new Uri (self, path);
		}
	}
}
