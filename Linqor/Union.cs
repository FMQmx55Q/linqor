using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the union of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Union<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right)
            where TKey : IComparable<TKey>
        {
            return left.Union(right, (l, r) => l.CompareTo(r));
        }
        
        /// <summary>
        /// Produces the union of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Union<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            foreach(var item in left
                .Concat(right, compare)
                .AsOrderedBy(left.KeySelector)
                .Distinct((l, r) => compare(l, r) == 0)) yield return item;
        }
    }
}
