using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns distinct elements from an ordered sequence.
        /// </summary>
        /// <param name="source">The ordered sequence to remove duplicate elements from.</param>
        public static OrderedEnumerable<T, TKey> Distinct<T, TKey>(
            this OrderedEnumerable<T, TKey> source)
        {
            return Distinct<T>(source, source.Comparer)
                .AsOrderedLike(source);
        }

        private static IEnumerable<T> Distinct<T>(
            IEnumerable<T> source,
            IComparer<T> comparer)
        {
            using (var enumerator = source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    yield return state.Current;
                    state = enumerator.SkipWhile(current => comparer.Compare(state.Current, current) == 0);
                }
            }
        }
    }
}
