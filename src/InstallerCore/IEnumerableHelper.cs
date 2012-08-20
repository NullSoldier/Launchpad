using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore
{
	public static class IEnumerableHelper
	{
		public delegate void Action();
		public delegate void Action<T1, T2>(T1 arg1, T2 arg2);
		public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
		public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
		public delegate TResult Func<TResult>();
		public delegate TResult Func<T, TResult>(T arg);
		public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
		public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
		public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
		
		public static int Count<T> (this IEnumerable<T> self)
		{
			IEnumerator<T> iterator = self.GetEnumerator();
			int count = 0;

			while (iterator.MoveNext())
				count++;

			return count;
		}

		public static T FirstOrDefault<T> (this IEnumerable<T> self, Func<T, bool> predicate)
		{
			IEnumerator<T> iterator = self.GetEnumerator ();

			while (iterator.MoveNext ()) {
				if (predicate (iterator.Current)) {
					return iterator.Current;
				}
			}

			return default(T);
		}

		public static T First<T>(this IEnumerable<T> self)
		{
			var value = FirstOrDefault (self, arg => true);

			if (value == null || value.Equals (default(T)))
				throw new InvalidOperationException("The sequence was empty.");

			return value;
		}

		public static IEnumerable<T> Where<T> (this IEnumerable<T> self, Func<T, bool> predicate)
		{
			IEnumerator<T> iterator = self.GetEnumerator();

			while (iterator.MoveNext()) {
				if (predicate (iterator.Current)) {
					yield return iterator.Current;
				}
			}
		}

		public static IEnumerable<TResult> Cast<T, TResult> (this IEnumerable<T> self)
		{
			IEnumerator<T> iterator = self.GetEnumerator();

			while (iterator.MoveNext ()) {
				yield return (TResult)Convert.ChangeType (iterator.Current, typeof (TResult));
			}
		}
	}
}
