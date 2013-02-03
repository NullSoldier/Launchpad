#region License, Terms and Author(s)
//
// LINQBridge
// Copyright (c) 2007 Atif Aziz, Joseph Albahari. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// This library is free software; you can redistribute it and/or modify it 
// under the terms of the New BSD License, a copy of which should have 
// been delivered along with this distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using InstallerCore;

namespace PluginSpaceport.Helpers
{
	public static class EnumerableHelper
	{
		public static void ForEach<TSource> (this IEnumerable<TSource> self,
			Action<TSource> action)
		{
			foreach (var i in self) {
				action (i);
			}
		}

		/// <summary>
		/// Produces the set union of two sequences by using the default 
		/// equality comparer.
		/// </summary>
		public static IEnumerable<TSource> Union<TSource>(
			this IEnumerable<TSource> first,
			IEnumerable<TSource> second)
		{
			return Union (first, second, /* comparer */ null);
		}

		/// <summary>
		/// Produces the set union of two sequences by using a specified 
		/// <see cref="IEqualityComparer{T}" />.
		/// </summary>
		public static IEnumerable<TSource> Union<TSource>(
			this IEnumerable<TSource> first,
			IEnumerable<TSource> second,
			IEqualityComparer<TSource> comparer)
		{
			return first.Concat (second).Distinct (comparer);
		}

		/// <summary>
		/// Concatenates two sequences.
		/// </summary>

		public static IEnumerable<TSource> Concat<TSource>(
			this IEnumerable<TSource> first,
			IEnumerable<TSource> second)
		{
			if (first == null) throw new ArgumentNullException ("first");
			if (second == null) throw new ArgumentNullException ("second");

			return ConcatYield (first, second);
		}

		private static IEnumerable<TSource> ConcatYield<TSource>(
			IEnumerable<TSource> first,
			IEnumerable<TSource> second)
		{
			foreach (var item in first)
				yield return item;

			foreach (var item in second)
				yield return item;
		}


		/// <summary>
		/// Returns distinct elements from a sequence by using the default 
		/// equality comparer to compare values.
		/// </summary>

		public static IEnumerable<TSource> Distinct<TSource>(
			this IEnumerable<TSource> source)
		{
			return Distinct (source, /* comparer */ null);
		}

		/// <summary>
		/// Returns distinct elements from a sequence by using a specified 
		/// <see cref="IEqualityComparer{T}"/> to compare values.
		/// </summary>

		public static IEnumerable<TSource> Distinct<TSource>(
			this IEnumerable<TSource> source,
			IEqualityComparer<TSource> comparer)
		{
			if (source == null) throw new ArgumentNullException ("source");

			return DistinctYield (source, comparer);
		}

		private static IEnumerable<TSource> DistinctYield<TSource>(
			IEnumerable<TSource> source,
			IEqualityComparer<TSource> comparer)
		{
			var set = new Dictionary<TSource, object> (comparer);
			var gotNull = false;

			foreach (var item in source)
			{
				if (item == null)
				{
					if (gotNull)
						continue;
					gotNull = true;
				}
				else
				{
					if (set.ContainsKey (item))
						continue;
					set.Add (item, null);
				}

				yield return item;
			}
		}


		public static IEnumerable<T> Intersect<T> (this IEnumerable<T> A, IEnumerable<T> B)
		{
			foreach (var i in A) {
				foreach (var j in B) {
					if (i.Equals (j))
						yield return i;
				}
			}
		}       
	}
}
