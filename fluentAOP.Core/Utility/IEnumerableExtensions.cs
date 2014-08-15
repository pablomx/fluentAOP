using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAop.Utility;
using Proxi;
using System.Collections;

namespace FluentAop
{
	static class IEnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
            Require.ArgumentNotNull("action", action);
			foreach (var c in collection) action(c);
		}
	}
}