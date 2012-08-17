using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore
{
	public static class IEnumerableHelper
	{
		public static int Count<T> (this IEnumerable<T> self)
		{
			IEnumerator<T> iterator = self.GetEnumerator();
			int count = 0;

			while (iterator.MoveNext())
				count++;

			return count;
		}
	}
}
