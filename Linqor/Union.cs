using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the set union of two ordered sequences.
        /// </summary>
        /// <param name="left">
        /// An ordered sequence whose distinct elements
        /// form the first set for the union.
        /// </param>
        /// <param name="right">
        /// A sequence that follows same ordering rules as the first sequence
        /// whose distinct elements
        /// form the second set for the union.
        /// </param>
        public static OrderedEnumerable<T, TKey> Union<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            IEnumerable<T> right)
        {
            return Union(left, right.AsOrderedLike(left), left.Comparer)
                .AsOrderedLike(left);
        }

        private static IEnumerable<T> Union<T>(
            IEnumerable<T> left,
            IEnumerable<T> right,
            IComparer<T> comparer)
        {
            return Distinct(Concat(left, right, comparer), comparer);
        }
    }
}
