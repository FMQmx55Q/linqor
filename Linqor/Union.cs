using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the union of two ordered sequences.
        /// </summary>
        public static OrderedEnumerable<T, TKey> Union<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            OrderedEnumerable<T, TKey> right)
        {
            return Union(left, right, left.Comparer)
                .AsOrderedLike(left);
        }
        
        /// <summary>
        /// Produces the union of two ordered sequences.
        /// </summary>
        private static IEnumerable<T> Union<T>(
            IEnumerable<T> left,
            IEnumerable<T> right,
            IComparer<T> comparer)
        {
            return Distinct(Concat(left, right, comparer), comparer);
        }
    }
}
