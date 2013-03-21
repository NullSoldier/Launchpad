﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UpdaterCore
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

		public static void ForEach<T> (
			this IEnumerable<T> self,
			Action<T> p)
		{
			foreach (var t in self) {
				p (t);
			}
		}

		public static int Count<T> (this IEnumerable<T> self)
		{
			int count = 0;
			var iter = self.GetEnumerator();
			while (iter.MoveNext()) {
				count++;
			}
			return count;
		}

		public static T FirstOrDefault<T> (
			this IEnumerable<T> self,
			Func<T, bool> p)
		{
			var iterator = self.GetEnumerator ();

			while (iterator.MoveNext ()) {
				if (p (iterator.Current)) {
					return iterator.Current;
				}
			}
			return default(T);
		}

		public static T First<T> (this IEnumerable<T> self)
		{
			var value = FirstOrDefault (self, arg => true);
			if (value == null || value.Equals (default (T))) {
				throw new InvalidOperationException ("The sequence was empty.");
			}
			return value;
		}

		public static IEnumerable<T> Where<T> (
			this IEnumerable<T> self,
			Func<T, bool> p)
		{
			var iterator = self.GetEnumerator();

			while (iterator.MoveNext()) {
				if (p (iterator.Current)) {
					yield return iterator.Current;
				}
			}
		}

		public static IEnumerable<TResult> Cast<T, TResult> (
			this IEnumerable<T> self)
		{
			var iterator = self.GetEnumerator();

			while (iterator.MoveNext ()) {
				yield return (TResult)Convert.ChangeType (iterator.Current, typeof (TResult));
			}
		}

		public static IEnumerable<TResult> Select<T, TResult> (
			this IEnumerable<T> self,
			Func<T, TResult> predicate)
		{
			foreach (T value in self) {
				yield return predicate (value);
			}
		}

		/// <summary>
		/// Concatenates two sequences.
		/// </summary>
		public static IEnumerable<TSource> Concat<TSource>(
			this IEnumerable<TSource> a,
			IEnumerable<TSource> b)
		{
			if (a == null) throw new ArgumentNullException ("a");
			if (b == null) throw new ArgumentNullException ("b");
			return ConcatYield (a, b);
		}

		private static IEnumerable<TSource> ConcatYield<TSource> (
			IEnumerable<TSource> a,
			IEnumerable<TSource> b)
		{
			foreach (var item in a) yield return item;
			foreach (var item in b) yield return item;
		}

		public static IEnumerable<T> Intersect<T> (
			this IEnumerable<T> a,
			IEnumerable<T> b)
		{
			foreach (var i in a) {
			foreach (var j in b) {
				if (i.Equals (j)) {
					yield return i;
				}
			}}
		}
	}
}