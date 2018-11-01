using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.ExtensionMethods
{
    public static class ExtIEnumerable
    {
        public static IEnumerable<TSource> SelectRecursiveClever<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> recursiveSelector)
        {
            Stack<TSource> stack = new Stack<TSource>();
            source.Reverse().ForEach(stack.Push);
            while (stack.Count > 0)
            {
                TSource current = stack.Pop();
                yield return current;
                recursiveSelector(current).Reverse().ForEach(stack.Push);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source)
            {
                action(item);
            }
        }

        public static T ReturnFirstOrNull<T>(this IEnumerable<T> list)
        {
            if (list != null && list.Count() > 0)
                return list.First();
            else
                return default(T);
        }
    }
}
